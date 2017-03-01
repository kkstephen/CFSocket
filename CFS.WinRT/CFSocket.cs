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
 
        public ICFCrypto Cipher
        {
            get; set;            
        }

        public bool Encryption
        {
            get; set;
        }
 
        public virtual async Task<string> ReceiveAsync()
        {
            string recv = await this.Stream.ReadAsync();

            if (Encryption)
            {
                return this.Cipher.Decrypt(recv);
            }

            return recv;
        }

        public virtual async Task SendAsync(string data)
        {
            if (Encryption)
            {
                data = this.Cipher.Encrypt(data);
            }

            await this.Stream.WriteAsync(data);
        }
    }
}
