using System;
using System.Threading.Tasks;

namespace CFS.Net
{
    public interface ICFStream : IDisposable
    {
        string ReadLine();
        Task<string> ReadLineAsync();

        void Write(string str);
        Task WriteAsync(string str);
        
        void WriteLine(string str);
        Task WriteLineAsync(string str);

        void Close();
    }
}
