namespace PsdParser
{
    public class ChannelInformation
    {
        public ChannelId Id { get; }
        public long ChannelDataLength { get; }

        internal ChannelInformation(PsdBinaryReader reader, bool isPSB)
        {
            Id = (ChannelId)reader.ReadInt16();
            ChannelDataLength = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
        }
    }
}