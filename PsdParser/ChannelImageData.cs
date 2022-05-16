namespace PsdParser
{
    public class ChannelImageData
    {
        readonly long position;
        readonly PsdBinaryReader reader;
        readonly int width, depth;
        readonly int[] rleIndex = Array.Empty<int>();

        public ChannelImageDataCompression Compression { get; }
        public ChannelId ChannelId { get; }
        internal ChannelImageData(PsdBinaryReader reader,ChannelId channelId, bool isPSB, int width, int height, int depth)
        {
            this.reader = reader;
            this.width = width;
            this.depth = depth;
            Compression = (ChannelImageDataCompression)reader.ReadUInt16();
            ChannelId = channelId;

            long size;
            if (Compression is ChannelImageDataCompression.RawData)
            {
                position = reader.BaseStream.Position;
                size = width * height * depth / 8;
                reader.BaseStream.Position += size;
            }
            else if(Compression is ChannelImageDataCompression.RLE)
            {
                rleIndex = new int[height];
                for (int i = 0; i < height; i++)
                    rleIndex[i] = isPSB ? reader.ReadInt32() : reader.ReadInt16();

                position = reader.BaseStream.Position;
                for (int i = 0; i < height; i++)
                    reader.BaseStream.Position += rleIndex[i];
            }
            else
            {
                throw new NotSupportedException($"Not supported compression: {Compression}");
            }
        }
        public void ReadLine(byte[] buffer, int y)
        {
            if(Compression is ChannelImageDataCompression.RawData)
            {
                reader.BaseStream.Position = position + width * depth / 8 * y;
                reader.Read(buffer, 0, width * depth / 8);
            }
            else if(Compression is ChannelImageDataCompression.RLE)
            {
                reader.BaseStream.Position = position + rleIndex.Take(y).Sum();
                var rle = reader.ReadBytes(rleIndex[y]);
                int di = 0;
                for (var si = 0; si < rle.Length; )
                {
                    var count = (sbyte)rle[si];
                    if (count > 0)
                    {
                        for (int i = 0; i < count + 1; i++)
                            buffer[di + i] = rle[si + 1 + i];
                        di += count + 1;
                        si += count + 1 + 1;
                    }
                    else
                    {
                        for (int i = 0; i < -count + 1; i++)
                            buffer[di + i] = rle[si + 1];
                        di += -count + 1;
                        si += 2;
                    }
                }
                if (di != buffer.Length)
                    throw new InvalidStreamPositionException(di, 0, buffer.Length);
            }
            else
            {
                throw new NotSupportedException($"Not supported compression: {Compression}");
            }
        }
    }
}