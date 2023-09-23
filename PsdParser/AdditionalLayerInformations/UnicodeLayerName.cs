using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser.AdditionalLayerInformations
{
    public class UnicodeLayerName : AdditionalLayerInformation
    {
        public string Name { get; private set; } = string.Empty;
        internal UnicodeLayerName(PsdBinaryReader reader,long length):base(reader, AdditionalLayerInformationKeys.UnicodeLayerName, length)
        {

        }

        private protected override void Load(PsdBinaryReader reader, long length)
        {
            Name = reader.ReadUnicodeString();
        }
    }
}
