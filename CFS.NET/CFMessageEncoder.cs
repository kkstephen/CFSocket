using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public abstract class CFMessageEncoder : ICFMessageEncoder
    {
        public abstract ICFMessage Decode(string str);        
    }

    public class CFServerMessageEncoder : CFMessageEncoder
    { 
        public override ICFMessage Decode(string data)
        {        
            if (string.IsNullOrEmpty(data) || data.Length < 3)
                throw new CFException("Invalid data to decode");

            CFServerMessage message = new CFServerMessage();
             
            message.Status = data.Substring(0, 3);

            if (data.Length > 3)
                message.Data = data.Substring(4);

            return message;
        }
    }

    public class CFClientMessageEncoder : CFMessageEncoder
    {
        public override ICFMessage Decode(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Length < 4)
                throw new CFException("Invalid data to decode");

            CFClientMessage message = new CFClientMessage();

            message.Command = data.Substring(0, 4);

            if (data.Length > 4)
                message.Data = data.Substring(5);

            return message;
        }
    }
}
