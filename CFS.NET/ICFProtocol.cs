using System;

namespace CFS.Net
{
    public interface ICFProtocol
    { 
        int MethodOffset { get; }
        int ResponseOffset { get; }

        ICFMessageFactory MessageFactory { get; }
        string GetMessageFormat();
    }
}
