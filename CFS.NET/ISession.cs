using System.Net;

namespace CFS.Net
{
    public interface ISession
    {
        IPEndPoint RemoteHost { get; }
        void Begin();
        void Start();
        void Stop();
        void Close();
    }
}
