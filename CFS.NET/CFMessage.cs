using System; 

namespace CFS.Net
{
    public abstract class CFMessage : ICFMessage
    {
        public string Data { get; set; }        
    }
        
    public class CFClientMessage : CFMessage
    {
        public string Command { get; set; }
 
        public override string ToString()
        {
            return string.Format("{0} {1}", Command, Data);
        }
    }

    public class CFServerMessage : CFMessage
    {
        public string Status { get; set; } 

        public override string ToString()
        {
            return string.Format("{0} {1}", Status, Data);
        }
    }
}
