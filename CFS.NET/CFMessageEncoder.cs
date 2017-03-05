using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public abstract class CFMessageEncoder : ICFMessageEncoder
    {
        public ICFProtocol Protocol { get; set; }

        public abstract ICFMessage Decode(string str);        
        public abstract string Encode(ICFMessage message);
    }

    public class CFServerMessageEncoder : CFMessageEncoder
    { 
        public CFServerMessageEncoder(ICFProtocol protocol)
        {
            this.Protocol = protocol;
        }

        public override ICFMessage Decode(string data)
        {        
            if (string.IsNullOrEmpty(data) || data.Length < this.Protocol.ResponseOffset)
                throw new CFException("Invalid data to decode");

            CFServerMessage message = new CFServerMessage();
             
            message.Response = data.Substring(0, this.Protocol.ResponseOffset);
            message.Data = data.Substring(this.Protocol.ResponseOffset + 1);

            return message;
        }

        public override string Encode(ICFMessage message)
        {
            return string.Format(this.Protocol.GetMessageFormat(), message); 
        }
    }

    public class CFClientMessageEncoder : CFMessageEncoder
    {
        public CFClientMessageEncoder(ICFProtocol protocol)
        {
            this.Protocol = protocol;
        }

        public override ICFMessage Decode(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Length < this.Protocol.MethodOffet)
                throw new CFException("Invalid data to decode");

            CFClientMessage message = new CFClientMessage();

            message.Method = data.Substring(0, this.Protocol.MethodOffet);
            message.Data = data.Substring(this.Protocol.MethodOffet + 1);

            return message;
        }

        public override string Encode(ICFMessage message)
        {
            return string.Format(this.Protocol.GetMessageFormat(), message);
        }
    }
}
