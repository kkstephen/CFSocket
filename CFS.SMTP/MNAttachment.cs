using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CFS.SMTP
{
    [Serializable]
    public class MNAttachment
    {
        public string FileName { get; private set; }
        public long Size { get { return this.getSize(); } }

        public MNAttachment(string file)
        {
            FileName = file;
        }

        private long getSize()
        {
            FileInfo f = new FileInfo(FileName);

            return f.Length;
        }
    }
}
