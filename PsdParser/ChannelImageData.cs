namespace PsdParser
{
    public class ChannelImageData
    {
        public ChannelImageDataCompression Compression { get; }
        internal ChannelImageData(PsdBinaryReader reader, bool isPSB, uint width, uint height, int depth)
        {
            Compression = (ChannelImageDataCompression)reader.ReadUInt16();

            long size;
            if (Compression is ChannelImageDataCompression.RawData)
            {
                size = width * height * depth / 8;
                reader.BaseStream.Position += size;
            }
            else if(Compression is ChannelImageDataCompression.RLE)
            {
                var lineLengthes = new uint[height];
                for (int i = 0; i < height; i++)
                    lineLengthes[i] = isPSB ? reader.ReadUInt32() : reader.ReadUInt16();

                for(int i = 0; i < height; i++)
                {
                    reader.BaseStream.Position += lineLengthes[i];
                }
            }
            else
            {
                throw new NotSupportedException($"Not supported compression: {Compression}");
            }
        }
    }
}