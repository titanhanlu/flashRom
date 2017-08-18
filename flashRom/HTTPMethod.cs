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

        public static string getDataFromUrl(string url, Dictionary<string, string> paramDic, string bindIP)
        {
            string finalUrl = BuildUrl(url, paramDic);
            return getDataFromUrl(finalUrl, bindIP);
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

        public static string postFileToUrl(string filePath, string url, string bindIP)
        {

            string fileFormName = "image";
            string contenttype = "application/octet-stream";
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ServicePoint.BindIPEndPointDelegate = delegate (ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
            {
                if (remoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return new IPEndPoint(IPAddress.Parse(bindIP), 0);
                }
                return null;
            };

            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";

            // Build up the post message header  
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("");
            sb.AppendLine();
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(filePath));
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.AppendLine();
            sb.AppendLine();

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array  
            // ensuring the boundary appears on a line by itself  
            byte[] boundaryBytes =
                Encoding.ASCII.GetBytes(Environment.NewLine + "--" + boundary +"--"+ Environment.NewLine);
            FileStream fileStream = null;
            WebResponse response = null;
            StreamReader sr = null;
            string result = null;
            try
            {
                fileStream = new FileStream(filePath,
                              FileMode.Open, FileAccess.Read);
                long length = postHeaderBytes.Length + fileStream.Length +
                                    boundaryBytes.Length;
                webrequest.ContentLength = length;

                Stream requestStream = webrequest.GetRequestStream();

                // Write out our post header  
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                // Write out the file contents  
                byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                             (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);

                // Write out the trailing boundary  
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                response = webrequest.GetResponse();
                sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
            }
            catch (Exception e)
            { }
            finally
            {
                response.Close();
                fileStream.Close();
                webrequest.Abort();
                sr.Close();
            }

            return result;
        }

        public static string getDataFromUrl(string url, string bindIP)
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
