using System;
using System.Net;

namespace CFS.Net
{
    public interface ICFSession : ICFSocket, IDisposable
    {
        string Host { get; }
        int Port { get; }
        bool IsAlive { get; } 
       
        void Start(); 
        void End();
        void Begin();
        void Abort(); 
    } 
}
