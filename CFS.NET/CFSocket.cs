using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFSocket : ICFSocket, IDisposable
    {
        public event EventHandler<ClientConnectEventArgs> OnConnect;
        public event EventHandler<ClientDisconnectEventArgs> OnDisconnect;
        public event EventHandler<SessionOpenEventArgs> OnOpen;
        public event EventHandler<SessionCloseEventArgs> OnClose;

        public event EventHandler<CFErrorEventArgs> OnError;
        public event EventHandler<DataReceivedEventArgs> OnDataReceived;

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

        private int timeout;
        public int Timeout
        {
            get
            {
                return this.timeout;
            }

            set
            {
                this.timeout = value;

                this.Connection.ReceiveTimeout = this.timeout * 1000;
                this.Connection.SendTimeout = this.timeout * 1000;
            }
        } 

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

            this.stream = new CFStream(this.Connection.GetStream());

            this.onSessionOpen(new SessionOpenEventArgs());
        }

        public void Close()
        {
            if (!this.m_closed)
            {
                this.m_closed = true;

                this.stream.Close();
                this.Connection.Close();

                this.onSessionClose(new SessionCloseEventArgs(this.ID));                
            }
        }

        public virtual string Receive()
        { 
            if (this.m_closed)
            {
                return "";
            }

            string data = this.stream.ReadLine();
 
            if (Encryption)
            {
                data = this.Cipher.Decrypt(data);
            } 

            if (!string.IsNullOrEmpty(data))
            {
                this.onDataReceived(new DataReceivedEventArgs(data));                 
            }
                
            return data;                        
        }

        public virtual void Send(string data)
        {
            if (this.m_closed)
            {
                return;
            }

            if (Encryption)
            {
                data = this.Cipher.Encrypt(data);
            }

            this.stream.WriteLine(data);                        
        }

        #region
        protected void onConnect(object sender, ClientConnectEventArgs e)
        {
            if (OnConnect != null)
            {
                OnConnect(sender, e);
            }
        }
         
        protected void onDisconnect(ClientDisconnectEventArgs e)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(this, e);
            }
        }

        protected void onSessionOpen(SessionOpenEventArgs e)
        {
            if (OnOpen != null)
            {
                OnOpen(this, e);
            }
        }

        private void onSessionClose(SessionCloseEventArgs e)
        {
            if (OnClose != null)
            {
                OnClose(this, e);
            }
        }

        protected void onDataReceived(DataReceivedEventArgs e)
        {
            if (OnDataReceived != null)
            {
                OnDataReceived(this, e);
            }
        }

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
