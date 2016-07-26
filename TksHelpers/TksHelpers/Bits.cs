using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TksHelpers
{
    public class Bits
    {
        public const byte BYTE_BITS = 8;
        public const byte INT16_BITS = 16;
        public const byte INT32_BITS = 32;
        public const byte INT64_BITS = 64;

        private bool[] _bitArray;

        public bool[] BitArray
        {
            get { return _bitArray.Clone() as bool[]; }
            private set { _bitArray = value; }
        }

        public byte AsByte()
        {
            if (BitArray.Length > 8)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, byte>(0, (current, b1) => Convert.ToByte(current * 2 + (b1 ? 1 : 0)));
        }

        public short AsUInt16()
        {
            if (BitArray.Length > 16)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, short>(0, (current, b1) => Convert.ToInt16(current * 2 + (b1 ? 1 : 0)));
        }

        public int AsInt32()
        {
            if (BitArray.Length > 32)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate(0, (current, b1) => Convert.ToInt32(current * 2 + (b1 ? 1 : 0)));
        }

        public long AsInt64()
        {
            if (BitArray.Length > 64)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, long>(0, (current, b1) => Convert.ToInt64(current * 2 + (b1 ? 1 : 0)));
        }

        public void FillFromList(IEnumerable<bool> values)
        {
            _bitArray = values.ToArray();
        }

        public void FillFromList(IEnumerable<int> values)
        {
            if (values.Any(i => i > 1 || i < 0))
                throw new InvalidCastException("At least 1 element isn't 0 or 1");
            _bitArray = values.Select(i => i == 1).ToArray();
        }

        public void FillWithTrue(int length)
        {
            _bitArray = new bool[length];
            for (var i = 0; i < length; i++)
                _bitArray[i] = true;
        }

        public void FillFromByte(byte value)
        {
            var b = value;
            BitArray = new bool[8];
            for (var i = BitArray.Length; i > 0; i--)
            {
                _bitArray[i - 1] = b % 2 != 0;
                b = Convert.ToByte(b / 2);
            }
        }

        public void FillFromUShort(ushort value)
        {
            var b = value;
            BitArray = new bool[16];
            for (var i = BitArray.Length; i > 0; i--)
            {
                _bitArray[i - 1] = b % 2 != 0;
                b = Convert.ToUInt16(b / 2);
            }
        }

        public void FillFromString(string bitsString)
        {
            if (bitsString.Any(c => c != '0' && c != '1'))
                throw new InvalidCastException("The string contains invalid at least 1 incorrect char");
            BitArray = new bool[bitsString.Length];
            for (var i = 0; i < BitArray.Length; i++)
            {
                _bitArray[i] = bitsString[i] == '1';
            }
        }

        public void CreateFromBits(params Bits[] bits)
        {
            CreateFromBitsList(bits);
        }

        public void CreateFromBitsList(IEnumerable<Bits> bits)
        {
            _bitArray = bits.Select(b => b._bitArray).Aggregate((bools, bools1) => bools.Concat(bools1).ToArray());
        }

        public Bits GetMSB()
        {
            if (_bitArray.Length < 8)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(8));
            return b;
        }

        public Bits GetLSB()
        {
            if (_bitArray.Length < 8)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length - 8));
            return b;
        }

        public Bits GetMSQ()
        {
            if (_bitArray.Length < 4)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(4));
            return b;
        }

        public Bits GetLSQ()
        {
            if (_bitArray.Length < 4)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length - 4));
            return b;
        }

        public Bits GetMSBit()
        {
            return GetFirstBits(1);
        }

        public Bits GetLSBit()
        {
            return GetLastBits(1);
        }

        public Bits GetFirstBits(int bitsCount)
        {
            if (_bitArray.Length < bitsCount)
                throw new InvalidDataException("Not Enough bits to take");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(bitsCount));
            return b;
        }

        public Bits GetLastBits(int bitsCount)
        {
            if (_bitArray.Length < bitsCount)
                throw new InvalidDataException("Not Enough bits to take");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length - bitsCount));
            return b;
        }

        public Bits GetInverted()
        {
            var b = new Bits { _bitArray = new bool[_bitArray.Length] };
            for (var i = 0; i < _bitArray.Length; i++)
            {
                b._bitArray[_bitArray.Length - 1 - i] = _bitArray[i];
            }
            return b;
        }

        public void ClearStart()
        {
            var b = _bitArray.ToList();
            while (!b[0])
                b.RemoveAt(0);
        }



        public static Bits operator &(Bits b1, Bits b2)
        {
            var bitsRes = new Bits { _bitArray = new bool[Math.Min(b1._bitArray.Length, b2._bitArray.Length)] };
            for (var i = bitsRes._bitArray.Length; i > 0; i--)
                bitsRes._bitArray[bitsRes.BitArray.Length - i] = b1.BitArray[b1.BitArray.Length - i] & b2.BitArray[b2.BitArray.Length - i];

            return bitsRes;
        }

        public static bool operator ==(Bits b1, Bits b2)
        {
            if (b1._bitArray == null)
                return b2._bitArray == null;
            if (b1._bitArray.Length != b2?._bitArray.Length)
                return false;
            return !b1._bitArray.Where((t, i) => t != b2._bitArray[i]).Any();
        }

        public static bool operator !=(Bits b1, Bits b2)
        {
            return !(b1 == b2);
        }

        public static implicit operator byte(Bits x)
        {
            if (x.BitArray.Length > 8)
                throw new InvalidCastException("Too Many bits");
            return x.BitArray.Aggregate<bool, byte>(0, (current, b1) => Convert.ToByte(current * 2 + (b1 ? 1 : 0)));
        }

        public static implicit operator string(Bits x)
        {
            return x.BitArray.Aggregate(string.Empty, (current, b) => current + (b ? "1" : "0"));
        }

    }
}
