using System;

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

        public ClientConnectEventArgs(string ip, int port) : base("Client Connect")
        { 
            this.IP = ip;
            this.Port = port;
        }
    } 

    public class DisconnectEventArgs : CFEventArgs
    {
        public string IP { get; private set; }
        public int Port { get; private set; }

        public DisconnectEventArgs(string ip, int port) : base("Client Disconnect")
        {
            this.IP = ip;
            this.Port = port;
        } 
    }

    public class ClientLoginEventArgs : CFEventArgs
    { 
        public string UserId { get; private set; }

        public ClientLoginEventArgs(string userid) : base("Client Login")
        {
            this.UserId = userid;            
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
