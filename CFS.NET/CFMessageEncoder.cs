using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public class CFServerMessageEncoder : ICFMessageEncoder
    {
        public ICFProtocol Protocol { get; set; }

        public CFServerMessageEncoder(ICFProtocol protocol)
        {
            this.Protocol = protocol;
        }

        public virtual ICFMessage Decode(string data)
        {
            CFServerMessage message = this.Protocol.MessageFactory.Create(MessageName.SERVER) as CFServerMessage;

            if (string.IsNullOrEmpty(data) || data.Length < message.Offset)
                throw new Exception("Invalid data to decode");
             
            message.Response = data.Substring(0, message.Offset);

            if (data.Length > message.Offset)
            {
                message.Data = data.Substring(message.Offset + 1);
            }

            return message;
        }

        public virtual string Encode(ICFMessage message)
        {
            CFClientMessage sm = message as CFClientMessage;

            return string.Format(this.Protocol.GetMessageFormat(), sm.Method, sm.Data); 
        }
    }

    public class CFClientMessageEncoder : ICFMessageEncoder
    {
        public ICFProtocol Protocol { get; set; }

        public CFClientMessageEncoder(ICFProtocol protocol)
        {
            this.Protocol = protocol;
        }

        public virtual ICFMessage Decode(string data)
        {
            CFClientMessage message = this.Protocol.MessageFactory.Create(MessageName.CLIENT) as CFClientMessage;

            if (string.IsNullOrEmpty(data) || data.Length < message.Offset)
                throw new Exception("Invalid data to decode");

            message.Method = data.Substring(0, message.Offset);

            if (data.Length > message.Offset)
            {
                message.Data = data.Substring(message.Offset + 1);
            }

            return message;
        }

        public virtual string Encode(ICFMessage message)
        {
            CFServerMessage cm = message as CFServerMessage;

            return string.Format(this.Protocol.GetMessageFormat(), cm.Response, cm.Data);
        }
    }
}
