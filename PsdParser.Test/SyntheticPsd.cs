using System;
using System.Collections.Generic;
using System.IO;

namespace PsdParser.Test
{
    /// <summary>
    /// テスト/ベンチマーク用に、有効なPSD(v1, 8bit, RGB, RLE圧縮)をメモリ上で合成生成するヘルパー。
    /// 実ファイルをコミットせずに、背の高いRLEレイヤーを多数含む大型PSDを再現性をもって生成できる。
    /// 生成内容は完全に決定的なので、リグレッションテストのゴールデン値も安定する。
    /// </summary>
    public static class SyntheticPsd
    {
        /// <summary>
        /// 指定サイズ・レイヤー数のPSD(v1, RGB, 8bit, 全チャンネルRLE圧縮)のバイト列を生成する。
        /// </summary>
        public static byte[] CreateRgb(int width, int height, int layerCount)
        {
            using var ms = new MemoryStream();

            // ---- File Header (26 bytes) ----
            WriteAscii(ms, "8BPS");
            WriteU16(ms, 1);              // version 1 (PSD)
            ms.Write(new byte[6], 0, 6);  // reserved
            WriteU16(ms, 4);              // channels (composite)
            WriteI32(ms, height);
            WriteI32(ms, width);
            WriteU16(ms, 8);              // depth
            WriteU16(ms, 3);              // color mode = RGB

            // ---- Color Mode Data Section ----
            WriteU32(ms, 0);

            // ---- Image Resources Section ----
            WriteU32(ms, 0);

            // ---- Layer And Mask Information Section ----
            // レイヤー情報の本体(layerCount + records + channel data)を先に組み立ててから長さを確定する。
            var layerInfoBody = BuildLayerInfoBody(width, height, layerCount);
            // L_li = layerInfoBody.Length (偶数に揃える)
            var liBody = layerInfoBody;
            if ((liBody.Length & 1) == 1)
            {
                var padded = new byte[liBody.Length + 1];
                Array.Copy(liBody, padded, liBody.Length);
                liBody = padded;
            }

            // LASection Length = 4(layerInfoLength field) + L_li
            long laLength = 4L + liBody.Length;
            WriteU32(ms, (uint)laLength);
            WriteU32(ms, (uint)liBody.Length); // layer info length
            ms.Write(liBody, 0, liBody.Length);

            return ms.ToArray();
        }

        static byte[] BuildLayerInfoBody(int width, int height, int layerCount)
        {
            using var ms = new MemoryStream();
            WriteI16(ms, (short)layerCount); // layer count (正: 透過データ無し扱い)

            // 各レイヤーのチャンネルデータを先に生成し、ChannelDataLengthを確定させる。
            var channelDataPerLayer = new List<byte[][]>();
            // channel順: R(0), G(1), B(2), A(-1)
            short[] channelIds = [0, 1, 2, -1];

            for (int l = 0; l < layerCount; l++)
            {
                var perChannel = new byte[channelIds.Length][];
                for (int ci = 0; ci < channelIds.Length; ci++)
                    perChannel[ci] = BuildChannelData(width, height, l, channelIds[ci]);
                channelDataPerLayer.Add(perChannel);
            }

            // Layer Records
            for (int l = 0; l < layerCount; l++)
            {
                var perChannel = channelDataPerLayer[l];

                WriteI32(ms, 0);        // Top
                WriteI32(ms, 0);        // Left
                WriteI32(ms, height);   // Bottom
                WriteI32(ms, width);    // Right

                WriteU16(ms, (ushort)channelIds.Length); // Channels
                for (int ci = 0; ci < channelIds.Length; ci++)
                {
                    WriteI16(ms, channelIds[ci]);
                    WriteU32(ms, (uint)perChannel[ci].Length); // channel data length
                }

                WriteAscii(ms, "8BIM");
                WriteAscii(ms, "norm");  // blend mode
                ms.WriteByte(255);       // opacity
                ms.WriteByte(0);         // clipping
                ms.WriteByte(0);         // flags
                ms.WriteByte(0);         // filler

                // Extra data: LayerMask(0) + BlendingRanges(0) + PascalName(aligned 4)
                var name = $"Layer{l}";
                var nameBytes = BuildPascalString(name, 4);
                uint extraLen = (uint)(4 + 4 + nameBytes.Length);
                WriteU32(ms, extraLen);
                WriteU32(ms, 0); // layer mask size
                WriteU32(ms, 0); // blending ranges length
                ms.Write(nameBytes, 0, nameBytes.Length);
            }

            // Channel Image Data (records順と同じ順序で)
            for (int l = 0; l < layerCount; l++)
            {
                var perChannel = channelDataPerLayer[l];
                for (int ci = 0; ci < perChannel.Length; ci++)
                    ms.Write(perChannel[ci], 0, perChannel[ci].Length);
            }

            return ms.ToArray();
        }

