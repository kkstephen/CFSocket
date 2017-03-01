using System;
using System.Net;

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
        public string IP { get; private set; }
        public int Port { get; private set; }

        public ClientConnectEventArgs(string host, int port) : base("Client Connect")
        { 
            this.IP = host;
            this.Port = port;
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

    public class SessionOpenEventArgs : CFEventArgs
    {
        public SessionOpenEventArgs() : base("Session Open")
        {
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

    public class DataReceivedEventArgs : CFEventArgs
    { 
        public DataReceivedEventArgs(string data) : base(data)
        {             
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
