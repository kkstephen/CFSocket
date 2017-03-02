using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFSocket : ICFSocket, IDisposable
    { 
        public event EventHandler<SessionOpenEventArgs> OnOpen;
        public event EventHandler<SessionCloseEventArgs> OnClose;
        public event EventHandler<CFErrorEventArgs> OnError;
        public event EventHandler<DataReceivedEventArgs> OnReceived;

        protected TcpClient Connection;

        private CFStream stream;
   
        public string ID { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }

        public bool IsConnected
        {
            get
            {
                return !this.m_closed;
            }
        }

        protected bool m_closed;

        private bool disposed = false;
 
        public int Timeout { get; set; }
      
        public ICFCrypto Cipher { get; set; }
        public bool Encryption { get; set; }
         
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (this.stream != null)
                    {
                        this.stream.Dispose();
                        this.stream = null;
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
            this.Timeout = 20;
        }

        public void Connect(int second)
        {
            var task = this.Connection.ConnectAsync(this.Host, this.Port);

            if (task.Wait(second * 1000, new CancellationToken(false)))
            { 
                this.Open();        
            }
        }

        protected void Open()
        {
            this.m_closed = false;

            this.Connection.Client.ReceiveTimeout = this.Timeout * 1000;
            this.Connection.Client.SendTimeout = this.Timeout * 1000;

            this.stream = new CFStream(this.Connection.GetStream());

            if (OnOpen != null)
            {
                OnOpen(this, new SessionOpenEventArgs());
            } 
        }

        public void Abort()
        {
            if (!this.m_closed)
            { 
                if (this.stream != null)
                    this.stream.Close();

                this.Connection.Close();

                this.m_closed = true;              
            }
        }

        public void Close()
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

            string data = this.stream.ReadLine();

            if (OnReceived != null)
            {
                OnReceived(this, new DataReceivedEventArgs(data));
            }

            if (Encryption)
            {
                data = this.Cipher.Decrypt(data);
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

            if (Encryption)
            {
                data = this.Cipher.Encrypt(data);
            }

            this.stream.WriteLine(data);                        
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
