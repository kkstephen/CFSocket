using System;

namespace CFS.Net
{
    public class CFException : Exception
    { 
        public CFException(string error) : base(error)
        {
        }        
    }
}
