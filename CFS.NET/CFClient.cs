using System;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFClient : CFConnection 
    {   
        public CFClient()
        {
            
            this.Encoder = new CFClientMessageEncoder();

            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
 
        public abstract void Connect(); 
    }
}
