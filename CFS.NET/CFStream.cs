using System;
using System.IO;
using System.Net.Sockets;

namespace CFS.Net
{
    public class CFStream : IDisposable
    {          
        private NetworkStream _netStream;
        private StreamReader _reader;
        private StreamWriter _writer;
        
        private bool disposed = false;

        public CFStream(NetworkStream stream)
        {
            this._netStream = stream;

            this._reader = new StreamReader(this._netStream);
            this._writer = new StreamWriter(this._netStream);            
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
        }        

        public string Read()
        { 
            if (this._netStream.CanRead)
            {
                return this._reader.ReadLine();
            }
            else
            {
                throw new IOException("Network receive data error.");
            }           
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
