using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public abstract class CFMessageEncoder : ICFMessageEncoder
    {
        public ICFProtocol Protocol { get; set; }

        public abstract ICFMessage Decode(string str);

        public abstract string Encode(ICFMessage message);    
    } 
}
