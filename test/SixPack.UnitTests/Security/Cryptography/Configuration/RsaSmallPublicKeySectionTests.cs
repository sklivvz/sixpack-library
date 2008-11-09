// RsaSmallPublicKeySectionTests.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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
using SixPack.Security.Cryptography;
using MbUnit.Framework;
using System.Configuration;
using SixPack.Security.Cryptography.Configuration;

namespace SixPack.UnitTests.Security.Cryptography.Configuration
{
    [TestFixture]
    public class RsaSmallPublicKeySectionTests
    {
        [Test]
        public void ShouldLoad()
        {
            RsaSmallPublicKeySection config = (RsaSmallPublicKeySection)ConfigurationManager.GetSection("RsaSmallPublicKeySection_ShouldLoad");
            Assert.AreEqual(1, config.Value.Modulus.IntValue());
            Assert.AreEqual(2, config.Value.Exponent.IntValue());
        }

        [RowTest]
        [Row("RsaSmallPublicKeySection_P_MissingModulus", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        [Row("RsaSmallPublicKeySection_P_EmptyModulus", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        [Row("RsaSmallPublicKeySection_P_InvalidModulus", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        [Row("RsaSmallPublicKeySection_P_VariousValues1", "0", "0")]
        [Row("RsaSmallPublicKeySection_P_VariousValues2", "DEAD", "BEEF")]
        [Row("RsaSmallPublicKeySection_P_VariousValues3", "01234567CAFEBABE", "FEEDFACE01234567")]
        [Row("RsaSmallPublicKeySection_P_VariousValues4", "0BADF00DCAFEBABEFEEDFACEDEADBEEF", "DEADBEEFFEEDFACECAFEBABE0BADF00D")]
        [Row("RsaSmallPublicKeySection_P_MissingExponent", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        [Row("RsaSmallPublicKeySection_P_EmptyExponent", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        [Row("RsaSmallPublicKeySection_P_InvalidExponent", "-1", "-1", ExpectedException = typeof(ConfigurationErrorsException))]
        public void PropertiesShouldBeLoadedCorrectly(string SectionName, string expectedModulus, string expectedExponent)
        {
            BigInteger mod = new BigInteger(expectedModulus, 16);
            BigInteger exp = new BigInteger(expectedExponent, 16);
            RsaSmallPublicKeySection config = (RsaSmallPublicKeySection)ConfigurationManager.GetSection(SectionName);
            Assert.IsNotNull(config, "Config is null.");
            Assert.IsNotNull(config.Value,"Value is null.");
            Assert.IsNotNull(config.Value.Modulus, "Modulus is null.");
            Assert.IsNotNull(config.Value.Exponent, "Exponent is null.");
            Assert.AreEqual(mod, config.Value.Modulus, "Modulus not loaded correctly.");
            Assert.AreEqual(exp, config.Value.Exponent, "Exponent not loaded correctly.");
        }
    }
}
