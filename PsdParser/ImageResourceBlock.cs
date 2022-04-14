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

            DataSize = reader.ReadUInt32();
            reader.BaseStream.Position += DataSize;

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + 2 + 1 + Math.Max(1, Name.Length) + 4 + DataSize);
        }
    }
}