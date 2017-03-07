using System; 

namespace CFS.Net
{
    public abstract class CFMessage : ICFMessage
    {
        public int Offset { get; set; }
        public string Data { get; set; }
    }
        
    public class CFClientMessage : CFMessage
    {
        public string Method { get; set; }

        public CFClientMessage(int offset)
        {
            this.Offset = offset;

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

        public CFServerMessage(int offset)
        {
            this.Offset = offset;

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
