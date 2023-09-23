using PsdParser.AdditionalLayerInformations;

namespace PsdParser
{
    public class AdditionalLayerInformation
    {
        static readonly string[] LongLengthKeys = new[]
        {
            "LMsk", "Lr16", "Lr32", "Layr", "Mt16", "Mt32", "Mtrn", "Alph", "FMsk", "lnk2", "FEid", "FXid", "PxSD"
        };

        private protected PsdBinaryReader reader;
        public long Position { get; }
        public string Key { get; }
        public long Length { get; }

        private protected AdditionalLayerInformation(PsdBinaryReader reader, string key, long length)
        {
            this.reader = reader;
            Position = reader.BaseStream.Position;
            Key = key;
            Length = length;

            Load(reader,length);
            reader.BaseStream.Position = Position + length;
        }

        private protected virtual void Load(PsdBinaryReader reader, long length) { }

        internal static AdditionalLayerInformation Parse(PsdBinaryReader reader,bool isPSB)
        {
            var position = reader.BaseStream.Position;

            var signature = new string(reader.ReadChars(4));
            if (signature != "8BIM" && signature != "8B64")
                throw new InvalidSignatureException(signature);

            var key = new string(reader.ReadChars(4));

            bool isLongLength = isPSB && LongLengthKeys.Contains(key);
            var length = isLongLength ? reader.ReadInt64() : reader.ReadUInt32();

            var info = key switch
            {
                AdditionalLayerInformationKeys.LayerID => new LayerID(reader, length),
                AdditionalLayerInformationKeys.UnicodeLayerName => new UnicodeLayerName(reader, length),
                AdditionalLayerInformationKeys.SectionDividerSetting => new SectionDividerSetting(reader, length),
                _ => new AdditionalLayerInformation(reader, key, length),
            };
            
            if (length % 2 is not 0)
            {
                length++;
                reader.BaseStream.Position++;
            }
            
            var offset = 4 + 4 + (isLongLength ? 8 : 4);
            InvalidStreamPositionException.ThrowIfInvalid(reader, position, offset + length);
            return info;
        }
        public static int MinSize(bool isPSB) => 4 + 4 + (isPSB ? 8 : 4);
    }
}