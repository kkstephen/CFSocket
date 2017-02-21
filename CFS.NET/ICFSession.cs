using System;
using System.Net;

namespace CFS.Net
{
    public interface ICFSession : IDisposable
     { 
         IPEndPoint ClientEndPoint { get; } 
            
         bool IsAlive { get; } 
         void Begin(); 
         void Start(); 
         void End(); 
         void Close(); 
     } 
}
