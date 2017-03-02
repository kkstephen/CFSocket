using System;

namespace CFS.Net
{
    public interface ICFMessage
    {        
        string Data { get; }
        void Parse(string str);
    }
}
