using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser.ImageResourceBlocks
{
    public class LayersGroupInformation : ImageResourceBlock
    {
        public short[] GroupIds { get; private set; } = [];
        internal LayersGroupInformation(PsdBinaryReader reader, ImageResourceBlockId id, string name, uint dataSize) : base(reader, id, name, dataSize)
        {

        }

        private protected override void Load(PsdBinaryReader reader, uint dataSize)
        {
            var count = dataSize / sizeof(short);
            var groupIds = new short[count];
            for (var i = 0; i < count; i++)
            {
                groupIds[i] = reader.ReadInt16();
            }
            GroupIds = groupIds;
        }
    }
}
