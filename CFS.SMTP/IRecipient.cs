using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.SMTP
{
    public interface IRecipient
    {        
        string Name { get; set; }
        string Address { get; set; }        
    }
}
