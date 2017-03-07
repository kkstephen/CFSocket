using System;

namespace CFS.Net
{
    public interface ICFMessage
    {
        int Offset { get; }
        string Data { get; }         
    }

    public interface ICFMessageEncoder
    {        
        ICFMessage Decode(string str);
        string Encode(ICFMessage message);
    }
}
