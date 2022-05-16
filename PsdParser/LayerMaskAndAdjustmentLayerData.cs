namespace PsdParser
{
    public class LayerMaskAndAdjustmentLayerData
    {
        public int Size { get; }

        public int Top { get; }
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }

        public byte DefaultColor { get; }
        public LayerMaskAndAdjustmentLayerDataFlags Flags { get; }
        public byte MaskParameters { get; }
        public byte MaskDensity { get; }
        public double MaskFeather { get; }
        public LayerMaskAndAdjustmentLayerDataFlags RealFlags { get; }
        public byte RealUserMaskBackground { get; }
        public int MaskTop { get; }
        public int MaskLeft { get; }
        public int MaskBottom { get; }
        public int MaskRight { get; }
        internal LayerMaskAndAdjustmentLayerData(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;
            Size = reader.ReadInt32();
            try
            {
                if (Size is 0)
                    return;
                
                Top = reader.ReadInt32();
                Left = reader.ReadInt32();
                Bottom = reader.ReadInt32();
                Right = reader.ReadInt32();

                DefaultColor = reader.ReadByte();
                Flags = (LayerMaskAndAdjustmentLayerDataFlags)reader.ReadByte();
                if (Flags.HasFlag(LayerMaskAndAdjustmentLayerDataFlags.IndicatesThatTheUserAndOrVectorMasksHaveParametersAppliedToThem))
                    MaskParameters = reader.ReadByte();
                if (MaskParameters is 1 or 4)
                    MaskDensity = reader.ReadByte();
                else if(MaskParameters is 2 or 8)
                    MaskFeather = reader.ReadDouble();

                if (Size is 20)
                {
                    reader.BaseStream.Position += 2;
                    return;
                }
                RealFlags = (LayerMaskAndAdjustmentLayerDataFlags)reader.ReadByte();
                MaskTop = reader.ReadInt32();
                MaskLeft = reader.ReadInt32();
                MaskBottom = reader.ReadInt32();
                MaskRight = reader.ReadInt32();
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