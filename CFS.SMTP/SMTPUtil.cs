using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CFS.SMTP
{
    public static class StringExtensions
    {
        public static string GetDomain(this string addr)
        {
            if (string.IsNullOrEmpty(addr))
                throw new Exception("Email address is empty.");

            int pos = addr.IndexOf("@");

            if (pos != -1)
                return addr.Substring(pos + 1);
            else
                return addr;
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email) || email.IndexOf(' ') != -1)
                return false;

            string pattern = @"[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9_\-\.]+\.[a-zA-Z]{2,5}";

            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            return check.IsMatch(email);
        }      

        public static string Base64Encode(this string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str.ToCharArray());

            return Convert.ToBase64String(buffer);
        }

        public static string Base64Decode(this string str)
        {
            byte[] buffer = Convert.FromBase64String(str);

            return System.Text.Encoding.Default.GetString(buffer);
        }

        public static string ChunkSplit(this string str, int chunkSize)
        {
            int strLength = str.Length;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < strLength; i += chunkSize)
            {
                if (i + chunkSize > strLength) chunkSize = strLength - i;

                sb.AppendLine(str.Substring(i, chunkSize));
            }

            return sb.ToString();
        }
    }
}
