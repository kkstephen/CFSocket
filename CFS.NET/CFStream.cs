using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFStream : ICFStream, IDisposable
    {
        private static readonly string CRLF = "\r\n";

        private NetworkStream _netStream;
        private StreamReader _sr;
        private StreamWriter _sw;
         
        private bool disposed = false;
 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (this._netStream != null)
                    {
                        this._netStream.Dispose();                       
                    }

                    if (this._sr != null)
                    {
                        this._sr.Dispose();                        
                    }

                    if (this._sw != null)
                    {
                        this._sw.Dispose();                        
                    }
                }

                // free native resources

                this._netStream = null;
                this._sr = null;
                this._sw = null;
                
                this.disposed = true;
            }            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public CFStream(Socket socket)
        {
            this._netStream = new NetworkStream(socket);                

            this._sr = new StreamReader(this._netStream);
            this._sw = new StreamWriter(this._netStream);
        }

        public void Close()
        {
            this._netStream.Close();

            this._sr.Close();
            this._sw.Close();
        }
 
        public string ReadLine()
        {
            if (this._netStream.CanRead)
            {
                return this._sr.ReadLine(); 
            }

            throw new IOException("Network stream can not to read data");
        }

        public async Task<string> ReadLineAsync()
        {
            if (this._netStream.CanRead)
            {
                return await this._sr.ReadLineAsync();
            }

            throw new IOException("Network stream can not to read data");
        }

        public void Write(string data)
        {
            if (this._netStream.CanWrite)
            {
                this._sw.Write(data);

                this._sw.Flush();
            }
            else
            {
                throw new IOException("Network stream can not to send data");
            }
        }

        public async Task WriteAsync(string data)
        {
            if (this._netStream.CanWrite)
            {
                await this._sw.WriteAsync(data);

                this._sw.Flush();
            }
            else
            {
                throw new IOException("Network stream can not to send data");
            }
        }

        public void WriteLine(string data)
        {
            this.Write(data + CRLF);
        }

        public async Task WriteLineAsync(string data)
        {
            await this.WriteAsync(data + CRLF);
        }
    }
}
