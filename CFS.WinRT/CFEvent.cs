using System;

namespace CFS.WinRT
{ 
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
