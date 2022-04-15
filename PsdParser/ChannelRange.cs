namespace PsdParser
{
    public class ChannelRange
    {
        public uint SourceRange { get; }
        public uint DestinationRange { get; }

        internal ChannelRange(PsdBinaryReader reader)
        {
            SourceRange = reader.ReadUInt32();
            DestinationRange = reader.ReadUInt32();
        }
    }
}
