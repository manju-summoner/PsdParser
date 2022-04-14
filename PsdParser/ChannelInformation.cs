namespace PsdParser
{
    public class ChannelInformation
    {
        public ushort Id { get; }
        public long ChannelDataLength { get; }

        internal ChannelInformation(PsdBinaryReader reader, bool isPSB)
        {
            Id = reader.ReadUInt16();
            ChannelDataLength = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
        }
    }
}