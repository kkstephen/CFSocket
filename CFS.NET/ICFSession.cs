using System;
using System.Net;

namespace CFS.Net
{
    public interface ICFSession : IDisposable
    {
        string Host { get; }
        int Port { get; }

        bool IsAlive { get; } 
        void Begin(); 
        void Start(); 
        void End(); 
        void Close(); 
    } 
}
