using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using CFS.Net;

namespace CFS.SMTP
{
    public class CFSmtp : CFConnection
    {        
        public event EventHandler<MailSendSuccessEventArgs> OnSendSuccess;

        public int Attempts { get; set; } 
            
        //User login
        public bool UseAuthenticate { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }

        private MNMessage email { get; set; }

        public string MailAgent { get; set; }
       
        // number of chars on one line
        private int bodyWidth = 76;

        //enf of line
        private const string CRLF = "\r\n";       
        
        private StringBuilder logs;
        
        public int BufferSize { get; set; }
  
        public string Log 
        { 
            get 
            {
                return logs.ToString();               
            } 
        }  
             
        public CFSmtp(string host, int port)
        {
            this.BufferSize = 16348;

            this.UseAuthenticate = false;
            this.LoginName = "";
            this.Password = "";

            this.Host = host;
            this.Port = port;            

            this.logs = new StringBuilder();

            this.Timeout = 10;
            this.Attempts = 2;

            this.OnOpen += CFSmtp_OnOpen;
        }

        private void CFSmtp_OnOpen(object sender, SessionOpenEventArgs e)
        {                          
            this.transmail();

            if (OnSendSuccess != null)
            {
                OnSendSuccess(this, new MailSendSuccessEventArgs(this.email.Index, this.email.ID, "Send OK."));
            }   

            return;                 
        }

        public void Send(MNMessage message)
        {
            if (!message.Recipient.Address.IsValidEmail())
                throw new Exception("Receipt email address is invalid.");

            if (!message.FromAddr.IsValidEmail())
                throw new Exception("Sender email address is invalid.");

            if (this.Host.Length == 0)
                throw new Exception("SMTP host is required.");

            if (this.UseAuthenticate)
            {
                if (string.IsNullOrEmpty(this.LoginName))
                    throw new Exception("Login name is required.");

                if (string.IsNullOrEmpty(this.Password))
                    throw new Exception("Password is required.");
            }

            if (string.IsNullOrEmpty(message.ID))
                throw new Exception("Message ID is required.");

            this.email = message;

            this.Connect();
        }

        public override void Send(string data)
        {
            this.Stream.WriteLine(data);
        }
         
        private void transmail()
        {      
            string message;
            string response;

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.SUCCESS))
            {
                throw new CFException(response);
            };                  

            //HELO
            message = string.Format("{0} {1}", SMTPCmd.Hello, this.email.FromAddr.GetDomain());

