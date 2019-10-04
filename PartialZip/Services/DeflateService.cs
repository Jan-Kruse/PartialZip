using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PartialZip.Services
{
    internal class DeflateService
    {
        internal byte[] Inflate(byte[] buffer)
        {
            using (MemoryStream input = new MemoryStream(buffer))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (DeflateStream deflate = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        deflate.CopyTo(output);
                        deflate.Close();
                        output.Position = 0;
                        return output.ToArray();
                    }
                }
            }
        }
    }
}
