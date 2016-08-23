using System;
using System.Threading.Tasks;

namespace CFS.WinRT
{
    public interface ICFSocket
    {
        bool Encryption { get; }
        ICFCrypto Cipher { get; }

        Task SendAsync(string data);
        Task<string> ReceiveAsync();
    }
}
