using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public interface ICFMessageFactory
    {
        ICFMessage Create(MessageName name);
    }
}
