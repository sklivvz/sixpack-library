// RsaSmallCodeParserSection.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Copyright (C) 2008 Marco Cecconi
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

/*
 * Made by: Nuno Lourenço
 * Create date: 2008/03/01
 * Modified date: 2008/04/17
 */
using System.Configuration;

namespace SixPack.Security.Cryptography.Configuration
{
    /// <summary>
    /// Defines the codes configuration section for use in the 'web.config' 
    /// configuration file, for configuring the codes validators/translators
    /// </summary>
    public class RsaSmallCodeParserSection : ConfigurationSection
    {
        #region Configuration properties

        /// <summary>
        /// Code length used by the translator algorithm
        /// </summary>
        [ConfigurationProperty("code-length", IsRequired = true, DefaultValue = int.MaxValue)]
        [IntegerValidator(ExcludeRange = false, MinValue = 1)]
        public int CodeLength
        {
            get
            {
                return (int)this["code-length"];
            }
            set
            {
                this["code-length"] = value;
            }
        }

        /// <summary>
        /// Incoming alphabet
        /// </summary>
        [ConfigurationProperty("initial-alphabet", IsRequired = true, DefaultValue = "MicrosoftKludge")]
        [StringValidator(MinLength = 1)]
        public string InitialAlphabet
        {
            get
            {
                return (string)this["initial-alphabet"];
            }
            set
            {
                this["initial-alphabet"] = value;
            }
        }

        /// <summary>
        /// Outgoing alphabet
        /// </summary>
        [ConfigurationProperty("final-alphabet", IsRequired = true, DefaultValue = "MICROSOFTKLUDGE")]
        [RegexStringValidator("^[0-9A-Z]+$")]
        public string FinalAlphabet
        {
            get
            {
                return (string)this["final-alphabet"];
            }
            set
            {
                this["final-alphabet"] = value;
            }
        }

        /// <summary>
        /// Public key used to decrypt the codes
        /// </summary>
        [ConfigurationProperty("public-key", IsRequired = true)]
        public RsaSmallPublicKeySection PublicKey
        {
            get
            {
                return (RsaSmallPublicKeySection)this["public-key"];
            }
            set
            {
                this["public-key"] = value;
            }
        }
        #endregion
   
        /// <summary>
        /// Called after deserialization.
        /// </summary>
        protected override void PostDeserialize()
        {
            if (((string)this["final-alphabet"]).Length != ((string)this["initial-alphabet"]).Length)
                throw new ConfigurationErrorsException("initial-alphabet and final-alphabet must have the same length.");
            base.PostDeserialize();
        }
    }
}
