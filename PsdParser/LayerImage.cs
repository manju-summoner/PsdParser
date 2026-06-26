namespace PsdParser
{
    public class LayerImage
    {
        public ChannelImageData[] ChannelImages { get; }

        public int Width { get; }
        public int Height { get; }
        readonly ColorMode colorMode;
        internal LayerImage(PsdBinaryReader reader, LayerRecord layer, bool isPSB, int depth, ColorMode colorMode)
        {
            Width = layer.Right - layer.Left;
            Height = layer.Bottom - layer.Top;
            this.colorMode = colorMode;
            
            ChannelImages = new ChannelImageData[layer.Channels];
            for (int c = 0; c < layer.Channels; c++)
            {
                int channelImageWidth, channelImageHeight;
                if(ChannelId.TransparencyMask <= layer.ChannelInfos[c].Id)
                {
                    channelImageWidth = Width;
                    channelImageHeight = Height;
                }
                else
                {
                    channelImageWidth = layer.LayerMaskAndAdjustmentLayerData.Right - layer.LayerMaskAndAdjustmentLayerData.Left;
                    channelImageHeight = layer.LayerMaskAndAdjustmentLayerData.Bottom - layer.LayerMaskAndAdjustmentLayerData.Top;
                }
                ChannelImages[c] = new ChannelImageData(reader, layer.ChannelInfos[c].Id, isPSB, channelImageWidth, channelImageHeight, depth);
            }
        }

        public byte[] Read()
        {
            if (colorMode is ColorMode.RGB)
                return ReadRGB();
            else if (colorMode is ColorMode.CMYK)
                return ReadCMYK();
            else
                throw new NotSupportedException($"Not supported format: {colorMode}");
        }

        unsafe byte[] ReadRGB()
        {
            byte[] buffer = new byte[Width * Height * 4];

            var b = new byte[Width];
            var g = new byte[Width];
            var r = new byte[Width];
            var a = new byte[Width];
            Array.Fill(a, byte.MaxValue);

            //y行ループの内側で毎回ChannelIdのswitchをしないよう、対応するチャンネルだけ事前に抽出する。
            var planes = BuildPlanes(r, g, b, a);

            fixed (byte* pBuf = buffer, pB = b, pG = g, pR = r, pA = a)
            {
                for (int y = 0; y < Height; y++)
                {
                    foreach (var (image, plane) in planes)
                        image.ReadLine(plane, y);

                    byte* dst = pBuf + (long)y * Width * 4;
                    for (int x = 0; x < Width; x++)
                    {
                        dst[0] = pB[x];
                        dst[1] = pG[x];
                        dst[2] = pR[x];
                        dst[3] = pA[x];
                        dst += 4;
                    }
                }
            }
            return buffer;
        }

        List<(ChannelImageData image, byte[] plane)> BuildPlanes(byte[] r, byte[] g, byte[] b, byte[] a)
        {
            var planes = new List<(ChannelImageData, byte[])>(ChannelImages.Length);
            foreach (var image in ChannelImages)
            {
                var plane = image.ChannelId switch { ChannelId.R => r, ChannelId.G => g, ChannelId.B => b, ChannelId.TransparencyMask => a, _ => null };
                if (plane is not null)
                    planes.Add((image, plane));
            }
            return planes;
        }

        unsafe byte[] ReadCMYK()
        {
            byte[] buffer = new byte[Width * Height * 4];
            var c = new byte[Width];
            var m = new byte[Width];
            var y = new byte[Width];
            var k = new byte[Width];
            var a = new byte[Width];
            Array.Fill(a, byte.MaxValue);

            var planes = new List<(ChannelImageData image, byte[] plane)>(ChannelImages.Length);
            foreach (var image in ChannelImages)
            {
                var plane = image.ChannelId switch { ChannelId.C => c, ChannelId.M => m, ChannelId.Y => y, ChannelId.K => k, ChannelId.TransparencyMask => a, _ => null };
                if (plane is not null)
                    planes.Add((image, plane));
            }

            fixed (byte* pBuf = buffer, pC = c, pM = m, pY = y, pK = k, pA = a)
            {
                for (int yi = 0; yi < Height; yi++)
                {
                    foreach (var (image, plane) in planes)
                        image.ReadLine(plane, yi);

                    byte* dst = pBuf + (long)yi * Width * 4;
                    for (int x = 0; x < Width; x++)
                    {
                        dst[0] = (byte)(pY[x] * pK[x] / 255);
                        dst[1] = (byte)(pM[x] * pK[x] / 255);
                        dst[2] = (byte)(pC[x] * pK[x] / 255);
                        dst[3] = pA[x];
                        dst += 4;
                    }
                }
            }
            return buffer;
        }
    }
}