using System;
 
namespace CFS.Net
{
    public abstract class CFProtocol : ICFProtocol
    { 
        public int MethodOffet { get; }
        public int ResponseOffset { get; }
        public string EndLine { get; }

        public abstract string GetMessageFormat();       
    }
}
