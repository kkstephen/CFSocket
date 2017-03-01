using System;
using System.IO;
using Windows.Networking.Sockets;
using System.Threading.Tasks;

namespace CFS.WinRT
{
    public class CFStream : IDisposable
    {
        private StreamSocket socket;

        private StreamReader _reader;
        private StreamWriter _writer;
 
        private bool _isAvabile = false;
        private string _data;
 
        private bool disposed = false;

        public CFStream(StreamSocket socket)
        {
            this.socket = socket;

            this._reader = new StreamReader(this.socket.InputStream.AsStreamForRead());
            this._writer = new StreamWriter(this.socket.OutputStream.AsStreamForWrite());

            this._isAvabile = true;
        }         

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.socket != null)
                    {
                        this.socket.Dispose();
                        this.socket = null;
                    } 
 
                    if (this._reader != null)
                    { 
                        this._reader = null;                     
                    }

                    if (this._writer != null)
                    { 
                        this._writer = null;                     
                    } 
                }

                this.disposed = true;
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }
        
        public void Close()
        {
            this.socket.InputStream.Dispose();
            this.socket.OutputStream.Dispose();

            this._isAvabile = false; 
        }        

        public async Task<string> ReadAsync()
        {           
            if (this._isAvabile)
            { 
                this._data = await this._reader.ReadLineAsync();

                if (!string.IsNullOrEmpty(this._data))
                {
                    return this._data;
                }
                else
                {
                    this._isAvabile = false;
                }                
            }

            throw new IOException("Network receive data error.");
        }

        public async Task WriteAsync(string data)
        {
            if (this._isAvabile)
            {                
                await this._writer.WriteLineAsync(data);
                await this._writer.FlushAsync();                
            }
            else
            {
                throw new IOException("Network send data error");
            }         
        }
    }
}
