using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSocket : ICFSocket, IDisposable
    { 
        public event EventHandler<SessionOpenEventArgs> OnOpen;
        public event EventHandler<SessionCloseEventArgs> OnClose;
        public event EventHandler<CFErrorEventArgs> OnError;
        public event EventHandler<DataReceivedEventArgs> OnReceived;

        protected TcpClient Connection { get; set; }
        protected CFStream Stream { get; private set; }
   
        public string ID { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        
        private bool m_closed;
               
        private bool disposed = false;

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
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CFSocket()
        {
            this.ID = "";

            this.m_closed = true;
        }

        public virtual void Open()
        {
            this.Stream = new CFStream(this.Connection.GetStream());

            this.m_closed = false;

            if (OnOpen != null)
            {
                OnOpen(this, new SessionOpenEventArgs());
            }
        }

        public virtual void Abort()
        {
            if (!this.m_closed)
            {
                if (this.Stream != null)
                    this.Stream.Close();
                
                this.Connection.Close();

                this.m_closed = true;
            }
        }
        
        public virtual void Close()
        {
            this.Abort();

            if (OnClose != null)
            {
                OnClose(this, new SessionCloseEventArgs(this.ID));
            }
        }

        public virtual string Receive()
        {
            if (this.m_closed)
                throw new Exception("Connection closed."); 

            string data = this.Stream.ReadLine();

            if (OnReceived != null)
            {
                OnReceived(this, new DataReceivedEventArgs(data));
            }

            return data;                        
        }

        public void Send(ICFMessage message)
        {
            this.Send(message.ToString());
        }
 
        public virtual void Send(string data)
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");
            
            this.Stream.WriteLine(data);                        
        }

        #region Event              
        protected void onSocketError(CFErrorEventArgs e)
        {
            if (OnError != null)
            {
                OnError(this, e);
            }
        }
        #endregion
    }
}
