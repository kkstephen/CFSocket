using System.Net;

namespace CFS.Net
{
    public interface ICFSession
    {
        IPEndPoint RemoteHost { get; }
        IPEndPoint PushHost { get; }
        bool IsAlive { get; }
        void Begin();
        void Start();
        void Stop();
        void Close();
    }
}
