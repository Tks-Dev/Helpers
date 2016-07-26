using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace TksHelpers
{
    public class Vigenere
    {
        public List<int> Code
        {
            get { return _code.Select(c => c.Offset).ToList(); }
            set { _code = value.Select(i => new Cesar(i)).ToList(); }
        }

        private List<Cesar> _code { get; set; }
        public List<char> CharCode
        {
            get { return Code.Select(Convert.ToChar).ToList(); }
            set { Code = new List<int>(value.Select(Convert.ToInt32)); }
        }

        public string TextCode
        {
            get { return CharCode.Aggregate(string.Empty, (current, c) => current + c); }
            set { CharCode = value.ToList(); }
        }

        public Vigenere(params Pair<char, char>[] pairs)
        {
            Code = pairs.Select(p=>p.GetOffset()).ToList();
        }

        public Vigenere(params int[] offsets)
        {
            Code = offsets.ToList();
        }

        public Vigenere(string baseText, string cryptedText)
        {
            var max = Math.Min(baseText.Length, cryptedText.Length);
            var pairs = new List<Pair<char,char>>();
            for (var i = 0; i < max; i++)
            {
                pairs.Add(new Pair<char, char>
                {
                    e1 = baseText[i],
                    e2 = cryptedText[i]
                });
            }
            Code = pairs.Select(p => p.GetOffset()).ToList();
        }

        public string Encode(string toCrypt)
        {
            var output = string.Empty;
            for (var i = 0; i < toCrypt.Length; i++)
                output += _code[i%_code.Count].Encode(toCrypt[i]);
            return output;
        }

        public string Decode(string crypted)
        {
            var output = string.Empty;
            for (var i = 0; i < crypted.Length; i++)
                output += _code[i % _code.Count].Decode(crypted[i]);
            return output;
        }
    }
}