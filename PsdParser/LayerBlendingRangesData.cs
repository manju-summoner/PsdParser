using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser
{
    public class LayerBlendingRangesData
    {
        public ImmutableList<ChannelRange> ChannelRanges { get; }
        internal LayerBlendingRangesData(PsdBinaryReader reader)
        {
            var position = reader.BaseStream.Position;

            var length = reader.ReadUInt32();
            var ranges = new List<ChannelRange>();
            for(int i = 0; i < length / 8; i++)
            {
                ranges.Add(new ChannelRange(reader));
            }
            ChannelRanges = ranges.ToImmutableList();

            InvalidStreamPositionException.ThrowIfInvalid(reader, position, length + 4);
        }
    }
    public class ChannelRange
    {
        public uint SourceRange { get; }
        public uint DestinationRange { get; }

        internal ChannelRange(PsdBinaryReader reader)
        {
            SourceRange = reader.ReadUInt32();
            DestinationRange = reader.ReadUInt32();
        }
    }
}
