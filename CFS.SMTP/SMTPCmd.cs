using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFS.SMTP
{
    public class SMTPCmd
    {
        public const string Hello = "HELO";
        public const string Form = "MAIL FROM";
        public const string To = "RCPT TO";
        public const string AUTH = "AUTH LOGIN";
        public const string Data = "DATA";
        public const string Quit = "QUIT";
    }
}
