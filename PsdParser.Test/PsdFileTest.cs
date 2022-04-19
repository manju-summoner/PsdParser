using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
                    WriteBitmap($"output\\{fileName}-{layerName}.bmp", buffer, image.Width, image.Height);
                }
            }
        }

        public static void WriteBitmap(string filePath, byte[] buffer, int width,int height)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            using var writer = new BinaryWriter(stream);
            writer.Write((byte)'B');
            writer.Write((byte)'M');
            writer.Write(buffer.Length + 54);
            writer.Write(0);
            writer.Write(54);
            writer.Write(40);
            writer.Write(width);
            writer.Write(-height);
            writer.Write((short)1);
            writer.Write((short)32);
            writer.Write(0);
            writer.Write(buffer.Length);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
            writer.Write(buffer);
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