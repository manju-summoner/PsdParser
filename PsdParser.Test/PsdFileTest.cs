using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PsdParser.Test
{
    public class PsdFileTest
    {

        [Test]
        public void OpenFile()
        {
            foreach (var file in GetPsdFiles())
            {
                using var psd = new PsdFile(file);
            }
            Assert.Pass();
        }


        [Test]
        public void ParseLayer()
        {
            foreach(var file in GetPsdFiles())
            {
                var fileName = Path.GetFileName(file).Replace(".","_");

                using var psd = new PsdFile(file);
                var layers = psd.LayerAndMaskInformationSection.LayerInfo.Items;
                for (int i = 0; i < layers.Length; i++)
                {
                    var (record, image) = layers[i];
                    if (image.Width is 0 || image.Height is 0) continue;
                    var buffer = image.Read();

                    if (!Directory.Exists("outout"))
                        Directory.CreateDirectory("output");

                    var layerName = record.AdditionalLayerInformations.OfType<AdditionalLayerInformations.UnicodeLayerName>().Select(x=>x.Name).FirstOrDefault() ?? record.LayerName;
                    var layerFileName = EscapeFileName($"{fileName}-{layerName}.png");
                    SavePng($"output\\{layerFileName}", buffer, image.Width, image.Height);
                }
            }
        }

        static string EscapeFileName(string fileName)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');
            return fileName;
        }

        public static void SavePng(string filePath, byte[] buffer, int width, int height)
        {
            using var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            bitmap.UnlockBits(data);
            bitmap.Save(filePath, ImageFormat.Png);
        }

        public static IEnumerable<string> GetPsdFiles()
        {
            var exe = Assembly.GetExecutingAssembly().Location;
            var exeDir = Path.GetDirectoryName(exe);
            if(exeDir is null)
                return Enumerable.Empty<string>();

            var dir = Path.Combine(exeDir, "TestData");
            if(!Directory.Exists(dir))
                return Enumerable.Empty<string>();

            return 
                new[] 
                { 
                    Directory.GetFiles(dir, "*.psd"),
                    Directory.GetFiles(dir, "*.psb")
                }
                .SelectMany(x => x);
        }

    }
}