using System;
using System.Threading.Tasks;

namespace CFS.WinRT
{
    public interface ICFSocket
    { 
        Task SendAsync(string data);
        Task<string> ReceiveAsync();
    }
}
