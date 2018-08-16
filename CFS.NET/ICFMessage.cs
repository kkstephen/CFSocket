using System;

namespace CFS.Net
{
    public interface ICFMessage
    {
        string Boundary { get; }
        int Offset { get; }
        string Data { get; }         
    }
}
