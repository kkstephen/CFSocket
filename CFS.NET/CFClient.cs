using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFClient : CFConnection, ICFClient
    { 
        public CFClient()
        {  
            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public virtual void Connect()
        { 
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.Host), this.Port);

            var iasr = this.Socket.BeginConnect(remoteEP, null, null);

            iasr.AsyncWaitHandle.WaitOne(this.Timeout);

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

        public abstract void Logout();
        public abstract void KeepAlive(); 
    }
}
