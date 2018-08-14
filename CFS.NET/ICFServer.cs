using System;
using System.Net.Sockets;

namespace CFS.Net
{
    public interface ICFServer : IDisposable
    {
        void Start();
        void Add(Socket socket);
        void Run();
        void Stop();
        void Clear();
    }
}
