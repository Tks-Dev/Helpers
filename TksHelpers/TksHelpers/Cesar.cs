using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TksHelpers
{
    public class Cesar
    {
        public static readonly char[] Ponctuation = { ' ', '.', ';', ',', ':', '!', '?', '\'', '"', '_', '-', '<', '>' };

        public int Offset { get; set; }

        public char CharOffset
        {
            get { return Convert.ToChar(Offset); }
            set { Offset = Convert.ToInt32(value); }
        }

        public char[] CharExceptions { get; set; }

        public int? AlphabetLength { get; private set; }

        private Cesar() { }

        public Cesar(int offset)
        {
            Offset = offset;
        }

        public Cesar(char offset)
        {
            CharOffset = offset;
        }

        public Cesar(Pair<char, char> offset)
        {
            Offset = offset.GetOffset();
        }

        public Cesar(int offset, int? alphabetLength)
        {
            AlphabetLength = alphabetLength;
            Offset = offset;
        }

        public Cesar(Pair<char, char> offset, int? alphabetLength)
        {
            CharOffset = Convert.ToChar(offset.GetOffset());
            AlphabetLength = alphabetLength;
        }

        public string Encode(string clear)
        {
            var mod = AlphabetLength ?? char.MaxValue;

            return clear.Aggregate(string.Empty, (current, c) => current +
            (CharExceptions?.Contains(c) == true ?
            c : Convert.ToChar((c + Offset) % mod)));
        }

        public char Encode(char clear)
        {
            var mod = AlphabetLength ?? char.MaxValue;
            return CharExceptions?.Contains(clear) == true
                ? clear
                : Convert.ToChar((clear + Offset) % mod);
        }

        public string Decode(string cryted)
        {
            var mod = AlphabetLength ?? char.MaxValue;
            int a;
            return cryted.Aggregate(string.Empty, (current, c) => current +
            (CharExceptions?.Contains(c) == true ?
            c : Convert.ToChar((a = (c - Offset)) < 0 ? a + mod : a) % mod));
        }

        public char Decode(char crypted)
        {
            var mod = AlphabetLength ?? char.MaxValue;
            return CharExceptions?.Contains(crypted) == true
                ? crypted
                : Convert.ToChar((crypted - Offset) % mod);
        }
    }

    public class CesarOffset
    {
        public char Base { get; set; }
        public char Next { get; set; }
    }

    public class Pair<T, U>
    {
        public T e1 { get; set; }
        public U e2 { get; set; }
    }

    public static class PairExtension
    {
        public static int GetOffset(this Pair<char, char> p)
        {
            var off = p.e2 - p.e1;
            return off > 0 ? off : off + char.MaxValue;
        }
    }
}
