namespace PsdParser
{
    public class ColorModeDataSection
    {
        public uint Length;

        internal ColorModeDataSection(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            Length = reader.ReadUInt32();
            reader.BaseStream.Position += Length;

            InvalidStreamPositionException.ThrowIfInvalid(reader,position, Length + 4);
        }
    }
}