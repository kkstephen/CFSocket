using System;

namespace CFS.Net
{
    public abstract class CFMessageEncoder : ICFMessageEncoder
    {
        protected ICFProtocol Protocol { get; set; }

        public abstract ICFMessage Decode(string str);
        public abstract string Encode(ICFMessage message);    
    } 
}
