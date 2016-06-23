using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFSocket : ICFSocket
    {
        protected CFStream Stream;
 
        public ICFCrypto Cipher
        {
            get; set;            
        }

        public bool Encryption
        {
            get; set;
        }

        public virtual string Receive()
        {
            string recv = this.Stream.Read();

            if (Encryption)
            {
                return this.Cipher.Decrypt(recv);
            }

            return recv;
        }

        public virtual void Send(string data)
        {
            if (Encryption)
            {
                data = this.Cipher.Encrypt(data);
            }

            this.Stream.Write(data);
        }
    }
}
