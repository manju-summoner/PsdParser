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

            //2バイトアライメントと4バイトアライメントが混在している？
            //CAIは4バイトアライメントされている必要がある？CAIに関する記述がドキュメント（https://www.adobe.com/devnet-apps/photoshop/fileformatashtml/）に存在しないため詳細は不明
            //Photoshop 2024以降で保存したファイルにCAIが含まれる可能性がある
            //https://helpx.adobe.com/jp/photoshop/using/content-credentials.html
            length += SkipZeroPadding(reader);

            var offset = 4 + 4 + (isLongLength ? 8 : 4);
            InvalidStreamPositionException.ThrowIfInvalid(reader, position, offset + length);
            return info;
        }
        static int SkipZeroPadding(BinaryReader reader)
        {
            var current = reader.BaseStream.Position;
            while(reader.ReadByte() is 0)
            {
            }
            reader.BaseStream.Position--;
            return (int)(reader.BaseStream.Position - current);
        }
        public static int MinSize(bool isPSB) => 4 + 4 + (isPSB ? 8 : 4);
    }
}