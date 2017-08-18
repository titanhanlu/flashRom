using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace flashRom
{
    class RouterApi
    {
        private static string sha1Key = "a2ffa5c9be07488bbb04a3a47d3c5f6a";
        private static string sha1Iv = "64175472480004614961023454661220";

        private string username = null;
        private string password = null;
        private string hostIP = null;
        private string bindIP = null;
        private string baseUrl = null;

        public RouterApi(string userName, string pwd, string hostIP, string bindIP)
        {
            this.username = username;
            this.password = pwd;
            this.hostIP = hostIP;
            this.bindIP = bindIP;
            this.baseUrl = "http://" + this.hostIP + "/cgi-bin/luci/";
        }

        public string getToken()
        {
            string nonce = getNonce();
            string pwd = pwdEncryption(nonce, this.password);
            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            string result = HTTPMethod.postDataToUrl()
        }

        public static string getNonce()
        {
            int source = new Random().Next();
            int ranNum = Math.Abs(source % (1000));
            string nonceType = "0";
            string nonceDevice = "abctest";
            string nonce = nonceType + "_" + nonceDevice + "_"
                + System.DateTime.Now.Millisecond + "_" + ranNum;
            return nonce;
        }

        public static string pwdEncryption(string nonce, string pwd)
        {
            string digest = Utils.Sha1(pwd + sha1Key);
            string password = Utils.Sha1(nonce + digest);
            return password;
        }


        public string getLuciApi(Dictionary<string, string> params, string url)
        {

        }
    }
}
