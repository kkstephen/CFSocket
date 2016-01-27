using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSession : ICFSession, ICFSocket
    {          
        public delegate void OnError(object sender, ErrorEventArgs e);
        public event OnError OnServerError;

        public delegate void OnSessionError(object sender, ErrorEventArgs e);
        public event OnSessionError OnClientError;

        public delegate void OnClose(object sender, SessionCloseEventArgs e);
        public event OnClose OnSessionClose;

        private TcpClient _conn; 

        public CFStream Stream
        {
            get; private set;             
        }

        public bool Encryption { get; set; }

        public ICFCrypto Cipher { get; set; }

        public IPEndPoint PushHost { get; set; } 
         
        public string ID { get; set; }

        public bool IsAlive
        {
            get
            {
                return !this.m_Stop;
            }
        }

        private bool m_Stop;
        
        public IPEndPoint RemoteHost { get; private set; }
         
        public CFSession(TcpClient conn)
        {
            this._conn = conn;
            this.Stream = new CFStream(conn.GetStream());
         
            this.m_Stop = true;
          
            this.RemoteHost = (IPEndPoint)this._conn.Client.RemoteEndPoint;
        }

        public abstract void Begin();

        public virtual void Start()
        {
            this.m_Stop = false;
        }
  
        public virtual void Stop()
        {
            this.m_Stop = true;
        }
         
        public virtual void Close()
        {
            if (!this.m_Stop)
            {
                this.m_Stop = true;

                this._conn.Close();
                this._conn = null;

                this.Stream.Close();
                this.Stream = null;
            } 
        }

        public virtual string Receive()
        {
            string recv = this.Stream.Read();

            if (Encryption)
            {
                return this.Cipher.Decrypt(recv);
            }

            return recv;
        }

        public virtual void Send(string data)
        {
            if (Encryption)
            {
                data = this.Cipher.Encrypt(data);
            }

            this.Stream.Write(data);
        } 

        #region Event 
        protected void onServerError(object sender, ErrorEventArgs e)
        {
            if (OnServerError != null)
            {
                OnServerError(sender, e);
            }
        }

        protected void onSessionError(object sender, ErrorEventArgs e)
        {
            if (OnClientError != null)
            {
                OnClientError(sender, e);
            }
        } 

        protected void onClientClose(object sender, SessionCloseEventArgs e)
        {
            if (OnSessionClose != null)
            {
                OnSessionClose(sender, e);
            }
        }
        #endregion
    }
}
