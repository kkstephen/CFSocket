using System; 

namespace CFS.Net
{
    public abstract class CFMessage : ICFMessage
    {
        public string Boundary { get; set; }
        public int Offset { get; set; }
        public string Data { get; set; }

        public CFMessage()
        {
            this.Boundary = "\r\n";
        }
    } 
}