            this.Send(message);
            this.addLogLine(" > ", message);

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.OK))
            {
                throw new CFException(response);
            }
            
            //AUTH LOGIN
            if (UseAuthenticate)
            {
                message = SMTPCmd.AUTH;
                this.Send(message);
                this.addLogLine(" > ", message);

                response = this.Receive();
                this.addLog(" < ", response);

                if (!response.StartsWith(SMTPCode.AuthBase64))
                {
                    throw new CFException(response);
                }

                //Name
                message = this.LoginName.Base64Encode();

                this.Send(message);
                this.addLogLine(" > ", message);

                response = this.Receive();
                this.addLog(" < ", response);

                if (!response.StartsWith(SMTPCode.AuthBase64))
                {
                    throw new CFException(response);
                }

                //Password
                message = this.Password.Base64Encode();

                this.Send(message);
                this.addLogLine(" > ", message);
                
                response = this.Receive();
                this.addLog(" < ", response);

                if (!response.StartsWith(SMTPCode.UserPass))
                {
                    throw new CFException(response);
                }
            }

            //From
            message = string.Format("{0}:<{1}>", SMTPCmd.Form, this.email.FromAddr);

            this.Send(message);
            this.addLogLine(" > ", message);

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.OK))
            {
                throw new CFException(response);
            }

            //To
            message = string.Format("{0}:<{1}>", SMTPCmd.To, this.email.Recipient.Address);

            this.Send(message);
            this.addLogLine(" > ", message);

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.OK))
            {
                throw new CFException(response);
            }

            //Data
            message = SMTPCmd.Data;

            this.Send(message);
            this.addLogLine(" > ", message);

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.DataInput))
            {
                throw new CFException(response);
            }

            //Mail header
            StringBuilder mHeader = new StringBuilder();
         
            string m_bound = this.getBoundary();
            string t_bound = this.getBoundary();

            string body = this.email.Body.Base64Encode().ChunkSplit(this.bodyWidth);
            string altbody = this.email.AlterBody.Base64Encode().ChunkSplit(this.bodyWidth);

            string mid = this.getMessageId();

            string form = "=?utf-8?B?" +  this.email.FromName.Base64Encode() + "?= <" + this.email.FromAddr + ">";
            string to = this.email.Recipient.Address;

            if (this.email.Recipient.Name.Trim() != "")
            {
                to = "=?utf-8?B?" + this.email.Recipient.Name.Base64Encode() + "?= <" + this.email.Recipient.Address + ">";
            }

            mHeader.Append("Message-ID: " + mid + CRLF);
            mHeader.Append("Return-path: <" + this.email.FromAddr + ">" + CRLF);
            
            mHeader.Append("From: " + form + CRLF);           
            mHeader.Append("To: " + to + CRLF);

         //   mail.Append("Reply-To: \"" + Message.FromName + "\" <" + Message.FromAddr + ">" + this.CRLF);
            mHeader.Append("Date: " + DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss zz") + CRLF);
            mHeader.Append("Subject: =?utf-8?B?" + this.email.Subject.Base64Encode() + "?=" + CRLF);
            mHeader.Append("X-Priority: 3" + CRLF);
            mHeader.Append("X-Mailer: " + MailAgent + CRLF);
            mHeader.Append("MIME-Version: 1.0" + CRLF);        
                      
            StringBuilder attatchments = new StringBuilder();

            // has attachment
            if (this.email.Attachments.Count > 0)
            {
                mHeader.Append("Content-Type: multipart/mixed;" + CRLF);
                mHeader.Append(" ".PadRight(8, ' ') + "boundary=\"" + m_bound + "\"" + CRLF);
                mHeader.Append(CRLF);

                mHeader.Append("--" + m_bound + CRLF);                
               
                foreach (string file in this.email.Attachments)
                {                     
                    if (File.Exists(file))
                    {
                        attatchments.Append("--" + m_bound + CRLF);
                        attatchments.Append("Content-Type:" + MIME.GetType(file) + CRLF);
                        attatchments.Append("Content-Transfer-Encoding: base64" + CRLF);
                        attatchments.Append("Content-Disposition: attachment; filename=\"" + Path.GetFileName(file) + "\"" + CRLF + CRLF);
                        attatchments.Append(this.getFileBytes(file).Base64Encode().ChunkSplit(this.bodyWidth));       
                        attatchments.Append(CRLF);
                    }
                }
            }                       

            if (this.email.IsHTML)
            {
                mHeader.Append("Content-Type: multipart/alternative;" + CRLF);
                mHeader.Append(" ".PadRight(8, ' ') + "boundary=\"" + t_bound + "\"" + CRLF + CRLF);
                               
                //plain text
                mHeader.Append("--" + t_bound + CRLF);
                mHeader.Append("Content-Type: text/plain; charset=\"" + this.email.CharSet.ToLower() + "\"" + CRLF);
                mHeader.Append("Content-Transfer-Encoding: base64" + CRLF + CRLF);
                mHeader.Append(altbody);

                //html text
                mHeader.Append("--" + t_bound + CRLF);
                mHeader.Append("Content-Type: text/html; charset=\"" + this.email.CharSet.ToLower() + "\"" + CRLF);
                mHeader.Append("Content-Transfer-Encoding: base64" + CRLF + CRLF);
                mHeader.Append(body);

                mHeader.Append("--" + t_bound + "--" + CRLF);
            }
            else
            {
                //plain text               
                mHeader.Append("Content-Type: text/plain; charset=\"" + this.email.CharSet.ToLower() + "\"" + CRLF);
                mHeader.Append("Content-Transfer-Encoding: base64" + CRLF + CRLF);
                mHeader.Append(body);
            }            

            //attachment parts
            if (this.email.Attachments.Count > 0)
            {
                mHeader.Append(CRLF);
                mHeader.Append(attatchments.ToString());
                mHeader.Append("--" + m_bound + "--");
            }           

            //end of mail
            mHeader.Append(CRLF + ".");

            message = mHeader.ToString();
                       
            this.Send(message);
            this.addLogLine(" > ", "send data ...");

            response = this.Receive();
            this.addLog(" < ", response);

            if (!response.StartsWith(SMTPCode.OK))
            {
                throw new CFException(response);
            }

            this.Quit();
        }

        public void Quit()
        {                       
            this.Send(SMTPCmd.Quit);
            this.addLogLine(" > ", SMTPCmd.Quit);

            this.addLog(" < ", this.Receive());            
        }

        private string getMessageId()
        {
            string dt = DateTime.Now.ToString("yyyyMMddHHmmss");

            return string.Format("<{0}-{1}@{2}>", dt, email.ID, email.FromAddr.GetDomain());            
        } 

        private string getBoundary()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private string getFileBytes(string FilePath)
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                byte[] buffer = new byte[Convert.ToInt32(fs.Length)];

                fs.Read(buffer, 0, buffer.Length);
                
                return Convert.ToBase64String(buffer);
            }            
        }
        
        private void addLog(string t, string str)
        {
            this.logs.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + t + str);                          
        }

        private void addLogLine(string t, string s)
        {
            this.addLog(t, s + CRLF);
        }   
    }
}
