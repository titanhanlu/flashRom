using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

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
        public string token = null;

        public RouterApi(string userName, string pwd, string hostIP, string bindIP)
        {
            this.username = userName;
            this.password = pwd;
            this.hostIP = hostIP;
            this.bindIP = bindIP;
            this.baseUrl = "http://" + this.hostIP + "/cgi-bin/luci/";
        }

        public string getToken()
        {
            string nonce = getNonce();
            string pwd = pwdEncryption(nonce, this.password);

            string result = HTTPMethod.postDataToUrl("username=" + username + "&password=" + pwd + "&nonce=" + nonce, baseUrl
            + "api/xqsystem/token", this.bindIP);
            TokenResult tokenResult = JsonConvert.DeserializeObject<TokenResult>(result);
            return tokenResult.token;

        }

        public static string getNonce()
        {
            int source = new Random().Next();
            int ranNum = Math.Abs(source % (1000));
            string nonceType = "0";
            string nonceDevice = "B8:CA:3A:AF:9C:3D";
            string nonce = nonceType + "_" + nonceDevice + "_"
                + Utils.CurrentTimeMillis() + "_" + ranNum;
            return nonce;
        }

        public static string pwdEncryption(string nonce, string pwd)
        {
            string digest = Utils.Sha1(pwd + sha1Key);
            string password = Utils.Sha1(nonce + digest);
            return password;
        }

        public string UploadRom(string filePath)
        {
            if(!System.IO.File.Exists(filePath))
            {
                return null;
            }
            if (this.token == null)
            {
                this.token = getToken();
            }
            string url = String.Format(baseUrl + ";stok={0}/api/xqsystem/upload_rom", this.token);
            string result = HTTPMethod.postFileToUrl(filePath, url, this.bindIP);
            return result;

        }

        public string getInitInfo()
        {
            string url = baseUrl + "api/xqsystem/init_info";
            string result = HTTPMethod.getDataFromUrl(url, this.bindIP);
            return result;
        }


        public string getLuciApi(Dictionary<string, string> paramDic, string url)
        {
            if (this.token == null)
            {
                this.token = getToken();
            }
            string dcUrl = String.Format(baseUrl + ";stok={0}/" + url, this.token);
            string result = HTTPMethod.getDataFromUrl(dcUrl, paramDic, this.bindIP);
            return result;
        }
    }

    class TokenResult
    {
        public string id { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public int code { get; set; }
    }
}
