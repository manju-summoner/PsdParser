using System.Collections.Immutable;

namespace PsdParser
{
    public class LayerInfo
    {
        public long Length { get; }
        public short LayerCount { get; }
        public ImmutableList<LayerRecord> LayerRecords { get; }
        public ImmutableList<ChannelImageData> ChannelImageDatas { get; }

        internal LayerInfo(PsdBinaryReader reader, bool isPSB, int depth)
        {
            var position = reader.BaseStream.Position;

            Length = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
            LayerCount = reader.ReadInt16();

            var layerRecords = new List<LayerRecord>();
            for (int i = 0; i < Math.Abs(LayerCount); i++)//LayerCountは負の値を取ることがある。
                layerRecords.Add(new LayerRecord(reader, isPSB));
            LayerRecords = layerRecords.ToImmutableList();

            var channelImageDatas = new List<ChannelImageData>();
            for (int i = 0; i < Math.Abs(LayerCount); i++) 
            {
                var layer = LayerRecords[i];
                var width = layer.Right - layer.Left;
                var height = layer.Bottom - layer.Top;
                for(int c = 0;c< layer.Channels;c++)
                    channelImageDatas.Add(new ChannelImageData(reader, isPSB, width, height, depth));
            }
            ChannelImageDatas = channelImageDatas.ToImmutableList();

            if (position + 4 + Length - reader.BaseStream.Position < 4)
                reader.BaseStream.Position = position + 4 + Length;

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + Length);
        }
    }
}