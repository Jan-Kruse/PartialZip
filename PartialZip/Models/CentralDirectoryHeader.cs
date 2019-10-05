using PartialZip.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class CentralDirectoryHeader
    {
        internal static uint Size => 6 * sizeof(uint) + 11 * sizeof(ushort);

        internal CentralDirectoryHeader(BinaryReader reader)
        {
            this.Signature = reader.ReadUInt32();
            this.VersionMade = reader.ReadUInt16();
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
            this.FileCommentLength = reader.ReadUInt16();
            this.DiskNumberStart = reader.ReadUInt16();
            this.InternalFileAttributes = reader.ReadUInt16();
            this.ExternalFileAttributes = reader.ReadUInt32();
            this.LocalHeaderOffset = reader.ReadUInt32();

            this.FileName = new string(reader.ReadChars(this.FileNameLength));

            if(this.ExtraFieldLength >= ExtendedInformationExtraField64.Size)
                this.ExtraField = new ExtendedInformationExtraField64(reader.ReadBytes(this.ExtraFieldLength));

            this.FileComment = new string(reader.ReadChars(this.FileCommentLength));
        }

        internal static IEnumerable<CentralDirectoryHeader> GetFromBuffer(byte[] buffer, ulong cdEntries)
        {
            if (buffer.Length >= EndOfCentralDirectory.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    ulong entriesRead = 0;

                    while(reader.BaseStream.Position + CentralDirectoryHeader.Size <= reader.BaseStream.Length && entriesRead < cdEntries)
                    {
                        yield return new CentralDirectoryHeader(reader);

                        entriesRead++;
                    }
                }
            }
            else
            {
                throw new PartialZipParsingException("Failed to parse central directory headers. The supplied buffer is too small");
            }
        }

        internal (ulong, ulong, ulong, uint) GetFileInfo()
        {
            int extraIndex = 0;

            ulong uncompressedSize = (this.UncompressedSize == uint.MaxValue) ? this.ExtraField.ExtraField[extraIndex++] : this.UncompressedSize;
            ulong compressedSize = (this.CompressedSize == uint.MaxValue) ? this.ExtraField.ExtraField[extraIndex++] : this.CompressedSize;
            ulong headerOffset = (this.LocalHeaderOffset == uint.MaxValue) ? this.ExtraField.ExtraField[extraIndex++] : this.LocalHeaderOffset;
            uint diskNum = (this.DiskNumberStart == ushort.MaxValue) ? (uint)this.ExtraField.ExtraField[extraIndex++] : this.DiskNumberStart;

            return (uncompressedSize, compressedSize, headerOffset, diskNum);
        }

        internal uint Signature { get; set; }

        internal ushort VersionMade { get; set; }

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

        internal ushort FileCommentLength { get; set; }

        internal ushort DiskNumberStart { get; set; }

        internal ushort InternalFileAttributes { get; set; }

        internal uint ExternalFileAttributes { get; set; }

        internal uint LocalHeaderOffset { get; set; }

        internal string FileName { get; set; }

        internal ExtendedInformationExtraField64 ExtraField { get; set; }

        internal string FileComment { get; set; }
    }
}
