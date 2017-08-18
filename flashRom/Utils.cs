using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace flashRom
{
    class Utils
    {
        public static string Sha1(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] data = SHA1.Create().ComputeHash(buffer);
            return BitConverter.ToString(data).Replace("-", String.Empty).ToLower();
            //StringBuilder sb = new StringBuilder();
            //foreach (var t in data)
            //{
            //    sb.Append(t.ToString("X2"));
            //}
            //return sb.ToString();
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
