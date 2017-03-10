using System; 

namespace CFS.Net
{
    public abstract class CFMessage : ICFMessage
    {
        public int Offset { get; set; }
        public string Data { get; set; }
    } 
}
