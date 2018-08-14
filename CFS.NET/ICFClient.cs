using System;
 
namespace CFS.Net
{
    interface ICFClient
    {
        void Connect();
        void Logout();
        void KeepAlive(); 
    }
}
