using NUnit.Framework;
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

        public static IEnumerable<string> GetPsdFiles()
        {
            var exe = Assembly.GetExecutingAssembly().Location;
            var exeDir = Path.GetDirectoryName(exe);
            if(exeDir is null)
                return Enumerable.Empty<string>();

            var dir = Path.Combine(exeDir, "TestData");
            if(!Directory.Exists(dir))
                return Enumerable.Empty<string>();

            return Directory.GetFiles(dir, "*.psd");
        }

    }
}