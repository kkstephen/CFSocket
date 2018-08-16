using System;

namespace CFS.Net
{
    public interface ICFMessageEncoder
    { 
        ICFMessage Decode(string str);
        string Encode(ICFMessage message);
    }
}
