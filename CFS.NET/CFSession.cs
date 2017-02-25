using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSession : CFSocket, ICFSession
    {    
        protected TcpClient Connection;
         
        public bool IsAlive
        {
            get
            {
                return !this.m_Stop;
            }
        } 

        private bool m_Stop;
        private bool m_closed;

        private bool disposed = false;

        public IPEndPoint ClientEndPoint { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (this.Stream != null)
                    { 
                        this.Stream.Dispose();

                        this.Stream = null;
                    }

                    if (this.Connection != null)
                    { 
                        this.Connection.Dispose();

                        this.Connection = null;
                    } 
                }

                this.disposed = true;
            }            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CFSession(TcpClient conn)
        {
            this.Connection = conn;
            this.Stream = new CFStream(conn.GetStream());
         
            this.m_Stop = true;
            this.m_closed = true;
          
            this.ClientEndPoint = this.Connection.Client.RemoteEndPoint as IPEndPoint;
        }
         
        public abstract void Begin();

        public virtual void Start()
        {
            this.m_Stop = false;
            this.m_closed = false;
        }
  
        public virtual void End()
        {
            this.m_Stop = true;
        }
         
        public virtual void Close()
        {            
            if (!this.m_closed)
            {
                this.m_closed = true;

                this.End();
                
                this.Stream.Close();
                this.Connection.Close();
            }       
        }  
    }
}
