namespace PsdParser
{
    public class ImageResourceBlock
    {
        public ushort Id { get; }
        public string Name { get; }
        public uint DataSize { get; }

        internal ImageResourceBlock(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            var signature = new string(reader.ReadChars(4));
            if (signature != "8BIM")
                throw new InvalidSignatureException(signature);

            Id = reader.ReadUInt16();
            Name = reader.ReadPascalString(2);
            var nameSize = Name.Length + 1;
            nameSize += nameSize % 2;

            DataSize = reader.ReadUInt32();
            var dataPadding = DataSize % 2;
            reader.BaseStream.Position += DataSize + dataPadding;

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + 2 + nameSize + 4 + DataSize + dataPadding);
        }
    }
}