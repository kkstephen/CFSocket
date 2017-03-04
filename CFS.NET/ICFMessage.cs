using System;

namespace CFS.Net
{
    public interface ICFMessage
    {
        string Data { get; }         
    }

    public interface ICFMessageEncoder
    {
        ICFMessage Decode(string str);
    }
}
