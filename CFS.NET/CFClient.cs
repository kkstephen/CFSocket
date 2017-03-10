using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFClient : CFConnection 
    { 
        public CFClient()
        {  
            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
 
        public virtual void Connect()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.Host), this.Port);

            var ir = this.Socket.BeginConnect(remoteEP, null, null);

            ir.AsyncWaitHandle.WaitOne(3000);

            if (ir.IsCompleted)
            {
                this.Socket.EndConnect(ir);
 
                this.Open();
            }
            else
            {
                throw new Exception("Connect remote host fail.");
            }
        }

        public abstract void GetServerMessage();       
    }
}
