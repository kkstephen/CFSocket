using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFS.SMTP
{
    [Serializable]
    public class MNRecipient
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateSend { get; set; }
        public string Status { get; set; }
    }
}
