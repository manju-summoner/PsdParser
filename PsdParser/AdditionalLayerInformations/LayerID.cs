using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser.AdditionalLayerInformations
{
    public class LayerID : AdditionalLayerInformation
    {
        public int Id { get; private set; }
        internal LayerID(PsdBinaryReader reader, long length) : base(reader, AdditionalLayerInformationKeys.LayerID, length)
        {

        }

        private protected override void Load(PsdBinaryReader reader, long length)
        {
            Id = reader.ReadInt32();
        }
    }
}
