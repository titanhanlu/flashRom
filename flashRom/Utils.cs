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
            StringBuilder sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
