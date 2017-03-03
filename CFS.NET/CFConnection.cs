﻿using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CFS.Net
{
    public abstract class CFConnection : ICFSocket, ICFConnection, IDisposable
    { 
        public event EventHandler<SessionOpenEventArgs> OnOpen;
        public event EventHandler<SessionCloseEventArgs> OnClose;
        public event EventHandler<CFErrorEventArgs> OnError;
        public event EventHandler<DataReceivedEventArgs> OnReceived;
         
        public Socket Socket { get; set; }
        protected ICFStream Stream { get; set; }
   
        public string ID { get; set; }

        public string Host { get; set; }
        public int Port { get; set; } 

        private int timeout;
        public int Timeout
        {
            get
            {
                return this.timeout;
            }

            set
            {
                this.timeout = value;

                this.Socket.ReceiveTimeout = this.timeout * 1000;
                this.Socket.SendTimeout = this.timeout * 1000;
            }
        }

        protected string _data;
        protected bool m_closed;
               
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (this.Stream != null)
                    {
                        this.Stream.Dispose();
                        this.Stream = null;
                    }

                    if (this.Socket != null)
                    {
                        this.Socket.Dispose();
                        this.Socket = null;
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

        public CFConnection()
        {
            this.ID = "";

            this.m_closed = true;
        }

        public virtual void Open()
        {
            this.Stream = new CFStream(this.Socket);

            this.m_closed = false;

            if (OnOpen != null)
            {
                OnOpen(this, new SessionOpenEventArgs());
            }
        }

        public virtual void Abort()
        {
            if (!this.m_closed)
            {
                if (this.Stream != null)
                    this.Stream.Close();
                
                if (this.Socket != null)
                    this.Socket.Close();

                this.m_closed = true;
            }
        }
        
        public virtual void Close()
        {
            this.Abort();

            if (OnClose != null)
            {
                OnClose(this, new SessionCloseEventArgs(this.ID));
            }
        }

        public virtual string Receive()
        {
            if (this.m_closed)
                throw new Exception("Connection closed."); 

            string data = this.Stream.ReadLine();

            if (OnReceived != null)
            {
                OnReceived(this, new DataReceivedEventArgs(data));
            }

            return data;                        
        }

        public virtual void Send(ICFMessage message)
        {
            this.Send(message.ToString());
        }
 
        public virtual void Send(string data)
        {
            if (this.m_closed)
                throw new Exception("Connection closed.");
            
            this.Stream.WriteLine(data);                        
        }

        public abstract void ReadMessage();       

        #region Event         
        protected void onSocketError(CFErrorEventArgs e)
        {
            if (OnError != null)
            {
                OnError(this, e);
            }
        }
        #endregion
    }
}