using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class EndOfCentralDirectory
    {
        internal static uint Size => 3 * sizeof(uint) + 5 * sizeof(ushort);

        internal EndOfCentralDirectory(byte[] buffer)
        {
            if(buffer.Length >= EndOfCentralDirectory.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    this.Signature = reader.ReadUInt32();
                    this.DiskNumber = reader.ReadUInt16();
                    this.StartCentralDirectoryDiskNumber = reader.ReadUInt16();
                    this.DiskCentralDirectoryRecordCount = reader.ReadUInt16();
                    this.CentralDirectoryRecordCount = reader.ReadUInt16();
                    this.CentralDirectorySize = reader.ReadUInt32();
                    this.CentralDirectoryStartOffset = reader.ReadUInt32();
                    this.CommentLength = reader.ReadUInt16();
                }
            }
            else
            {
                throw new Exception("The supplied buffer is too small");
            }
        }

        internal uint Signature { get; set; }

        internal ushort DiskNumber { get; set; }

        internal ushort StartCentralDirectoryDiskNumber { get; set; }

        internal ushort DiskCentralDirectoryRecordCount { get; set; }

        internal ushort CentralDirectoryRecordCount { get; set; }

        internal uint CentralDirectorySize { get; set; }

        internal uint CentralDirectoryStartOffset { get; set; }

        internal ushort CommentLength { get; set; }

        internal bool IsZip64 => this.DiskNumber == ushort.MaxValue || 
            this.StartCentralDirectoryDiskNumber == ushort.MaxValue ||
            this.DiskCentralDirectoryRecordCount == ushort.MaxValue || 
            this.CentralDirectorySize == uint.MaxValue ||
            this.CentralDirectoryStartOffset == uint.MaxValue || 
            this.CommentLength == ushort.MaxValue;
    }
}
