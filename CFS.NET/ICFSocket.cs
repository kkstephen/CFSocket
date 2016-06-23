using System;

namespace CFS.Net
{
    public interface ICFSocket
    {
        bool Encryption { get; }
        ICFCrypto Cipher { get; }

        void Send(string data);
        string Receive();
    }
}
