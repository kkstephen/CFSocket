using System;
 
namespace CFS.Net
{
    public abstract class CFProtocol : ICFProtocol
    { 
        public int MethodOffset { get; set; }         
        public int ResponseOffset { get; set; }

        public ICFMessageFactory MessageFactory { get; set; }
        public abstract string GetMessageFormat(); 
    }
}
