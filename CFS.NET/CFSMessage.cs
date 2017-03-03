using System; 

namespace CFS.Net
{
    public abstract class CFSMessage : ICFMessage
    {
        public string Data { get; set; }
        public abstract void Parse(string str); 
    }

    public class CFSClientMessage : CFSMessage
    {
        public string Command { get; set; }

        public override void Parse(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 4)
                throw new CFException("invalid command");

            this.Command = str.Substring(0, 4);

            if (str.Length > 4)
                this.Data = str.Remove(0, 5);
            else
                this.Data = "";
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Command, Data);
        }
    }

    public class CFSServerMessage : CFSMessage
    {
        public string Status { get; set; }

        public override void Parse(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 3)
                throw new CFException("invalid status");

            this.Status = str.Substring(0, 3);

            if (str.Length > 3)
                this.Data = str.Remove(0, 4);
            else
                this.Data = "";
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Status, Data);
        }
    }
}
