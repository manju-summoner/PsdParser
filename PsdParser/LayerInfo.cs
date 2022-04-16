namespace PsdParser
{
    public class LayerInfo
    {
        public long Length { get; }
        public short LayerCount { get; }
        public LayerRecord[] LayerRecords { get; }
        public LayerImage[] LayerImages { get; }

        internal LayerInfo(PsdBinaryReader reader, bool isPSB, int depth, ColorMode colorMode)
        {
            var position = reader.BaseStream.Position;
            Length = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
            var lengthSize = isPSB ? 8 : 4;
            LayerCount = reader.ReadInt16();

            
            LayerRecords = new LayerRecord[Math.Abs(LayerCount)];//LayerCountは負の値を取ることがある。
            for (int i = 0; i < LayerRecords.Length; i++)
                LayerRecords[i] = new LayerRecord(reader, isPSB);

            LayerImages = new LayerImage[LayerRecords.Length];
            for (int i = 0; i < Math.Abs(LayerCount); i++)
                LayerImages[i] = new LayerImage(reader, LayerRecords[i], isPSB, depth, colorMode);

            if (position + lengthSize + Length - reader.BaseStream.Position < 4)
                reader.BaseStream.Position = position + lengthSize + Length;

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, lengthSize + Length);
        }
    }
}