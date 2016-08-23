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
                
        public ConcurrentDictionary<string, ICFSession> Sessions
        {
            get; private set;            
        } 

        public Thread serverThread;

        public string Host { get; set; }
        public int Port { get; set; }
        
        private IPEndPoint svrIP;   

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

        public CFServer(string host, int port)
        {
            Port = port;
            Host = host;      

            this.m_stop = true;

            this.svrIP = new IPEndPoint(IPAddress.Parse(Host), Port); 

            this.Sessions = new ConcurrentDictionary<string, ICFSession>(); 
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

        public void Start()
        {
            this.Sessions.Clear();

            this.m_stop = false;
                        
            try
            {
                this.m_listener = new TcpListener(this.svrIP);

                this.m_listener.Start();
                
                this.Run();
            }
            catch(Exception ex)
            {
                this.serverError(this, new CFErrorEventArgs(ex.Message));
            }
        }
        
        public abstract void Process(TcpClient client);

        protected virtual async void Run()
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
                catch(Exception ex)
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

            if (this.m_listener != null)
            {
                this.m_listener.Stop();
                this.m_listener = null;
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
