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

        byte[] ReadRGB()
        {
            byte[] buffer = new byte[Width * Height * 4];

            var b = new byte[Width];
            var g = new byte[Width];
            var r = new byte[Width];
            var a = new byte[Width];
            Array.Fill(a, byte.MaxValue);

            for(int y = 0; y < Height; y++)
            {
                for(int c = 0;c<ChannelImages.Length;c++)
                {
                    var image = ChannelImages[c];
                    var channelBuffer = image.ChannelId switch { ChannelId.R => r, ChannelId.G => g, ChannelId.B => b, ChannelId.TransparencyMask => a, _ => null };
                    if (channelBuffer is null) 
                        continue;

                    ChannelImages[c].ReadLine(channelBuffer, y);
                }

                for (int x = 0; x < Width; x++)
                {
                    buffer[y * Width * 4 + x * 4 + 0] = b[x];
                    buffer[y * Width * 4 + x * 4 + 1] = g[x];
                    buffer[y * Width * 4 + x * 4 + 2] = r[x];
                    buffer[y * Width * 4 + x * 4 + 3] = a[x];
                }
            }
            return buffer;
        }
        byte[] ReadCMYK()
        {
            byte[] buffer = new byte[Width * Height * 4];
            var c = new byte[Width];
            var m = new byte[Width];
            var y = new byte[Width];
            var k = new byte[Width];
            var a = new byte[Width];
            Array.Fill(a, byte.MaxValue);

            for (int yi = 0; yi < Height; yi++)
            {
                for (int ci = 0; ci < ChannelImages.Length; ci++)
                {
                    var image = ChannelImages[ci];
                    var channelBuffer = image.ChannelId switch { ChannelId.C => c, ChannelId.M => m, ChannelId.Y => y, ChannelId.K=> k, ChannelId.TransparencyMask => a, _ => null };
                    if (channelBuffer is null)
                        continue;

                    ChannelImages[ci].ReadLine(channelBuffer, yi);
                }

                for (int x = 0; x < Width; x++)
                {
                    buffer[yi * Width * 4 + x * 4 + 0] = (byte)(y[x] * k[x] / 255);
                    buffer[yi * Width * 4 + x * 4 + 1] = (byte)(m[x] * k[x] / 255);
                    buffer[yi * Width * 4 + x * 4 + 2] = (byte)(c[x] * k[x] / 255);
                    buffer[yi * Width * 4 + x * 4 + 3] = a[x];
                }
            }
            return buffer;
        }
    }
}