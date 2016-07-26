using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    /// <summary>
    /// Fournit les services d'encodage et de décryptage par l'algorithme RSA.
    /// Utilise le principe de base de l'algorithme.
    /// 
    /// Soit M le message, P la clé publique et S la clé secrète : S(P(M()) = M.
    /// Pour construire les clés, il faut choisir deux nombres premiers p et q,
    /// calculer leur produit n=pq et m=(p-1)(q-1). On choisit ensuite un
    /// nombre alpha inversible dans Z/mZ et premier avec m
    /// (il permettra de définir la clé de décryptage avec Beta l'inverse de Alpha).
    /// On obtient donc la clé privée (n,Alpha) et la clé publique (n,Beta)
    /// Codage :
    /// Le message original doit être décomposé en une série d'entiers M de valeurs
    /// comprises entre 0 et n-1. Pour chacun de ces entiers, on calculera C≡M^Alpha[n].
    /// Décodage :
    /// On calcule tout d'abord Beta l'inverse de Alpha dans Z/mZ (modulo m) pour obtenir 
    /// la clé privée : (n,Beta). Pour chaque entier codé, il suffit de calculer M≡C^Beta[n].
    /// </summary>
    class RsaEncoder
    {
        /// <summary>
        /// Un des deux nombres premiers permettant de construire les clés
        /// </summary>
        private int _p;

        /// <summary>
        /// Un des deux nombres premiers permettant de construire les clés
        /// </summary>
        private int _q;

        /// <summary>
        /// Produit des nombres premiers p et q
        /// </summary>
        private int _n;

        /// <summary>
        /// Résultat du calcul (p-1)(q-1)
        /// </summary>
        private int _m;

        /// <summary>
        /// Nombre Alpha inversible dans Z/mZ et premier avec m
        /// </summary>
        private int _alpha;

        /// <summary>
        /// Inverse de Alpha
        /// </summary>
        private int _beta;

        /// <summary>
        /// Crée une instance d'un encodeur RSA avec des clés publiques et privées.
        /// </summary>
        /// <param name="p">Nombre premier p permettant de construire les clés</param>
        /// <param name="q">Nombre premier q permettant de construire les clés</param>
        /// <param name="alpha">Nombre Alpha inversible dans Z/mZ avec m=(p-1)(q-1)</param>
        public RsaEncoder(int p, int q, int alpha)
        {
            // p et q doivent être des nombres premiers
            if (!MathCrypto.IsPrime(p) || !MathCrypto.IsPrime(q))
            {
                throw new PrimeException("p et q doivent être des nombres premiers");
            }

            _p = p;
            _q = q;
            _n = p * q;
            _m = (p - 1) * (q - 1);

            // Alpha doit être premier avec m
            if (!MathCrypto.IsCoprime(_m, alpha))
            {
                throw new AlphaException("m=(p-1)(q-1) et alpha doivent être premiers entre eux");
            }

            _alpha = alpha;

            // Calcul de Beta, l'inverse de Alpha dans Z/mZ en utilisant
            // le théorème de Bézout
            for (_beta = 0; (_beta * alpha) % _m != 1; _beta++);
        }

        /// <summary>
        /// Encode un message
        /// </summary>
        /// <param name="input">Message à coder</param>
        /// <returns></returns>
        public byte[] Encode(byte[] input)
        {
            var output = new byte[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                // Chaque nombre est passé à la puissance alpha dans Z/nZ
                output[i] = (byte)MathCrypto.PowModulo(input[i], _alpha, _n);
            }
            return output;
        }

        /// <summary>
        /// Décode un message
        /// </summary>
        /// <param name="input">Message à décoder</param>
        /// <returns>Message décodé</returns>
        public byte[] Decode(byte[] input)
        {
            var output = new byte[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                // Chaque nombre est passé à la puissance beta dans Z/nZ
                output[i] = (byte)MathCrypto.PowModulo(input[i], _beta, _n);
            }
            return output;
        }

        /// <summary>
        /// Convertit une chaîne de caractères en un message codable/décodable
        /// </summary>
        /// <param name="input">Message sous la forme d'une chaîne de caractères</param>
        /// <returns>Message codable/décodable</returns>
        public byte[] StringToBytes(string input)
        {
            // Récupération des caractères du message
            var letters = input.ToCharArray();

            // Récupération des caractères ASCII en supprimant
            // les caractères non ASCII
            var asciiLetters = new List<byte>();
            for (var i = 0; i < letters.Length; i++)
            {
                if (letters[i] <= 127)
                {
                    asciiLetters.Add((byte)letters[i]);
                }
            }

            // Les octets à envoyer au BigInteger doivent respecter 
            // l'ordre avec primauté des octets de poids faible
            asciiLetters.Reverse();

            // Nombre brut contenant le message
            var rawNumber = new BigInteger(asciiLetters.ToArray());

            // Message à encoder
            var message = new List<byte>();

            // Décomposition en base n
            while (rawNumber > 0)
            {
                message.Add((byte)(rawNumber % _n));
                rawNumber /= _n;
            }
            message.Reverse();

            // Encodage classique
            return message.ToArray();
        }

        /// <summary>
        /// Convertit un message en son équivalent en chaîne de caractères
        /// </summary>
        /// <param name="input">Message brut</param>
        /// <returns>Message sous forme d'une chaîne de caractères</returns>
        public string BytesToString(byte[] input)
        {
            // Conversion base n => base 10
            var rawNumber = new BigInteger(0);
            for (var i = 0; i < input.Length - 1 ; i++)
            {
                rawNumber += input[i];
                rawNumber *= _n;
            }
            // Ajout du dernier nombre (n^0)
            rawNumber += input[input.Length - 1];

            // On récupère les codes des caractères
            // en inversant pour retrouver les octets de poids fort en première position
            var asciiLetters = rawNumber.ToByteArray().Reverse().ToArray<byte>();

            // Reconstruction de la chaîne ASCII
            var result = new StringBuilder();
            foreach (var letter in asciiLetters)
            {
                result.Append((char)letter);
            }

            // Chaîne ASCII résultat
            return result.ToString();
        }

        public string GetProps()
        {
            return "p : " + _p + ", q : " + _q + ", alpha : " + _alpha + ", m : " + _m;
        }
    }

    public class PrimeException : ArgumentException
    {
        public PrimeException(string message)
        {
            
        }
        
    }

    public class AlphaException : ArgumentException
    {
        public AlphaException(string message) { }
    }
}