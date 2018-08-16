using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFServer : ICFServer
    { 
        public event EventHandler<ServerStartEventArgs> OnStart;
        public event EventHandler<ServerStopEventArgs> OnStop;
        public event EventHandler<ClientConnectEventArgs> OnConnect;
        public event EventHandler<ClientDisconnectEventArgs> OnDisconnect;
        public event EventHandler<CFErrorEventArgs> OnServerError;
        public event EventHandler<CFErrorEventArgs> OnClientError;
        public event EventHandler<DataReceivedEventArgs> OnDataReceived;

        private TcpListener m_listener;
 
        public ConcurrentDictionary<string, ICFSession> Sessions
        {
            get; private set;            
        } 
 
        public string Host { get; set; }
        public int Port { get; set; }
        
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

        public CFServer()
        {
            this.m_stop = true;

            this.Sessions = new ConcurrentDictionary<string, ICFSession>(); 
        } 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed resources 
                    this.close();

                    if (this.Sessions != null)
                    {
                        this.Sessions.Clear();                       
                    }                          
                }

                // Free your own state (unmanaged objects).
                this.Sessions = null;

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
                                    
            try
            {
                var ep = new IPEndPoint(IPAddress.Parse(Host), Port);

                this.m_listener = new TcpListener(ep);
                 
                this.m_listener.Start();

                this.m_stop = false;

                if (OnStart != null)
                {
                    OnStart(this, new ServerStartEventArgs("Server Ready"));
                }     
            }
            catch(Exception ex)
            {
                this.Server_Error(this, new CFErrorEventArgs(ex.Message));
            }
        }
        
        public abstract void Add(Socket socket);

        public async void Run()
        {
            try
            {     
                while (!this.m_stop)
                {               
                    var socket = await this.m_listener.AcceptSocketAsync();
                   
                    this.Add(socket);      
                }
            }
            catch
            {                
            }

            this.m_stop = true;

            if (OnStop != null)
            {
                OnStop(this, new ServerStopEventArgs("Server Stop"));
            }    
        }

        public void Stop()
        {
            this.m_stop = true;

            this.close();            
        }

        private void close()
        {
            if (this.m_listener != null)
            {                
                this.m_listener.Stop();

                this.m_listener.Server.Dispose();
                this.m_listener = null;            
            }           
        } 
    
        public void Clear()
        { 
            foreach (var session in this.Sessions.Values)
            {
                if (session.IsAlive)
                    session.Abort();
            }                  
        }

        public void Abandon(string sessionId)
        {
            ICFSession session = null;

            if (this.Sessions.TryGetValue(sessionId, out session))
            {
                session.Abort();
            } 
        }

        protected void Terminate(ICFSession session)
        {
            if (session != null)
            {
                session.Abort();

                session.Dispose();
                session = null;
            }
        }

        #region Server Event  
        protected void Client_Connect(ClientConnectEventArgs e)
        {
            if (OnConnect != null)
            {
                OnConnect(this, e);
            }
        }

        protected void Client_Disconnect(ClientDisconnectEventArgs e)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(this, e);
            }
        }

        protected void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OnDataReceived != null)
            {
                OnDataReceived(sender, e);
            }
        }
 
        protected void Client_Error(object sender, CFErrorEventArgs e)
        {
            if (OnClientError != null)
            { 
                OnClientError(sender, e);
            }
        }

        protected void Server_Error(object sender, CFErrorEventArgs e)
        {
            if (OnServerError != null)
            {
                OnServerError(sender, e);
            }
        }
        #endregion 
    }
}
