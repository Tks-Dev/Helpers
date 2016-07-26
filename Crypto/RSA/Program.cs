using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        private static RsaEncoder rsa;
        private static readonly byte[] _message = {
            01, 00, 40, 07, 00, 18, 28, 47, 13, 24, 12, 46,
            15, 06, 47, 52, 52, 51, 51, 12, 05, 25, 35, 27,
            24, 42, 50, 29, 30, 17, 43, 36, 26, 46, 35, 19,
            54, 49, 38, 14, 18, 49, 50, 08, 26
        };

        static void Main(string[] args)
        {
            /*
             * Codage du tableau message
             */
            Console.WriteLine("Elements en clair : ");
            foreach (var element in _message)
            {
                Console.Write(element + ", ");
            }
            Console.WriteLine();

            // Création d'un encodeur RSA avec p=5, q=11 et Alpha=3
            rsa = new RsaEncoder(5, 11, 3);

            // Encodage
            var encoded = rsa.Encode(_message);

            // Affichage des éléments codés
            Console.WriteLine("\nElements codés : ");
            foreach (var element in encoded)
            {
                Console.Write(element + ", ");
            }
            Console.WriteLine();

            // Décodage
            var decoded = rsa.Decode(encoded);

            // Affichage des éléments décodés
            Console.WriteLine("\nElements décodés : ");
            foreach (var element in decoded)
            {
                Console.Write(element + ", ");
            }
            Console.WriteLine();

            /*
             * Codage de chaînes de caractères
             */

            // Codage et affichage
            Console.WriteLine("\nCodage du message \"Tangente, l'aventure mathematique\" : ");
            var stringEncoded = rsa.Encode(rsa.StringToBytes("Tangente, l'aventure mathematique"));
            foreach (var element in stringEncoded)
            {
                Console.Write(element + ", ");
            }
            Console.WriteLine();

            // Décodage et affichage
            var stringDecoded = rsa.BytesToString(rsa.Decode(stringEncoded));
            Console.WriteLine("\nChaîne décodée : \n" + stringDecoded);


            // Codage et affichage
            Console.WriteLine("\nCodage du message \"Hello world!\" : ");
            var stringEncoded2 = rsa.Encode(rsa.StringToBytes("Hello world!"));
            foreach (var element in stringEncoded2)
            {
                Console.Write(element + ", ");
            }
            Console.WriteLine();

            // Décodage et affichage
            var stringDecoded2 = rsa.BytesToString(rsa.Decode(stringEncoded2));
            Console.WriteLine("\nChaîne décodée : \n" + stringDecoded2);

            // Pause
            Console.WriteLine("Continuer avec votre texte ? (o/n)");
            rsa = null;
            while (Console.ReadLine().ToLower().FirstOrDefault() == 'o')
            {
                DoRsa();
                Console.WriteLine("Saisissez le texte : ");
                stringEncoded2 = rsa.Encode(rsa.StringToBytes(Console.ReadLine()));
                foreach (var element in stringEncoded2)
                {
                    Console.Write(element + ", ");
                }
                Console.WriteLine();

                // Décodage et affichage
                stringDecoded2 = rsa.BytesToString(rsa.Decode(stringEncoded2));
                Console.WriteLine("\nChaîne décodée : \n" + stringDecoded2);
                Console.WriteLine("Continuer avec votre texte ? (o/n)");
            }

        }

        private static void DoRsa()
        {
            if (rsa != null)
                return;

            var p = 877;
            while (!MathCrypto.IsPrime(p))
                p--;
            var q = 3*p/4;
            var alpha = p*q;
            do
            {
                try
                {
                    if (q < 0)
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                    rsa = new RsaEncoder(p, q, alpha);
                }
                catch (PrimeException)
                {
                    q--;
                }
                catch (AlphaException)
                {
                    alpha++;
                }
            } while (rsa == null);
            Console.WriteLine(rsa.GetProps());
        }
    }

}
