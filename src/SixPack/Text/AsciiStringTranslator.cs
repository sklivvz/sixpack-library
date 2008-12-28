// AsciiStringTranslator.cs 
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
using System.Text;

namespace SixPack.Text
{
    /// <summary>
    /// Implementation of the <see cref="IStringTranslator"/> interface for ASCII-only strings.
    /// </summary>
    public class AsciiStringTranslator : IStringTranslator
    {
        private readonly string finalAlphabet;
        private readonly string initialAlphabet;
        private readonly char[] translationTable = new char[128];
        private readonly bool isCaseInvariant;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsciiStringTranslator"/> class.
        /// </summary>
        /// <param name="initialAlphabet">The initial alphabet.</param>
        /// <param name="finalAlphabet">The final alphabet.</param>
        /// <param name="caseInvariant">if set to <c>true</c> the translation will be case invariant.</param>
        /// <remarks>Both parameters must have the same length.</remarks>
        public AsciiStringTranslator(string initialAlphabet, string finalAlphabet, bool caseInvariant)
        {
            if (string.IsNullOrEmpty(initialAlphabet))
            {
                throw new ArgumentException("Parameter should not be null or empty.", "initialAlphabet");
            }
            if (string.IsNullOrEmpty(finalAlphabet))
            {
                throw new ArgumentException("Parameter should not be null or empty.", "finalAlphabet");
            }
            if (initialAlphabet.Length != finalAlphabet.Length)
            {
                throw new ArgumentException("The two parameters must have the same length.");
            }

            if (caseInvariant)
            {
                this.initialAlphabet = initialAlphabet.ToUpperInvariant();
                this.finalAlphabet = finalAlphabet.ToUpperInvariant();
            }
            else
            {
                this.initialAlphabet = initialAlphabet;
                this.finalAlphabet = finalAlphabet;
            }

            isCaseInvariant = caseInvariant;

            byte[] inascii = Encoding.ASCII.GetBytes(this.initialAlphabet);
            for (int i = 0; i < inascii.Length; i++)
            {
                if (translationTable[inascii[i]] != 0)
                    throw new ArgumentException("Badly formatted initial alphabet.", "initialAlphabet");
                translationTable[inascii[i]] = this.finalAlphabet[i];
            }
        }

        #region IStringTranslator Members

        /// <summary>
        /// Gets the initial alphabet.
        /// </summary>
        /// <value>The initial alphabet.</value>
        public string InitialAlphabet
        {
            get { return initialAlphabet; }
        }

        /// <summary>
        /// Gets the final alphabet.
        /// </summary>
        /// <value>The final alphabet.</value>
        public string FinalAlphabet
        {
            get { return finalAlphabet; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is case invariant.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is case invariant; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseInvariant
        {
            get { return isCaseInvariant; }
        }

        /// <summary>
        /// Translates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string Translate(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (isCaseInvariant)
                text = text.ToUpperInvariant();
            StringBuilder sb = new StringBuilder(text.Length);
            byte[] inascii = Encoding.ASCII.GetBytes(text);
            foreach (byte b in inascii)
            {
                sb.Append(translationTable[b]);
            }
            return sb.ToString();
        }

        #endregion
    }
}
