using System;

namespace CFS.Net
{
    public interface ICFStream : IDisposable
    {
        string ReadAll();
        string ReadLine();
        void Write(string str);
        void WriteLine(string str);

        void Close();
    }
}
