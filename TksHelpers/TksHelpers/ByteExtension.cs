using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TksHelpers
{
    public static class ByteExtension
    {
        public static Bits AsBits(this byte value)
        {
            var bits = new Bits();
            bits.FillFromByte(value);
            return bits;
        }
    }
}
