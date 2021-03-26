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
        private string _url;

        private Task<WebHeaderCollection> _contentHeaders;

        internal HttpService(string url)
        {
            this._url = url;
            this._contentHeaders = this.GetContentInfo();
        }

        internal async Task<ulong> GetContentLength()
        {
            WebHeaderCollection headers = await this._contentHeaders;
            return ulong.Parse(headers.Get("Content-Length"));
        }

        internal async Task<bool> SupportsPartialZip()
        {
            WebHeaderCollection headers = await this._contentHeaders;
            return headers.Get("Accept-Ranges") == "bytes";
        }

        private async Task<WebHeaderCollection> GetContentInfo()
        {
            HttpWebRequest request = WebRequest.CreateHttp(this._url);
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Method = "HEAD";

            using (WebResponse response = await request.GetResponseAsync())
            {
                return response.Headers;
            }
        }

        internal async Task<byte[]> GetRange(ulong startBytes, ulong endBytes)
        {
            if(startBytes < endBytes)
            {
                HttpWebRequest request = WebRequest.CreateHttp(this._url);
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.AddRange((long)startBytes, (long)endBytes);
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
