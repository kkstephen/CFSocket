using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CFS.SMTP
{
    public class MailSendSuccessEventArgs : EventArgs
    {
        public int Index { get; private set; }        
        public string MailID { get; private set;}     
        public string Text { get; private set; }        

        public MailSendSuccessEventArgs(int index, string mailId, string msg)
        {
            this.Index = index;            
            this.MailID = mailId;      
            this.Text = msg;
        }
    }

    public class MailSendFailEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public string MailID { get; private set; }
        public string Address { get; private set; }
        public string Text { get; private set; }
        public string Host { get; private set; }

        public MailSendFailEventArgs(int index, string msg, string mailId, string address, string host)
        {
            this.Index = index;
            this.MailID = mailId;
            this.Address = address;
            this.Text = msg;
            this.Host = host;
        }
    }

    public class MailJobSatrtEventArgs : EventArgs
    {
        public string ID { get; private set; }

        public MailJobSatrtEventArgs(string id)
        {
            this.ID = id;
        }
    }

    public class MailJobRunEventArgs : EventArgs
    {
        public string ID { get; private set; }

        public MailJobRunEventArgs(string id)
        {
            this.ID = id;
        }
    }

    public class MailLogEventArgs : EventArgs
    {
        public string ID { get; private set; }
        public string Message { get; private set; }

        public MailLogEventArgs(string id, string message)
        {
            this.ID = id;
            this.Message = message;
        }
    }
}
