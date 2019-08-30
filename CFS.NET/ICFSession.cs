using System;
using System.Threading.Tasks;

namespace CFS.Net
{
    public interface ICFSession : IDisposable
    {
        bool IsAlive { get; }

        string Host { get; }
        int Port { get; }
               
        void Start(); 
        void End();
        void Abort();
    } 
}
