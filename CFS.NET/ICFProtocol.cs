using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public interface ICFProtocol
    {
        int MethodOffet { get; }
        int ResponseOffset { get; }
        string EndLine { get; }

        string GetMessageFormat();
    }
}
