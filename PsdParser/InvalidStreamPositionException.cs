using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsdParser
{
    internal class InvalidStreamPositionException(long current, long from, long length) : Exception($"Invalid stream position. current: {current}, target: {from + length}, from: {from}, length: {length}")
    {
        public InvalidStreamPositionException(PsdBinaryReader reader, long from, long length) : this(reader.BaseStream.Position, from, length)
        {

        }
        public static void ThrowIfInvalid(PsdBinaryReader reader, long from, long length)
        {
            if (reader.BaseStream.Position != from + length)
                throw new InvalidStreamPositionException(reader, from, length);
        }
    }
}
