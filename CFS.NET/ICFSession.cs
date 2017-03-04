using System;
using System.Net;

namespace CFS.Net
{
    public interface ICFSession : IDisposable
    {
        bool IsAlive { get; }

        string Host { get; }
        int Port { get; }
               
        void Start(); 
        void Begin(); 
        void End();
        void Abort();
    } 
}
