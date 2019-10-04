using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class EndOfCentralDirectory64
    {
        internal static uint Size => 5 * sizeof(ulong) + 3 * sizeof(uint) + 2 * sizeof(ushort);

        internal EndOfCentralDirectory64(byte[] buffer)
        {
            if(buffer.Length >= EndOfCentralDirectory64.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    this.Signature = reader.ReadUInt32();
                    this.EndOfCentralDirectoryRecordSize = reader.ReadUInt64();
                    this.VersionMadeBy = reader.ReadUInt16();
                    this.VersionNeeded = reader.ReadUInt16();
                    this.DiskNumber = reader.ReadUInt32();
                    this.StartCentralDirectoryDiskNumber = reader.ReadUInt32();
                    this.DiskCentralDirectoryRecordCount = reader.ReadUInt64();
                    this.CentralDirectoryRecordCount = reader.ReadUInt64();
                    this.CentralDirectorySize = reader.ReadUInt64();
                    this.CentralDirectoryStartOffset = reader.ReadUInt64();
                }
            }
            else
            {
                throw new Exception("The supplied buffer is too small");
            }
        }

        internal uint Signature { get; set; }

        internal ulong EndOfCentralDirectoryRecordSize { get; set; }

        internal ushort VersionMadeBy { get; set; }

        internal ushort VersionNeeded { get; set; }

        internal uint DiskNumber { get; set; }

        internal uint StartCentralDirectoryDiskNumber { get; set; }

        internal ulong DiskCentralDirectoryRecordCount { get; set; }

        internal ulong CentralDirectoryRecordCount { get; set; }

        internal ulong CentralDirectorySize { get; set; }

        internal ulong CentralDirectoryStartOffset { get; set; }
    }
}
