using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;


namespace flashRom
{
    class HTTPMethod
    {
        public static string postDataToUrl(string data, string url, Dictionary<string, string> paramDic, string bindIP)
        {
            string finalUrl = BuildUrl(url, paramDic);
            return postDataToUrl(data, finalUrl, bindIP);
        }

        public static string getDataToUrl(string url, Dictionary<string, string> paramDic, string bindIP)
        {
            string finalUrl = BuildUrl(url, paramDic);
            return getDataToUrl(finalUrl, bindIP);
        }
        public static string postDataToUrl(string data, string url, string bindIP)
        {
            string result = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.BindIPEndPointDelegate = delegate (ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
                {
                    if (remoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return new IPEndPoint(IPAddress.Parse(bindIP), 0);
                    }
                    return null;
                };
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] btBody = Encoding.UTF8.GetBytes(data);
                request.ContentLength = btBody.Length;
                request.GetRequestStream().Write(btBody, 0, btBody.Length);
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
            }
            finally
            {
                response.Close();
                streamReader.Close();
                request.Abort();
                response.Close();
            }
            return result;
        }

        public static string getDataToUrl(string url, string bindIP)
        {
            string result = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.BindIPEndPointDelegate = delegate (ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
                {
                    if (remoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return new IPEndPoint(IPAddress.Parse(bindIP), 0);
                    }
                    return null;
                };
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.UserAgent = "Mozilla / 5.0(X11; Linux x86_64) AppleWebKit / 537.36(KHTML, like Gecko) Ubuntu Chromium/ 44.0.2403.89 Chrome / 44.0.2403.89 Safari / 537.36";
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
            }
            finally
            {
                response.Close();
                streamReader.Close();
                request.Abort();
                response.Close();
            }
            return result;
        }

        public static string BuildUrl(string url, Dictionary<string, string> paramDic)
        {
            if (paramDic == null || paramDic.Count == 0)
            {
                return url;
            }
            foreach (var key in paramDic.Keys)
            {
                if (url.Contains("?"))
                {
                    url += "&" + key + "=" + HttpUtility.UrlEncode(paramDic[key], Encoding.UTF8);
                }
                else
                {
                    url += "?" + key + "=" + HttpUtility.UrlEncode(paramDic[key], Encoding.UTF8);
                }
            }
            return url;
        }
    }
}
