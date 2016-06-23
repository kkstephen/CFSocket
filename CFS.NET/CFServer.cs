using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CFS.Net
{
    public abstract class CFServer : IDisposable
    {
        public delegate void OnServerError(object sender, CFErrorEventArgs e);
        public event OnServerError ServerError;

        public delegate void OnStart(object sender, StartEventArgs e);
        public event OnStart OnServerStart;

        public delegate void OnStop(object sender, StopEventArgs e);
        public event OnStop OnServerStop;

        public delegate void OnConnectionAccept(object sender, ClientConnectEventArgs e);
        public event OnConnectionAccept OnConnect;
 
        public delegate void OnClientDisconnect(object sender, DisconnectEventArgs e);
        public event OnClientDisconnect OnDisconnect;

        public delegate void OnClientError(object sender, CFErrorEventArgs e);
        public event OnClientError ClientError;

        private TcpListener m_listener;
        private UdpClient m_pushClient;
                
        public ConcurrentDictionary<string, ICFSession> Sessions
        {
            get; private set;            
        } 

        public Thread serverThread;

        public string Host { get; set; }
        public int Port { get; private set; }
        public int Status { get; private set; }

        private volatile bool m_stop;

        public bool IsRunning
        {
            get
            {
                return !this.m_stop;
            }
        }

        private bool disposed = false;

        private static readonly object _object = new object();

        public CFServer(string host, int port, int status_port)
        {
            Port = port;
            Host = host;
            Status = status_port;

            this.m_stop = true;
       
            this.Sessions = new ConcurrentDictionary<string, ICFSession>();

            IPEndPoint svrIP = new IPEndPoint(IPAddress.Parse(Host), Port);

            this.m_listener = new TcpListener(svrIP);

            IPEndPoint statusIP = new IPEndPoint(IPAddress.Parse(Host), Status);

            this.m_pushClient = new UdpClient(statusIP);
            this.m_pushClient.Ttl = 64;
        }
  
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed resources 
                    if (this.m_listener != null)
                    {
                        this.m_listener.Server.Dispose();
                        this.m_listener = null;
                    }

                    if (this.m_pushClient.Client != null)
                    {                        
                        this.m_pushClient.Client.Dispose();
                        this.m_pushClient = null;
                    }
                     
                    if (this.Sessions != null)
                    {
                        this.Sessions.Clear();
                        this.Sessions = null;
                    }                          
                }

                // Free your own state (unmanaged objects).
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void Start();
        
        public abstract void Process(TcpClient client);

        protected async void Run()
        {
            if (OnServerStart != null)
            {
                OnServerStart(this, new StartEventArgs("Server ready"));
            }         

            while (true)
            {
                try
                {
                    var client = await this.m_listener.AcceptTcpClientAsync();

                    if (!m_stop)
                    { 
                        this.Process(client);                                  
                    }
                    else
                    {
                        client.Close();
                    }
                }
                catch
                {
                    this.m_stop = true;

                    break;
                }
            }

            if (OnServerStop != null)
            {
                OnServerStop(this, new StopEventArgs("Server stop"));
            }
        }

        public void Stop()
        { 
            this.m_stop = true;     
            
            this.m_listener.Stop();
            this.m_pushClient.Close(); 
        }
   
        public void Initialize()
        {
            this.Sessions.Clear();
            
            this.m_listener.Start();

            this.m_stop = false;
        }

        public void Push(string message)
        { 
            foreach (ICFSession session in this.Sessions.Values)
            {
                if (this.m_stop)
                    break;

                if (session.IsAlive && session.PushHost != null)
                    this.m_pushClient.Send(Encoding.Default.GetBytes(message), message.Length, session.PushHost);
            }                             
        }

        public void Clear()
        { 
            foreach (var session in this.Sessions.Values)
            {
                if (session.IsAlive)
                    session.Close();
            }                  
        }

        public void Abort(string sessionId)
        {
            ICFSession session = null;

            if (this.Sessions.TryGetValue(sessionId, out session))
            {
                if (session.IsAlive)
                    session.Close();
            } 
        }

        #region server 
 
        protected void onConnectServer(object sender, ClientConnectEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    e.Session.Begin();
                },
                    null
            );   

            if (OnConnect != null)
            {
                OnConnect(sender, e);
            }
        }  

        protected void onDisconnectServer(object sender, DisconnectEventArgs e)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(sender, e);
            }
        }

        protected void clientError(object sender, CFErrorEventArgs e)
        {
            if (ClientError != null)
            {
                ClientError(sender, e);
            }
        }

        protected void serverError(object sender, CFErrorEventArgs e)
        {
            if (ServerError != null)
            {
                ServerError(sender, e);
            }
        }

        #endregion 
    }
}
