using System;
 
namespace CFS.Net
{
    public abstract class CFProtocol : ICFProtocol
    {  
        public ICFMessageFactory MessageFactory { get; set; }

        public string Format { get; set; }

        public string Translate(string status, string data)
        {
            return string.Format(this.Format, status, data);
        }
    }
}
