using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public abstract class CFClient : CFConnection 
    {  
        public CFSServerMessage Message { get; set; }

        public CFClient()
        {
            this.Message = new CFSServerMessage();
        }

        public abstract void Connect(); 
    }
}
