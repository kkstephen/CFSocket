using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSession : CFSocket, ICFSession, IDisposable
    {
        protected bool m_stop;
        
        public bool IsAlive
        {
            get
            {
                return !this.m_stop;
            }
        }
  
        public CFSession(TcpClient client)
        {
            this.Connection = client;

            this.m_closed = false;

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
