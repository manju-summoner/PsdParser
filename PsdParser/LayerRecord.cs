﻿using System.Collections.Immutable;

namespace PsdParser
{
    public class LayerRecord
    {
        public uint Top { get; }
        public uint Left { get; }
        public uint Bottom { get; }
        public uint Right { get; }

        public ushort Channels { get; }
        public ImmutableList<ChannelInformation> ChannelInfos { get; }

        public string BlendMode { get; }
        public byte Opacity { get; }
        public bool Clipping { get; }
        public LayerFlags LayerFlags { get; }
        public uint ExtraDataLength { get; }
        public LayerMaskAndAdjustmentLayerData LayerMaskAndAdjustmentLayerData { get; }
        public LayerBlendingRangesData LayerBlendingRangesData { get; }
        public string LayerName { get; }

        public ImmutableList<AdditionalLayerInformation> AdditionalLayerInformations { get; }

        internal LayerRecord(PsdBinaryReader reader, bool isPSB)
        {
            Top = reader.ReadUInt32();
            Left = reader.ReadUInt32();
            Bottom = reader.ReadUInt32();
            Right = reader.ReadUInt32();

            Channels = reader.ReadUInt16();
            var infos = new List<ChannelInformation>();
            for (int i = 0; i < Channels; i++)
                infos.Add(new ChannelInformation(reader, isPSB));
            ChannelInfos = infos.ToImmutableList();

            var signature = new string(reader.ReadChars(4));
            if (signature != "8BIM")
                throw new InvalidSignatureException(signature);

            BlendMode = new string(reader.ReadChars(4));
            Opacity = reader.ReadByte();
            Clipping = reader.ReadByte() is 1;
            LayerFlags = (LayerFlags)reader.ReadByte();
            reader.BaseStream.Position += 1;

            ExtraDataLength = reader.ReadUInt32();
            var extraDataPosition = reader.BaseStream.Position;

            LayerMaskAndAdjustmentLayerData = new LayerMaskAndAdjustmentLayerData(reader);
            LayerBlendingRangesData = new LayerBlendingRangesData(reader);
            LayerName = reader.ReadPascalString(4);

            var additional = new List<AdditionalLayerInformation>();
            while(reader.BaseStream.Position < extraDataPosition + ExtraDataLength)
                additional.Add(AdditionalLayerInformation.Parse(reader, isPSB));
            AdditionalLayerInformations = additional.ToImmutableList();

            InvalidStreamPositionException.ThrowIfInvalid(reader,extraDataPosition,ExtraDataLength);
        }
    }
}