        /// <summary>1チャンネル分の画像データ(compression u16 + RLE index table + packbits data)を生成。</summary>
        static byte[] BuildChannelData(int width, int height, int layer, short channelId)
        {
            using var ms = new MemoryStream();
            WriteU16(ms, 1); // RLE

            var line = new byte[width];
            var packedLines = new byte[height][];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    line[x] = SamplePixel(x, y, layer, channelId, width);
                packedLines[y] = PackBits(line);
            }

            // index table (i16 per line)
            for (int y = 0; y < height; y++)
                WriteI16(ms, (short)packedLines[y].Length);
            // packbits data
            for (int y = 0; y < height; y++)
                ms.Write(packedLines[y], 0, packedLines[y].Length);

            return ms.ToArray();
        }

        /// <summary>決定的なピクセル値生成。横方向に16pxの連長を作りRLEが効くようにする。</summary>
        static byte SamplePixel(int x, int y, int layer, short channelId, int width)
        {
            if (channelId == -1)
            {
                // alpha: 端を透過、中央を不透明に。横ランあり。
                int edge = width / 8;
                if (x < edge || x >= width - edge) return 0;
                return 255;
            }
            int baseVal = ((x / 16) + (y / 4) + layer * 17 + channelId * 53);
            return (byte)(baseVal & 0xFF);
        }

        /// <summary>標準的なPackBits(RLE)エンコード。ChannelImageData.ReadLineのデコーダと整合する。</summary>
        static byte[] PackBits(byte[] data)
        {
            var outp = new List<byte>(data.Length / 2 + 16);
            int len = data.Length;
            int i = 0;
            while (i < len)
            {
                byte b = data[i];
                int run = 1;
                while (i + run < len && data[i + run] == b && run < 128) run++;
                if (run >= 2)
                {
                    outp.Add((byte)(sbyte)-(run - 1));
                    outp.Add(b);
                    i += run;
                }
                else
                {
                    int litStart = i;
                    int lit = 0;
                    while (i < len && lit < 128)
                    {
                        // 3連長以上がここから始まるならリテラルを打ち切る
                        if (i + 2 < len && data[i] == data[i + 1] && data[i + 1] == data[i + 2]) break;
                        i++; lit++;
                    }
                    outp.Add((byte)(sbyte)(lit - 1));
                    for (int k = 0; k < lit; k++) outp.Add(data[litStart + k]);
                }
            }
            return outp.ToArray();
        }

        static byte[] BuildPascalString(string s, int align)
        {
            // length byte + bytes、(1+len)をalignの倍数にパディング
            var raw = System.Text.Encoding.ASCII.GetBytes(s);
            int total = 1 + raw.Length;
            int pad = align - total % align;
            if (pad == align) pad = 0;
            var result = new byte[total + pad];
            result[0] = (byte)raw.Length;
            Array.Copy(raw, 0, result, 1, raw.Length);
            return result;
        }

        static void WriteAscii(Stream s, string v)
        {
            foreach (var c in v) s.WriteByte((byte)c);
        }
        static void WriteU16(Stream s, ushort v) { s.WriteByte((byte)(v >> 8)); s.WriteByte((byte)v); }
        static void WriteI16(Stream s, short v) => WriteU16(s, (ushort)v);
        static void WriteU32(Stream s, uint v)
        {
            s.WriteByte((byte)(v >> 24)); s.WriteByte((byte)(v >> 16));
            s.WriteByte((byte)(v >> 8)); s.WriteByte((byte)v);
        }
        static void WriteI32(Stream s, int v) => WriteU32(s, (uint)v);
    }
}
