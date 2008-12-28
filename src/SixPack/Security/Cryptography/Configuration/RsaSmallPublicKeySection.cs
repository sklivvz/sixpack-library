// RsaSmallPublicKeySection.cs 
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

using System.Configuration;

namespace SixPack.Security.Cryptography.Configuration
{
    /// <summary>
    /// Encapsulates the data necessary to generate an RsaSmallPublicKey
    /// </summary>
    public class RsaSmallPublicKeySection : ConfigurationSection
    {
        /// <summary>
        /// Modulus
        /// </summary>
        [ConfigurationProperty("modulus", DefaultValue = "0", IsRequired = true)]
        [RegexStringValidator("^[0-9A-F]+$")]
        private string Modulus
        {
            get
            {
                return (string)this["modulus"];
            }
            set
            {
                this["modulus"] = value;
            }
        }

        /// <summary>
        /// Exponent
        /// </summary>
        [ConfigurationProperty("exponent", DefaultValue = "0", IsRequired = true)]
        [RegexStringValidator("^[0-9A-F]+$")]
        private string Exponent
        {
            get
            {
                return (string)this["exponent"];
            }
            set
            {
                this["exponent"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the RsaSmallKey value.
        /// </summary>
        /// <value>The value.</value>
        public RsaSmallPublicKey Value
        {
            get
            {
                return new RsaSmallPublicKey(new BigInteger(Modulus, 16), new BigInteger(Exponent, 16));
            }

            set
            {
                Modulus = value.Modulus.ToHexString();
                Exponent = value.Exponent.ToHexString();
            }
        }
    }
}
