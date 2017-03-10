using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFSession : CFConnection, ICFSession
    {
        private bool m_stop;
        public bool IsAlive
        {
            get
            {
                return !this.m_stop;
            }
        }
 
        public CFSession(Socket socket)
        {
            this.Socket = socket;

            var remoteHost = socket.RemoteEndPoint as IPEndPoint;

            this.Host = remoteHost.Address.ToString();
            this.Port = remoteHost.Port;

            this.m_stop = true;
        }

        public virtual void Start()
        {
            this.m_stop = false;

            this.Open();
        }

        public void Begin()
        {
            if (this.Authorize())
            {
                this.HandleProtocol();
            }
            
            this.Close();
        }

        public abstract bool Authorize();

        public abstract void HandleProtocol();

        public virtual void End()
        {
            this.m_stop = true;
        }

        public abstract void GetClientMessage();       
    }
}
