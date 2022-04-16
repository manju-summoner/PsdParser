namespace PsdParser
{
    public class FileHeaderSection
    {
        public ushort Version { get; }
        public ushort Channels { get; }
        public int Width { get; }
        public int Height { get; }
        public ushort Depth { get; }
        public ColorMode ColorMode { get; }

        internal FileHeaderSection(PsdBinaryReader reader)
        {
            var sign = new string(reader.ReadChars(4));
            if (sign != "8BPS")
                throw new NotSupportedException($"{sign} is not supported");

            Version = reader.ReadUInt16();
            if (Version != 1 && Version != 2)
                throw new NotSupportedException($"Not supported file version: {Version}");

            reader.BaseStream.Position += 6;

            Channels = reader.ReadUInt16();
            Height = reader.ReadInt32();
            Width = reader.ReadInt32();
            Depth = reader.ReadUInt16();
            ColorMode = (ColorMode)reader.ReadUInt16();

            InvalidStreamPositionException.ThrowIfInvalid(reader, 0, 4 + 2 + 6 + 2 + 4 + 4 + 2 + 2);
        }
    }
}