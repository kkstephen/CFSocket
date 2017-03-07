using System;

namespace CFS.Net
{
    public abstract class CFMessageFactory : ICFMessageFactory
    {
        public abstract ICFMessage Create(MessageName name);     
    }
}
