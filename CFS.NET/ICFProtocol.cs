using System;

namespace CFS.Net
{
    public interface ICFProtocol
    { 
        string Format { get; }
        ICFMessageFactory MessageFactory { get; }

        string Translate(string status, string data);
    }
}
