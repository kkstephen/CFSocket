using System;

namespace CFS.WinRT
{
    public interface ICFCrypto
    {
        string Encrypt(string str);
        string Decrypt(string str);
    }
}
