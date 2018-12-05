using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFS.SMTP
{
    [Serializable]
    public class MNMessage
    {
        public int Index { get; set; }
        public string ID { get; set; }
        
        public IRecipient Recipient { get; set; }
        public string FromName { get; set; }
        public string FromAddr { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AlterBody { get; set; }

        public bool IsHTML { get; set; }
        public string CharSet { get; set; }

        public List<string> Attachments { get; set; }

        public MNMessage()
        {
            Attachments = new List<string>();           
        } 
    }
}
