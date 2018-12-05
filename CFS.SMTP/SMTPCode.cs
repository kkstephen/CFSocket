using System;
 
namespace CFS.SMTP
{
    public class SMTPCode
    {
        public const string Ready = "100";
        public const string Correct = "200";
        public const string Authentication = "300";
        public const string ClientErr = "400";
        public const string ServerErr = "500";

        public const string SUCCESS = "220";
        public const string OK = "250";
        public const string Close = "221";
        public const string UserPass = "235";
        public const string AuthBase64 = "334";
        public const string DataInput = "354";
    }
}
