using System;

namespace CFS.Net
{
    public class ClientLoginEventArgs : EventArgs
    {
        public string Message { get; private set; }        

        public ClientLoginEventArgs(string str)
        {
            this.Message = str;            
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

    public class SessionCloseEventArgs : EventArgs
    {
        public string ID { get; private set; }
     
        public SessionCloseEventArgs(string id)
        {            
            this.ID = id;
        }
    }

    public class ClientCloseEventArgs : EventArgs
    {
        public string Message { get; private set; }
    
        public ClientCloseEventArgs(string str)
        {
            this.Message = str;             
        }
    }
    
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }        

        public ErrorEventArgs(string message)
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

    public class ConnectEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public ISession Session { get; private set; }

        public ConnectEventArgs(string message, ISession se)
        {
            this.Message = message;
            this.Session = se;
        }
    } 
}
