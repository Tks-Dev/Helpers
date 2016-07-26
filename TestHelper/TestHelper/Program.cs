using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TksHelpers;

namespace TestHelper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var c1 = new Cesar(8)
            {
                CharExceptions = Cesar.Ponctuation
            };
            var text = c1.Encode("ERROR");
            Console.WriteLine(text);
            Console.WriteLine(c1.Decode(text));
            Console.ReadKey();
            var v1 = new Vigenere("BaseText","KeyTexte");
            Console.WriteLine(v1.TextCode);
            text = v1.Encode("Le texte");
            Console.WriteLine(text);
            Console.WriteLine(v1.Decode(text));
            Console.ReadKey();
        }
    }
}
