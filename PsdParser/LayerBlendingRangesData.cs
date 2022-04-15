namespace PsdParser
{
    public class LayerBlendingRangesData
    {
        public ChannelRange[] ChannelRanges { get; }
        internal LayerBlendingRangesData(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            var length = reader.ReadUInt32();
            ChannelRanges = new ChannelRange[length/8];
            for(int i = 0; i < length / 8; i++)
            {
                ChannelRanges[i] = new ChannelRange(reader);
            }

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, length + 4);
        }
    }
}
