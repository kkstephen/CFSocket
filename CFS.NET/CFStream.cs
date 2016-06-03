using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
 
namespace CFS.Net
{
    public class CFStream : IDisposable
    {          
        private NetworkStream _netStream;
        private StreamReader _reader;
        private StreamWriter _writer;

        private bool _isAvabile = false;
        private string _data;

        public bool IsAvailable
        {
            get
            {
                return this._isAvabile;
            }          
        }

        private bool disposed = false;

        public CFStream(NetworkStream stream)
        {
            this._netStream = stream;

            this._reader = new StreamReader(this._netStream);
            this._writer = new StreamWriter(this._netStream);

            this._isAvabile = true;
        }         

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
                        this._netStream = null;
                    }

                    if (this._reader != null)
                    {                        
                        this._reader.Dispose();
                        this._reader = null;                     
                    }

                    if (this._writer != null)
                    {                        
                        this._writer.Dispose();
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
            this._netStream.Close();
            this._reader.Close();
            this._writer.Close();

            this._isAvabile = false;
        }        

        public string Read()
        {
            if (this._netStream.CanRead)
            {
                this._data = this._reader.ReadLine();

                if (!string.IsNullOrEmpty(this._data))
                {
                    return this._data;                     
                }
            }

            throw new IOException("Network receive data error.");
        }

        public void Write(string data)
        {
            if (this._netStream.CanWrite)
            { 
                this._writer.WriteLine(data);
                this._writer.Flush();
            }
            else
            {
                throw new IOException("Network send data error");
            }
        }
    }
}
