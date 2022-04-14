﻿using System.Collections.Immutable;

namespace PsdParser
{
    public class LayerAndMaskInformationSection
    {
        public long Length { get; }
        public LayerInfo? LayerInfo { get; }
        public GlobalLayerMaskInfo? GlobalLayerMaskInfo { get; }
        public ImmutableList<AdditionalLayerInformation> AdditionalLayerInformations { get; } = ImmutableList.Create<AdditionalLayerInformation>();

        internal LayerAndMaskInformationSection(PsdBinaryReader reader,bool isPSB, int depth)
        {
            var position = reader.BaseStream.Position;

            Length = isPSB ? reader.ReadInt64() : reader.ReadUInt32();
            if (Length > 0)
            {
                LayerInfo = new LayerInfo(reader, isPSB, depth);
                GlobalLayerMaskInfo = new GlobalLayerMaskInfo(reader);

                var additionalInfos = new List<AdditionalLayerInformation>();
                while(position + 4 + Length < reader.BaseStream.Position)
                    additionalInfos.Add(AdditionalLayerInformation.Parse(reader, isPSB));
                AdditionalLayerInformations = additionalInfos.ToImmutableList();
            }
            InvalidStreamPositionException.ThrowIfInvalid(reader, position, 4 + Length);
        }
    }

}