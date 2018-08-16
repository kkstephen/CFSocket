using System;

namespace CFS.Net
{
    public interface ICFMessageFactory
    {
        ICFMessage Create(MessageName name, string str);
    }
}
