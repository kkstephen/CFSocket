using System;

namespace CFS.Net
{
    public class ClientConnectEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public ICFSession Session { get; private set; }

        public ClientConnectEventArgs(string message, ICFSession se)
        {
            this.Message = message;
            this.Session = se;
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
}
