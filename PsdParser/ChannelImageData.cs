namespace PsdParser
{
    public class ChannelImageData
    {
        readonly long position;
        readonly PsdBinaryReader reader;
        readonly int width, depth;
        readonly int[] rleIndex = [];
        //各スキャンラインのRLEデータ先頭オフセット(positionからの相対バイト数)を事前計算したもの。
        //ReadLineで毎回rleIndex.Take(y).Sum()するとO(height^2)になるため累積和をここに持つ。
        readonly long[] rleOffsets = [];
        //RLE生バイトの読み込み用スクラッチ。行ごとにReadBytesで確保し直さず使い回す。
        readonly byte[] rleScratch = [];

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
                rleOffsets = new long[height];
                long offset = 0;
                int maxLine = 0;
                for (int i = 0; i < height; i++)
                {
                    rleOffsets[i] = offset;
                    offset += rleIndex[i];
                    if (rleIndex[i] > maxLine) maxLine = rleIndex[i];
                }
                rleScratch = new byte[maxLine];
                reader.BaseStream.Position += offset;
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
                reader.BaseStream.Position = position + rleOffsets[y];
                int len = rleIndex[y];
                var rle = rleScratch;
                for (int read = 0; read < len;)
                {
                    int n = reader.Read(rle, read, len - read);
                    if (n <= 0) break;
                    read += n;
                }
                int di = 0;
                for (var si = 0; si < len; )
                {
                    var count = (sbyte)rle[si];
                    if (count >= 0)
                    {
                        //リテラル: count+1 バイトをそのままコピー
                        int n = count + 1;
                        Array.Copy(rle, si + 1, buffer, di, n);
                        di += n;
                        si += n + 1;
                    }
                    else
                    {
                        //連長: 次の1バイトを -count+1 回繰り返す
                        int n = -count + 1;
                        Array.Fill(buffer, rle[si + 1], di, n);
                        di += n;
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