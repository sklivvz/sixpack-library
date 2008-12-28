// RsaSmallCodeParser.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SixPack.Text;

namespace SixPack.Security.Cryptography
{
    /// <summary>
    /// Parses an encrypted, translated code and transforms it in a BigInteger using the RsaSmall algorithm.
    /// </summary>
    public sealed class RsaSmallCodeParser
    {
        private static readonly char[] correctDigitSequence = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly int radix;
        private readonly RsaSmall rsa;
        private readonly IStringTranslator translator;
        private readonly string validCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSmallCodeParser"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the code.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <param name="translator">The translator.</param>
        public RsaSmallCodeParser(int codeLength, RsaSmallPublicKey rsaPublicKey, IStringTranslator translator)
        {
			if(codeLength < 1)
			{
				throw new ArgumentOutOfRangeException("codeLength", "codeLenth must be greater than zero.");
			}

			if (rsaPublicKey == null)
			{
				throw new ArgumentNullException("rsaPublicKey");
			}
			if (translator == null)
			{
				throw new ArgumentNullException("translator");
			}

        	char[] alphabetLetters = translator.FinalAlphabet.ToCharArray();

            Array.Sort(alphabetLetters);

            int i = 0;
            foreach (char alphabetLetter in alphabetLetters)
            {
                // Ignore duplicates
                if (alphabetLetter != correctDigitSequence[i])
                {
                    // Not a duplicate, move to next char in valid sequence of digits
                    i++;
					if (alphabetLetter != correctDigitSequence[i])
					{
						throw new ArgumentException("The final alphabet does not contain a valid digit sequence.", "translator");
					}
                }
            }
            radix = i + 1;

            rsa = new RsaSmall(new RsaSmallFullKey(rsaPublicKey.Modulus, 0, rsaPublicKey.Exponent));
            this.translator = translator;
            validCodeRegex =
                string.Format(CultureInfo.InvariantCulture, @"^[{0}]{{{1}}}$", translator.InitialAlphabet, codeLength);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSmallCodeParser"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the code.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <param name="initialAlphabet">The initial alphabet.</param>
        /// <param name="finalAlphabet">The final alphabet.</param>
        public RsaSmallCodeParser(int codeLength, RsaSmallPublicKey rsaPublicKey, string initialAlphabet,
                                  string finalAlphabet) :
                                      this(
                                      codeLength, rsaPublicKey,
                                      new AsciiStringTranslator(initialAlphabet, finalAlphabet, true))
        {
        }

        /// <summary>
        /// Parses the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public BigInteger Parse(string code)
        {
            BigInteger result;
            Exception e = privateParse(code, out result);
            if (e != null)
                throw e;
            return result;
        }

        private Exception privateParse(string code, out BigInteger result)
        {
            result = null;
            if (code == null)
                return new ArgumentNullException("code");
            string cleanCode = code.Trim();
            cleanCode = cleanCode.ToUpperInvariant();
            if (!Regex.IsMatch(cleanCode, validCodeRegex))
                return new FormatException("Invalid code format");
            string tranCode = translator.Translate(cleanCode);
            result = rsa.Decrypt(new BigInteger(tranCode, radix));
            return null;
        }

        /// <summary>
        /// Tries to parse the code and returns true if successful. The parsed value is passed in result.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryParse(string code, out BigInteger result)
        {
            Exception e = privateParse(code, out result);
            return e == null;
        }
    }
}
