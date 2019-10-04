using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PartialZip.Models
{
    internal class ExtendedInformationExtraField64
    {
        internal static uint Size => 2 * sizeof(ushort);

        internal ExtendedInformationExtraField64(byte[] buffer)
        {
            if (buffer.Length >= ExtendedInformationExtraField64.Size)
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    this.FieldTag = reader.ReadUInt16();
                    this.FieldSize = reader.ReadUInt16();

                    this.ExtraField = new ulong[(reader.BaseStream.Length - reader.BaseStream.Position) / sizeof(ulong)];

                    for (int i = 0; i < this.ExtraField.Length; i++)
                        this.ExtraField[i] = reader.ReadUInt64();
                }
            }
            else
            {
                throw new Exception("The supplied buffer is too small");
            }
        }

        internal ushort FieldTag { get; set; }

        internal ushort FieldSize { get; set; }

        internal ulong[] ExtraField { get; set; }
    }
}
