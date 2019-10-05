using PartialZip.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class LocalFileHeader
    {
        internal static uint Size => 4 * sizeof(uint) + 7 * sizeof(ushort);

        internal LocalFileHeader(byte[] buffer)
        {
            if (buffer.Length >= LocalFileHeader.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    this.Signature = reader.ReadUInt32();
                    this.VersionNeeded = reader.ReadUInt16();
                    this.Flags = reader.ReadUInt16();
                    this.Compression = reader.ReadUInt16();
                    this.ModifiedTime = reader.ReadUInt16();
                    this.ModifiedDate = reader.ReadUInt16();
                    this.CRC32 = reader.ReadUInt32();
                    this.CompressedSize = reader.ReadUInt32();
                    this.UncompressedSize = reader.ReadUInt32();
                    this.FileNameLength = reader.ReadUInt16();
                    this.ExtraFieldLength = reader.ReadUInt16();
                }
            }
            else
            {
                throw new PartialZipParsingException("Failed to parse local file header. The supplied buffer is too small");
            }
        }

        internal uint Signature { get; set; }

        internal ushort VersionNeeded { get; set; }

        internal ushort Flags { get; set; }

        internal ushort Compression { get; set; }

        internal ushort ModifiedTime { get; set; }

        internal ushort ModifiedDate { get; set; }

        internal uint CRC32 { get; set; }

        internal uint CompressedSize { get; set; }

        internal uint UncompressedSize { get; set; }

        internal ushort FileNameLength { get; set; }

        internal ushort ExtraFieldLength { get; set; }
    }
}
