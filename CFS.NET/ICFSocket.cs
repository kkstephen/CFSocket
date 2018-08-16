using System;

namespace CFS.Net
{
    public interface ICFSocket : IDisposable
    {
        void Send(string data);
        string Receive();
    }
}
