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
        public unsafe void ReadLine(byte[] buffer, int y)
        {
            if(Compression is ChannelImageDataCompression.RawData)
            {
                int rowBytes = width * depth / 8;
                long src = position + (long)rowBytes * y;
                var mem = reader.UnderlyingBuffer;
                if (mem is not null)
                {
                    Array.Copy(mem, (int)src, buffer, 0, rowBytes);
                }
                else
                {
                    reader.BaseStream.Position = src;
                    reader.Read(buffer, 0, rowBytes);
                }
            }
            else if(Compression is ChannelImageDataCompression.RLE)
            {
                int len = rleIndex[y];
                long src = position + rleOffsets[y];
                var mem = reader.UnderlyingBuffer;
                byte[] rle;
                int rleStart;
                if (mem is not null)
                {
                    //メモリ上のファイルバッファから直接デコード(スクラッチへのコピー不要)
                    rle = mem;
                    rleStart = (int)src;
                }
                else
                {
                    reader.BaseStream.Position = src;
                    rle = rleScratch;
                    rleStart = 0;
                    for (int read = 0; read < len;)
                    {
                        int n = reader.Read(rle, read, len - read);
                        if (n <= 0) break;
                        read += n;
                    }
                }

                int di = 0;
                int siEnd = rleStart + len;
                int bufLen = buffer.Length;
                fixed (byte* pSrc = rle, pDst = buffer)
                {
                    int si = rleStart;
                    while (si < siEnd)
                    {
                        var count = (sbyte)pSrc[si];
                        if (count >= 0)
                        {
                            //リテラル: count+1 バイトをそのままコピー
                            int n = count + 1;
                            if (di + n > bufLen) break; //破損データ保護
                            Buffer.MemoryCopy(pSrc + si + 1, pDst + di, bufLen - di, n);
                            di += n;
                            si += n + 1;
                        }
                        else
                        {
                            //連長: 次の1バイトを -count+1 回繰り返す
                            int n = -count + 1;
                            if (di + n > bufLen) break; //破損データ保護
                            byte v = pSrc[si + 1];
                            new Span<byte>(pDst + di, n).Fill(v);
                            di += n;
                            si += 2;
                        }
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