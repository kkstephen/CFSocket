using System; 

namespace CFS.Net
{
    public abstract class CFMessage : ICFMessage
    {
        public string Data { get; set; }
    }
        
    public class CFClientMessage : CFMessage
    {
        public string Method { get; set; }

        public CFClientMessage()
        {
            this.Method = "";
            this.Data = "";
        }

        public CFClientMessage(string method, string content)
        {
            this.Method = method;
            this.Data = content;
        } 
    }

    public class CFServerMessage : CFMessage
    {
        public string Response { get; set; }

        public CFServerMessage()
        {
            this.Response = "";
            this.Data = "";
        }

        public CFServerMessage(string response, string content)
        {
            this.Response = response;
            this.Data = content;
        }
    }
}
