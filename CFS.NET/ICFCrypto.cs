using System;

namespace CFS.Net
{
    public interface ICFCrypto
    {
        string Encrypt(string str);
        string Decrypt(string str);
    }
}
