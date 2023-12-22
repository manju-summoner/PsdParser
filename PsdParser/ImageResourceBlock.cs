using PsdParser.ImageResourceBlocks;

namespace PsdParser
{
    public class ImageResourceBlock
    {
        public ImageResourceBlockId Id { get; }
        public string Name { get; }
        public uint DataSize { get; }
        public long Position { get; }

        private protected ImageResourceBlock(PsdBinaryReader reader, ImageResourceBlockId id, string name, uint dataSize)
        {
            Id = id;
            Name = name;
            DataSize = dataSize;
            Position = reader.BaseStream.Position;
            Load(reader, dataSize);
        }

        private protected virtual void Load(PsdBinaryReader reader, uint dataSize)
        {
            reader.BaseStream.Position += dataSize;
        }

        internal static ImageResourceBlock Parse(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            var signature = new string(reader.ReadChars(4));
            if (signature != "8BIM")
                throw new InvalidSignatureException(signature);

            var key = (ImageResourceBlockId)reader.ReadUInt16();
            var name = reader.ReadPascalString(2, out var nameSize);
            var dataSize = reader.ReadUInt32();

            var block = key switch
            {
                ImageResourceBlockId.LayersGroupInformation => new LayersGroupInformation(reader, key, name, dataSize),
                ImageResourceBlockId.LayerGroupsEnabledId => new LayerGroupsEnabledId(reader, key, name, dataSize),
                _ => new ImageResourceBlock(reader, key, name, dataSize),
            };
            reader.BaseStream.Position += dataSize % 2;

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + 2 + nameSize + 4 + dataSize + dataSize % 2);
            return block;
        }
    }
}