using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CFS.WinRT
{
    public class CFSocket : ICFSocket
    {
        public string Host { get; set; }
        public int Port { get; set; }

        protected CFStream Stream;
   
        public virtual async Task<string> ReceiveAsync()
        {
            return await this.Stream.ReadAsync();  
        }

        public virtual async Task SendAsync(string data)
        {  
            await this.Stream.WriteAsync(data);
        }
    }
}
