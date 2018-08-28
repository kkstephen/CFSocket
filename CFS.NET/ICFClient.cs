using System;
 
namespace CFS.Net
{
    interface ICFClient
    { 
        void Logout();
        void KeepAlive(); 
    }
}
