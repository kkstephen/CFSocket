using System;

namespace CFS.Net
{
    public interface ICipher
    {
        string Encrypt(string str);
        string Decrypt(string str);
    }
}
