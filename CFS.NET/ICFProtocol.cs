using System;

namespace CFS.Net
{
    public interface ICFProtocol
    {
        string EndLine { get; }
        int MethodOffset { get; }
        int ResponseOffset { get; }

        ICFMessageFactory MessageFactory { get; }
        string GetMessageFormat();
    }
}
