using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSession : CFSocket, ICFSession 
    {
        private bool m_stop;

        public bool IsAlive
        {
            get
            {
                return !this.m_stop;
            }
        }

        private int timeout; 
        public int Timeout
        {
            set
            {
                this.timeout = value;

                this.Connection.ReceiveTimeout = this.timeout * 1000;
                this.Connection.SendBufferSize = this.timeout * 1000;
            }
        }

        public CFSession(TcpClient client) 
        {
            this.Connection = client; 

            this.m_stop = true;   
        }
        
        public virtual void Start()
        {  
            this.m_stop = false;            

            this.Open();
        }

        public abstract void Begin();

        public virtual void End()
        {
            this.m_stop = true;
        } 
    }
}
