namespace PsdParser.ImageResourceBlocks
{
    public class LayerGroupsEnabledId : ImageResourceBlock
    {
        public byte[] GroupIds { get; private set; } = [];
        internal LayerGroupsEnabledId(PsdBinaryReader reader, ImageResourceBlockId id, string name, uint dataSize) : base(reader, id, name, dataSize)
        {
        }

        private protected override void Load(PsdBinaryReader reader, uint dataSize)
        {
            var count = dataSize / sizeof(byte);
            var groupIds = new byte[count];
            for (var i = 0; i < count; i++)
            {
                groupIds[i] = reader.ReadByte();
            }
            GroupIds = groupIds;
        }
    }
}