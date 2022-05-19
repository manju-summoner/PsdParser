namespace PsdParser
{
    public class LayerAndMaskInformationSection
    {
        public long Length { get; }
        public LayerInfo LayerInfo { get; }
        public GlobalLayerMaskInfo GlobalLayerMaskInfo { get; }
        public AdditionalLayerInformation[] AdditionalLayerInformations { get; }

        internal LayerAndMaskInformationSection(PsdBinaryReader reader, bool isPSB, int depth, ColorMode colorMode)
        {
            var position = reader.BaseStream.Position;

            Length = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
            var lengthSize = isPSB ? 8 : 4;
            if (Length > 0)
            {
                LayerInfo = new LayerInfo(reader, isPSB, depth, colorMode);

                if (reader.BaseStream.Position == position + 4 + Length)
                {
                    GlobalLayerMaskInfo = new GlobalLayerMaskInfo();
                    AdditionalLayerInformations = Array.Empty<AdditionalLayerInformation>();
                }
                else
                {
                    GlobalLayerMaskInfo = new GlobalLayerMaskInfo(reader);

                    var maxPadding = AdditionalLayerInformation.MinSize(isPSB);
                    var additionalInfos = new List<AdditionalLayerInformation>();
                    while (maxPadding <= position + lengthSize + Length - reader.BaseStream.Position)
                        additionalInfos.Add(AdditionalLayerInformation.Parse(reader, isPSB));
                    AdditionalLayerInformations = additionalInfos.ToArray();
                    if (position + lengthSize + Length - reader.BaseStream.Position < maxPadding)
                        reader.BaseStream.Position = position + lengthSize + Length;
                }
            }
            else
            {
                LayerInfo = new LayerInfo();
                GlobalLayerMaskInfo = new GlobalLayerMaskInfo();
                AdditionalLayerInformations = Array.Empty<AdditionalLayerInformation>();
            }
            InvalidStreamPositionException.ThrowIfInvalid(reader, position, lengthSize + Length);
        }
    }

}