namespace PsdParser
{
    public class LayerMaskAndAdjustmentLayerData
    {
        public uint Size { get; }

        public uint Top { get; }
        public uint Left { get; }
        public uint Bottom { get; }
        public uint Right { get; }

        public byte DefaultColor { get; }
        public LayerMaskAndAdjustmentLayerDataFlags Flags { get; }
        public byte MaskParameters { get; }
        public byte MaskDensity { get; }
        public double MaskFeather { get; }
        public LayerMaskAndAdjustmentLayerDataFlags RealFlags { get; }
        public byte RealUserMaskBackground { get; }
        public uint MaskTop { get; }
        public uint MaskLeft { get; }
        public uint MaskBottom { get; }
        public uint MaskRight { get; }
        internal LayerMaskAndAdjustmentLayerData(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;
            Size = reader.ReadUInt32();
            try
            {
                if (Size is 0)
                    return;
                
                Top = reader.ReadUInt32();
                Left = reader.ReadUInt32();
                Bottom = reader.ReadUInt32();
                Right = reader.ReadUInt32();

                DefaultColor = reader.ReadByte();
                Flags = (LayerMaskAndAdjustmentLayerDataFlags)reader.ReadByte();
                if (Flags.HasFlag(LayerMaskAndAdjustmentLayerDataFlags.IndicatesThatTheUserAndOrVectorMasksHaveParametersAppliedToThem))
                    MaskParameters = reader.ReadByte();
                if (MaskParameters is 1 or 4)
                    MaskDensity = reader.ReadByte();
                else
                    MaskFeather = reader.ReadDouble();

                if (Size is 20)
                {
                    reader.BaseStream.Position += 2;
                    return;
                }
                RealFlags = (LayerMaskAndAdjustmentLayerDataFlags)reader.ReadByte();
                MaskTop = reader.ReadUInt32();
                MaskLeft = reader.ReadUInt32();
                MaskBottom = reader.ReadUInt32();
                MaskRight = reader.ReadUInt32();
            }
            finally
            {
                InvalidStreamPositionException.ThrowIfInvalid(reader, position, Size + 4);
            }
        }
    }
    [Flags]
    public enum LayerMaskAndAdjustmentLayerDataFlags
    {
        PositionRelativeToLayer = 1,
        LayerMaskDisabled = 2,
        InvertLayerMaskWhenBlending = 4,
        IndicatesThatTheUserMaskActuallyCameFromRenderingOtherData  = 8,
        IndicatesThatTheUserAndOrVectorMasksHaveParametersAppliedToThem = 16,
    }
}