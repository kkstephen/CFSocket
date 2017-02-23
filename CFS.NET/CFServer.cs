using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CFS.Net
{
    public abstract class CFServer : IDisposable
    { 
        public event EventHandler<ServerStartEventArgs> OnStart;
        public event EventHandler<ServerStopEventArgs> OnStop;
        public event EventHandler<ClientConnectEventArgs> OnConnect;
        public event EventHandler<DisconnectEventArgs> OnDisconnect;
        public event EventHandler<CFErrorEventArgs> OnServerError;
        public event EventHandler<CFErrorEventArgs> OnClientError;         

        private TcpListener m_listener;
 
        public ConcurrentDictionary<string, ICFSession> Sessions
        {
            get; private set;            
        } 
 
        public string Host { get; private set; }
        public int Port { get; private set; }
        
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
            this.Port = port;
            this.Host = host;      

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

                if (OnStart != null)
                {
                    OnStart(this, new ServerStartEventArgs("Server Ready"));
                }     
            }
            catch(Exception ex)
            {
                this.serverError(this, new CFErrorEventArgs(ex.Message));
            }
        }
        
        public abstract void Process(TcpClient client);

        public async virtual void Run()
        {             
            try
            {
                while (!this.m_stop)
                {                                        
                    var client = await this.m_listener.AcceptTcpClientAsync();

                    this.Process(client);                   
                }             
            }
            catch(Exception)
            {
                this.m_stop = true; 
            }            

            if (OnStop != null)
            {
                OnStop(this, new ServerStopEventArgs("Server Stop"));
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
                session.Close();
            } 
        }

        #region server  
        protected void onConnectServer(object sender, ClientConnectEventArgs e)
        {
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
            if (OnClientError != null)
            {
                OnClientError(sender, e);
            }
        }

        protected void serverError(object sender, CFErrorEventArgs e)
        {
            if (OnServerError != null)
            {
                OnServerError(sender, e);
            }
        }

        #endregion 
    }
}
