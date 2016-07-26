using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    /// <summary>
    /// Fournit un ensemble de fonctions mathématiques
    /// appliquées à la cryptographie
    /// </summary>
    class MathCrypto
    {
        /// <summary>
        /// Recherche si un nombre est premier,
        /// c'est à dire s'il admet ou non des diviseurs
        /// </summary>
        /// <param name="number">Nombre entier à tester</param>
        /// <returns>true si le nombre est premier, false sinon</returns>
        public static bool IsPrime(int number)
        {
            // Cas particuliers
            if (number <= 1) return false;
            if (number == 2) return true;

            // Nombre maximum par lequel diviser
            var boundary = (int)Math.Floor(Math.Sqrt(number));

            // Recherche d'un éventuel diviseur
            for (var i = 2; i <= boundary; ++i)
                if (number % i == 0) return false;

            // Pas de diviseur : nombre premier
            return true;
        }

        /// <summary>
        /// Détermine si deux nombres sont premiers
        /// entre eux en déterminant leur PGCD
        /// </summary>
        /// <param name="a">Un nombre entier</param>
        /// <param name="b">Un nombre entier</param>
        /// <returns>true si les deux nombres sont premiers, false sinon</returns>
        public static bool IsCoprime(int a, int b)
        {
            // Recherche du PGCD de a et b
            while (a != 0 && b != 0)
            {
                if (a > b) a %= b;
                else b %= a;
            }
            // a et b premiers entre eux si PGCD(a,b) == 1
            return Math.Max(a, b) == 1;
        }

        /// <summary>
        /// Effectue un calcul équivalent au Math.Pow() mais dans
        /// dans Z/mZ, c'est à dire qu'à chaque calcul de puissance,
        /// on ne gardera que le reste de la division du résulat par le modulo
        /// </summary>
        /// <param name="number">Nombre à élever à la puissance pow</param>
        /// <param name="pow">Puissance</param>
        /// <param name="m">Modulo</param>
        /// <returns></returns>
        public static int PowModulo(int number, int pow, int m)
        {
            var result = number;
            for (var i = 2; i <= pow; i++)
            {
                result = (result * number) % m;
            }
            return result;
        }

        /// <summary>
        /// Donne un affichage binaire d'un tableau de bytes
        /// classé en commençant par les octets de poids fort
        /// </summary>
        /// <param name="bytes">Tableau de bytes</param>
        /// <returns>Représentation binaire</returns>
        public static string BytesToBinaryString(byte[] bytes) {
            var result = new StringBuilder();
            foreach (var aByte in bytes)
            {
                var number = aByte;
                var currentNumber = "";

                while (number != 0)
                {
                    currentNumber = number%2 + currentNumber;
                    number /= 2;
                }
                result.Append("0" + currentNumber);
            }
            return result.ToString();
        }
    }
}
