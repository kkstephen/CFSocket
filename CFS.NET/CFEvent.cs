using System;

namespace CFS.Net
{
    public class ClientConnectEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public string IP { get; private set; }
        public string Id { get; private set; }

        public ClientConnectEventArgs(string message, string ip, string id)
        {
            this.Message = message;
            this.IP = ip;
            this.Id = id;
        }
    } 

    public class DisconnectEventArgs : EventArgs
    {
        public string SessonId { get; private set; }
       
        public DisconnectEventArgs(string ssid)
        {
            this.SessonId = ssid;
        }
    }

    public class ClientLoginEventArgs : EventArgs
    {
        public string Message { get; private set; }        

        public ClientLoginEventArgs(string str)
        {
            this.Message = str;            
        }
    }

    public class SessionCloseEventArgs : EventArgs
    {
        public string ID { get; private set; }
     
        public SessionCloseEventArgs(string id)
        {            
            this.ID = id;
        }
    } 
    
    public class CFErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }        

        public CFErrorEventArgs(string message)
        {
            this.Message = message;
        }
    }
    
    public class StartEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public StartEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public class StopEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public StopEventArgs(string message)
        {
            this.Message = message;
        }
    } 
}
