using System;

namespace CFS.Net
{
    public interface ICFConnection
    { 
        void Open();
        void Close();

        void Send(string str);
        string  Receive();
    }
}
