using System.Collections.Immutable;

namespace PsdParser
{
    public class ImageResourceSection
    {
        public uint Length { get; }
        public ImmutableList<ImageResourceBlock> Blocks { get; }

        internal ImageResourceSection(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;
            Length = reader.ReadUInt32();

            var blocks = new List<ImageResourceBlock>();
            while (reader.BaseStream.Position - position < Length)
            {
                blocks.Add(new ImageResourceBlock(reader));
            }
            Blocks = blocks.ToImmutableList();


            InvalidStreamPositionException.ThrowIfInvalid(reader, position, Length + 4);
        }
    }
}