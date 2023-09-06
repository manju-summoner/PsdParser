using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser
{
    internal class PsdBinaryReader : BinaryReader
    {
        public PsdBinaryReader(Stream input) : base(input)
        {
        }

        public override ushort ReadUInt16() => BitConverter.ToUInt16(ReadBigEndian(sizeof(ushort)));
        public override short ReadInt16()=> BitConverter.ToInt16(ReadBigEndian(sizeof(short)));

        public override uint ReadUInt32() => BitConverter.ToUInt32(ReadBigEndian(sizeof(uint)));
        public override int ReadInt32() => BitConverter.ToInt32(ReadBigEndian(sizeof(int)));

        public override long ReadInt64() => BitConverter.ToInt64(ReadBigEndian(sizeof(long)));
        public override ulong ReadUInt64() => BitConverter.ToUInt64(ReadBigEndian(sizeof(ulong)));

        public override float ReadSingle() => BitConverter.ToSingle(ReadBigEndian(sizeof(float)));
        public override double ReadDouble() => BitConverter.ToDouble(ReadBigEndian(sizeof(double)));
        public override decimal ReadDecimal() => BitConverter.ToInt32(ReadBigEndian(sizeof(decimal)));

        public string ReadPascalString(int align) => ReadPascalString(align, out _);
        public string ReadPascalString(int align, out int readCount)
        {
            readCount = ReadByte();
            var bytes = ReadBytes(readCount);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var result = Encoding.GetEncoding("shift-jis").GetString(bytes, 0, readCount).TrimEnd('\0');

            readCount++;
            var padding = align - readCount % align;
            if (padding != align)
            {
                readCount += padding;
                BaseStream.Position += padding;
            }
            return result;
        }

        public string ReadUnicodeString()
        {
            var length = ReadInt32() * 2;
            var bytes = ReadBytes(length);
            var txt =  Encoding.BigEndianUnicode.GetString(bytes, 0, length);
            return txt;
        }


        byte[] ReadBigEndian(int size) 
        {
            var bytes = ReadBytes(size);
            Array.Reverse(bytes);

            return bytes;
        }
    }
}
