using System;

namespace CFS.Net
{
    public interface ICFSocket
    { 
        void Send(string data);
        string Receive();
 
        void Close();
    }
}
