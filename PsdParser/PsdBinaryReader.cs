using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser
{
    internal class PsdBinaryReader(Stream input) : BinaryReader(input)
    {
        //入力がメモリ上のバッファなら、デコード時にコピーを介さず直接読むために公開する。
        //(PsdFileはpubliclyVisibleなMemoryStreamを渡すためGetBufferが使える)
        public byte[]? UnderlyingBuffer { get; } = (input as MemoryStream)?.GetBuffer();

        public override ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(ReadBigEndian(stackalloc byte[sizeof(ushort)]));
        public override short ReadInt16() => BinaryPrimitives.ReadInt16BigEndian(ReadBigEndian(stackalloc byte[sizeof(short)]));

        public override uint ReadUInt32() => BinaryPrimitives.ReadUInt32BigEndian(ReadBigEndian(stackalloc byte[sizeof(uint)]));
        public override int ReadInt32() => BinaryPrimitives.ReadInt32BigEndian(ReadBigEndian(stackalloc byte[sizeof(int)]));

        public override long ReadInt64() => BinaryPrimitives.ReadInt64BigEndian(ReadBigEndian(stackalloc byte[sizeof(long)]));
        public override ulong ReadUInt64() => BinaryPrimitives.ReadUInt64BigEndian(ReadBigEndian(stackalloc byte[sizeof(ulong)]));

        public override float ReadSingle() => BinaryPrimitives.ReadSingleBigEndian(ReadBigEndian(stackalloc byte[sizeof(float)]));
        public override double ReadDouble() => BinaryPrimitives.ReadDoubleBigEndian(ReadBigEndian(stackalloc byte[sizeof(double)]));
        //元実装と互換: decimalだがInt32相当(4バイト)をビッグエンディアンで読む
        public override decimal ReadDecimal() => BinaryPrimitives.ReadInt32BigEndian(ReadBigEndian(stackalloc byte[sizeof(int)]));

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


        //ストリームからsizeバイトをspanへ読み込んで返す。
        //BinaryPrimitives.Read*BigEndianがビッグエンディアンとして解釈するため、ここでは反転しない。
        ReadOnlySpan<byte> ReadBigEndian(Span<byte> buffer)
        {
            var stream = BaseStream;
            int read = 0;
            while (read < buffer.Length)
            {
                int n = stream.Read(buffer[read..]);
                if (n <= 0)
                    throw new EndOfStreamException();
                read += n;
            }
            return buffer;
        }
    }
}
