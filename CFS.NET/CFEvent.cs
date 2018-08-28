using System;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFEventArgs : EventArgs
    {
        public string Message { get; private set; }         

        public CFEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public class ClientConnectEventArgs : CFEventArgs
    {
        public Socket EndPoint { get; private set; }
 
        public ClientConnectEventArgs(Socket socket) : base("Client Connect")
        {
            this.EndPoint = socket;     
        }
    } 

    public class ClientDisconnectEventArgs : CFEventArgs
    {
        public string IP { get; private set; }
        public int Port { get; private set; }

        public ClientDisconnectEventArgs(string host, int port) : base("Client Disconnect")
        {
            this.IP = host;
            this.Port = port;
        } 
    }

    public class SessionEventArgs : CFEventArgs
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public SessionEventArgs(string host, int port) : base("Session")
        {
            this.IP = host;
            this.Port = port;
        }
    }

    public class SessionOpenEventArgs : SessionEventArgs
    { 
        public SessionOpenEventArgs(string host, int port) : base(host, port)
        {
            this.IP = host;
            this.Port = port;
        }
    } 

    public class SessionCloseEventArgs : CFEventArgs
    {
        public string ID { get; private set; }
     
        public SessionCloseEventArgs(string id) : base("Session Close")
        {            
            this.ID = id;
        }
    }

    public class SessionDataEventArgs : CFEventArgs
    { 
        public int Count { get; set; }

        public SessionDataEventArgs(int count) : base("Data available")
        {
            this.Count = count;
        }
    }
    
    public class CFErrorEventArgs : CFEventArgs
    {   
        public CFErrorEventArgs(string message) : base(message)
        {
        }
    }
    
    public class ServerStartEventArgs : CFEventArgs
    {
        public ServerStartEventArgs(string message) : base(message)
        {
        }
    }

    public class ServerStopEventArgs : CFEventArgs
    {
        public ServerStopEventArgs(string message) : base(message)
        {
        }
    } 
}
