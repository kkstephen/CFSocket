using System;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFClient : CFConnection 
    {  
        public CFSServerMessage Message { get; set; }

        public CFClient()
        {
            this.Message = new CFSServerMessage();

            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public abstract void Connect(); 
    }
}
