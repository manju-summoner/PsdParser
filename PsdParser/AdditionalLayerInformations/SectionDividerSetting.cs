using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser.AdditionalLayerInformations
{
    public class SectionDividerSetting : AdditionalLayerInformation
    {
        public LsctType Type { get; private set; }
        public string BlendMode { get; private set; } = string.Empty;
        public LsctSubType SubType { get; private set; }

        internal SectionDividerSetting(PsdBinaryReader reader, long length) : base(reader, "lsct", length)
        {

        }
        private protected override void Load(PsdBinaryReader reader, long length)
        {
            Type = (LsctType)reader.ReadInt32();
            if (length < 12) return;

            var signature = new string(reader.ReadChars(4));
            if(signature != "8BIM")
                throw new InvalidSignatureException(signature);

            BlendMode = new string(reader.ReadChars(4));
            if (length < 16) return;

            SubType = (LsctSubType)reader.ReadInt32();

        }

        public enum LsctType : int
        {
            AnyOtherTypeOfLayer = 0,
            OpenedFolder = 1,
            ClosedFolder = 2,
            BoundingSectionDivider = 3,
        }
        public enum LsctSubType : int
        {
            Normal = 0,
            SceneGroup = 1,
        }
    }
}
