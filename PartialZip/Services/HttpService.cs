using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartialZip.Services
{
    internal class HttpService
    {
        internal async Task<ulong> GetContentLength(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Method = "HEAD";

            using (WebResponse response = await request.GetResponseAsync())
            {
                string acceptRanges = response.Headers.Get("Accept-Ranges");
                string headerValue = response.Headers.Get("Content-Length");

                if (acceptRanges == "bytes")
                    return ulong.Parse(headerValue);
                else
                    throw new Exception("The web server does not support the Range header.");
            }
        }

        internal async Task<byte[]> GetRange(string url, ulong startBytes, ulong endBytes)
        {
            if(startBytes < endBytes)
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Headers.Add("Range", $"bytes={startBytes}-{endBytes}");
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream output = new MemoryStream())
                        {
                            await responseStream.CopyToAsync(output);
                            return output.ToArray();
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Invalid byte range.");
            }
        }
    }
}
