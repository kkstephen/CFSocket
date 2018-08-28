using System;
  
namespace CFS.Net
{
    public abstract class CFClient : CFConnection, ICFClient
    {  
        public abstract void Logout();
        public abstract void KeepAlive(); 
    }
}
