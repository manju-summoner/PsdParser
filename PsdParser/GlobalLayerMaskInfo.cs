namespace PsdParser
{
    public class GlobalLayerMaskInfo
    {
        public uint Length { get; }
        public ushort OverlayColorSpace { get; }
        public ulong ColorComponent { get; }
        public ushort Opacity { get; }
        public MaskKind Kind { get; }
        internal GlobalLayerMaskInfo(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            Length = reader.ReadUInt32();
            try
            {
                if (Length is 0)
                    return;
                OverlayColorSpace = reader.ReadUInt16();
                ColorComponent = reader.ReadUInt64();
                Opacity = reader.ReadUInt16();
                Kind = (MaskKind)reader.ReadByte();

                reader.BaseStream.Position = position + 4 + Length;
            }
            finally
            {
                InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + Length);
            }
        }
        internal GlobalLayerMaskInfo()
        {
            Length = 0;
        }
    }
}