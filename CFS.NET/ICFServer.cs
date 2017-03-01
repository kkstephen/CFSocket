using System;

namespace CFS.Net
{
    public interface ICFServer
    {
        void Start();
        void Run();
        void Stop();
        void Clear();
    }
}
