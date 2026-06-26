using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace PsdParser.Test
{
    /// <summary>
    /// PSDの読み込み・デコード性能の計測と、最適化によってデコード結果が変化していないことを
    /// 保証するリグレッションテスト。
    ///
    /// ・リグレッション: 実テストファイル(rgb/cmyk, psd/psb)および合成大型PSDをデコードし、
    ///   全レイヤーのピクセル出力のSHA256が既知のゴールデン値と一致することを確認する。
    ///   最適化で出力が1バイトでも変われば失敗する。
    /// ・計測: 解析+全レイヤーデコードに要する時間を複数回計測してコンソールへ出力する
    ///   (Explicit。`dotnet test --filter`で明示的に実行)。
    /// </summary>
    [TestFixture]
    public class PerformanceRegressionTests
    {
        // 合成大型PSD(背の高いRLEレイヤーを多数含む)。RLEデコードのO(H^2)経路を強く刺激する。
        const int SynthWidth = 800;
        const int SynthHeight = 1600;
        const int SynthLayers = 6;

        static string? synthPath;

        [OneTimeSetUp]
        public void SetUp()
        {
            var bytes = SyntheticPsd.CreateRgb(SynthWidth, SynthHeight, SynthLayers);
            synthPath = Path.Combine(Path.GetTempPath(), $"psdparser_synth_{SynthWidth}x{SynthHeight}x{SynthLayers}.psd");
            File.WriteAllBytes(synthPath, bytes);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            try { if (synthPath != null && File.Exists(synthPath)) File.Delete(synthPath); }
            catch { /* 一時ファイルの削除失敗は無視 */ }
        }

        // ---- リグレッション(出力の安定性) ----

        // ゴールデン値は GoldenHash テストで出力した実測値を貼り付けて固定する。
        static readonly System.Collections.Generic.Dictionary<string, string> Goldens = new()
        {
            ["rgb.psd"] = "16284C9FE0982FB4EB38FF5CA4496256A09099BCF11A2191FF84E4BF7ABDC43A",
            ["rgb.psb"] = "16284C9FE0982FB4EB38FF5CA4496256A09099BCF11A2191FF84E4BF7ABDC43A",
            ["cmyk.psd"] = "BEC051DF198783128EF1CB7109C46EA454929097EF62C0FECC90F7F951E99E19",
            ["cmyk.psb"] = "C8A40691E373C4FA1453A87D02E57D3078B72C1DB46C0B8735E3189D3E0328B8",
            ["synth"] = "569337D7ADD269D90615926ECA5ACF0C90426916F38C993E29BF6409D2BD23F9",
        };

        [Test]
        public void DecodeOutput_RealFiles_IsStable()
        {
            foreach (var file in PsdFileTest.GetPsdFiles())
            {
                var key = Path.GetFileName(file);
                if (!Goldens.TryGetValue(key, out var expected) || expected == "PLACEHOLDER")
                    Assert.Ignore($"golden未設定: {key}");
                var actual = HashAllLayers(file);
                Assert.That(actual, Is.EqualTo(expected), $"{key} のデコード出力が変化しています");
            }
        }

        [Test]
        public void DecodeOutput_Synthetic_IsStable()
        {
            if (Goldens["synth"] == "PLACEHOLDER")
                Assert.Ignore("golden未設定: synth");
            var actual = HashAllLayers(synthPath!);
            Assert.That(actual, Is.EqualTo(Goldens["synth"]), "合成PSDのデコード出力が変化しています");
        }

        /// <summary>各テストファイルの現在のハッシュを出力する(ゴールデン値の更新用)。</summary>
        [Test]
        public void GoldenHash_Print()
        {
            foreach (var file in PsdFileTest.GetPsdFiles())
                TestContext.Out.WriteLine($"GOLDEN {Path.GetFileName(file)} = {HashAllLayers(file)}");
            TestContext.Out.WriteLine($"GOLDEN synth = {HashAllLayers(synthPath!)}");
        }

        static string HashAllLayers(string file)
        {
            using var psd = new PsdFile(file);
            using var sha = SHA256.Create();
            var layers = psd.LayerAndMaskInformationSection.LayerInfo.Items;
            foreach (var (record, image) in layers)
            {
                if (image.Width is 0 || image.Height is 0) continue;
                var buffer = image.Read();
                // 寸法も指紋に含める
                Span<byte> dim = stackalloc byte[8];
                BitConverter.TryWriteBytes(dim[..4], image.Width);
                BitConverter.TryWriteBytes(dim[4..], image.Height);
                sha.TransformBlock(dim.ToArray(), 0, 8, null, 0);
                sha.TransformBlock(buffer, 0, buffer.Length, null, 0);
            }
            sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            return Convert.ToHexString(sha.Hash!);
        }

        // ---- 計測 ----

        [Test, Explicit("ベンチマーク: 明示実行のみ")]
        public void Benchmark_RealFiles()
        {
            foreach (var file in PsdFileTest.GetPsdFiles())
                Measure(Path.GetFileName(file), file, iterations: 20);
        }

        [Test, Explicit("ベンチマーク: 明示実行のみ")]
        public void Benchmark_SyntheticLarge()
        {
            Measure($"synth {SynthWidth}x{SynthHeight}x{SynthLayers}", synthPath!, iterations: 10);
        }

        static void Measure(string label, string file, int iterations)
        {
            // ファイルをOSキャッシュへ載せる + JITウォームアップ
            for (int i = 0; i < 2; i++) DecodeAll(file);

            var swParse = new Stopwatch();
            var swDecode = new Stopwatch();
            long bytes = 0;
            for (int i = 0; i < iterations; i++)
            {
                swParse.Start();
                using var psd = new PsdFile(file);
                var layers = psd.LayerAndMaskInformationSection.LayerInfo.Items;
                swParse.Stop();

                swDecode.Start();
                foreach (var (record, image) in layers)
                {
                    if (image.Width is 0 || image.Height is 0) continue;
                    bytes += image.Read().Length;
                }
                swDecode.Stop();
            }

            double parseMs = swParse.Elapsed.TotalMilliseconds / iterations;
            double decodeMs = swDecode.Elapsed.TotalMilliseconds / iterations;
            TestContext.Out.WriteLine(
                $"[{label}] parse={parseMs:F2}ms decode={decodeMs:F2}ms total={parseMs + decodeMs:F2}ms ({iterations} iters)");
        }

        static void DecodeAll(string file)
        {
            using var psd = new PsdFile(file);
            foreach (var (record, image) in psd.LayerAndMaskInformationSection.LayerInfo.Items)
            {
                if (image.Width is 0 || image.Height is 0) continue;
                image.Read();
            }
        }
    }
}
