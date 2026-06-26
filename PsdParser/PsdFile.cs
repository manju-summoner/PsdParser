namespace PsdParser
{
    public class PsdFile : IDisposable
    {
        readonly Stream stream;
        readonly PsdBinaryReader reader;

        public FileHeaderSection Header { get; }
        public ColorModeDataSection ColorModeDataSection { get; }
        public ImageResourceSection ImageResourceSection { get; }
        public LayerAndMaskInformationSection LayerAndMaskInformationSection { get; }

        public PsdFile(string file)
        {
            //RGBデコードはチャンネルをスキャンライン単位で R->G->B->A と飛び飛びにシークするため、
            //FileStreamのままだとバッファが効かず実質ランダムIOになる。
            //ファイル全体を一度メモリへ読み込み、メモリ上でシークすることでこのコストを排除する。
            //(PSDは全レイヤーを展開する用途であり、全体ロードのメモリ増加は許容範囲)
            //publiclyVisible:trueでGetBuffer()を許可し、デコード時にバッファから直接読めるようにする。
            var bytes = File.ReadAllBytes(file);
            stream = new MemoryStream(bytes, 0, bytes.Length, writable: false, publiclyVisible: true);
            reader = new PsdBinaryReader(stream);

            Header = new FileHeaderSection(reader);
            ColorModeDataSection = new ColorModeDataSection(reader);
            ImageResourceSection = new ImageResourceSection(reader);
            LayerAndMaskInformationSection = new LayerAndMaskInformationSection(reader, Header.Version is 2, Header.Depth, Header.ColorMode);
        }

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // マネージド状態を破棄します (マネージド オブジェクト)
                    stream?.Dispose();
                    reader?.Dispose();
                }

                // アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~PsdFile()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}