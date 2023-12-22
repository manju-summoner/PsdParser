namespace PsdParser
{
    public class ImageResourceSection
    {
        public uint Length { get; }
        public ImageResourceBlock[] Blocks { get; }

        internal ImageResourceSection(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;
            Length = reader.ReadUInt32();

            var blocks = new List<ImageResourceBlock>();
            while (reader.BaseStream.Position - position < Length)
            {
                blocks.Add(ImageResourceBlock.Parse(reader));
            }
            Blocks = blocks.ToArray();


            InvalidStreamPositionException.ThrowIfInvalid(reader, position, Length + 4);
        }
    }
}