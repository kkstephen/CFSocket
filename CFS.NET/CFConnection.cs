using System;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFConnection : ICFSocket, ICFConnection
    { 
        public event EventHandler<SessionOpenEventArgs> OnOpen;
        public event EventHandler<SessionCloseEventArgs> OnClose;        
        public event EventHandler<SessionDataEventArgs> OnReceived;
        public event EventHandler<SessionDataEventArgs> OnSend;
        public event EventHandler<CFErrorEventArgs> OnError;

        public ICFMessageEncoder Encoder { get; set; }
        public Socket Socket { get; set; }
        protected ICFStream Stream { get; set; }
   
        public string ID { get; set; }

        public string Host { get; set; }
        public int Port { get; set; } 

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

                this.Socket.ReceiveTimeout = this.timeout * 1000;
                this.Socket.SendTimeout = this.timeout * 1000;
            }
        }

        protected string recv_data;      

        private bool m_closed;
        public bool IsClosed
        {
            get
            {
                return this.m_closed;
            }
        }        
               
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
                    }

                    if (this.Socket != null)
                    {
                        this.Socket.Dispose();                        
                    }
                }
                
                // free native resources
                this.Stream = null;
                this.Socket = null;

                this.disposed = true;
            }            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CFConnection()
        {
            this.ID = "";
            this.m_closed = true;

            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public virtual void Connect()
        {
            //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.Host), this.Port);

            var iasr = this.Socket.BeginConnect(this.Host, this.Port, null, null);

            iasr.AsyncWaitHandle.WaitOne(this.Timeout * 1000);

            if (iasr.IsCompleted)
            {
                this.Socket.EndConnect(iasr);

                this.Open();
            }
            else
            {
                throw new Exception("Connect remote host fail.");
            }
        }

        public virtual void Open()
        {
            this.Stream = new CFStream(this.Socket);

            this.m_closed = false;

            if (OnOpen != null)
            {
                OnOpen(this, new SessionOpenEventArgs(this.Host, this.Port));
            }
        }

        public virtual void Abort()
        {
            if (!this.m_closed)
            {
                if (this.Stream != null)
                    this.Stream.Close();
                
                if (this.Socket != null)
                    this.Socket.Close();

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
 
        public virtual void Send(string data)
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");

            this.Stream.Write(data);

            if (!string.IsNullOrEmpty(data) && OnSend != null)
            {
                OnSend(this, new SessionDataEventArgs(data.Length));
            }           
        }

        public virtual async Task SendAsync(string data)
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");

            await this.Stream.WriteAsync(data);

            if (!string.IsNullOrEmpty(data) && OnSend != null)
            {
                OnSend(this, new SessionDataEventArgs(data.Length));
            }
        }

        public virtual string Receive()
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");

            //message boundary: crlf
            string data = this.Stream.ReadLine();
             
            if (!string.IsNullOrEmpty(data) && OnReceived != null)
            {
                OnReceived(this, new SessionDataEventArgs(data.Length));
            } 

            return data;
        }

        public virtual async Task<string> ReceiveAsync()
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");

            //message boundary: crlf
            string data = await this.Stream.ReadLineAsync();

            if (!string.IsNullOrEmpty(data) && OnReceived != null)
            {
                OnReceived(this, new SessionDataEventArgs(data.Length));
            }

            return data;
        }

        public virtual void SendMessage(ICFMessage message)
        {
            string body = this.Encoder.Encode(message);

            this.Send(body + message.Boundary);
        }

        public virtual async Task SendMessageAsync(ICFMessage message)
        {
            string body = this.Encoder.Encode(message);

            await this.SendAsync(body + message.Boundary);
        } 

        public virtual ICFMessage ReceiveMessage()
        {
            this.recv_data = this.Receive();

            return this.Encoder.Decode(this.recv_data);
        }

        public virtual async Task<ICFMessage> ReceiveMessageAsync()
        {
            this.recv_data = await this.ReceiveAsync();

            return this.Encoder.Decode(this.recv_data);
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
