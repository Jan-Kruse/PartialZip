using PartialZip.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class EndOfCentralDirectoryLocator64
    {
        internal static uint Size => 3 * sizeof(uint) + 1 * sizeof(ulong);

        internal EndOfCentralDirectoryLocator64(byte[] buffer)
        {
            if (buffer.Length >= EndOfCentralDirectoryLocator64.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    this.Signature = reader.ReadUInt32();
                    this.StartCentralDirectory64DiskNumber = reader.ReadUInt32();
                    this.EndOfCentralDirectory64StartOffset = reader.ReadUInt64();
                    this.DiskCount = reader.ReadUInt32();
                }
            }
            else
            {
                throw new PartialZipParsingException("Failed to parse end of ZIP64 central directory locator. The supplied buffer is too small");
            }
        }

        internal uint Signature { get; set; }

        internal uint StartCentralDirectory64DiskNumber { get; set; }

        internal ulong EndOfCentralDirectory64StartOffset { get; set; }

        internal uint DiskCount { get; set; }
    }
}
