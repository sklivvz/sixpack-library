// BigIntegerTests.cs 
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

using SixPack.Security.Cryptography;
using MbUnit.Framework;

namespace SixPack.UnitTests.Security.Cryptography
{
    [TestFixture]
    public class BigIntegerTests
    {
        [RowTest]
        [Row("0", "0", true)]
        [Row("-DEADBEEF", "-DEADBEEF", true)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            "CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            true)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            , true)]
        [Row("0", "0BADF00D", false)]
        [Row("-DEADBEEF", "-0BADF00D", false)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            "0BADF00D", false)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            "-0BADF00D", false)]
        [Row("0", null, false)]
        [Row("-DEADBEEF", null, false)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            null, false)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            null, false)]
        public void Equals_ShouldWork(string a, string b, bool res)
        {
            BigInteger foo = a == null ? null : new BigInteger(a, 16);
            BigInteger bar = b == null ? null : new BigInteger(b, 16);
            if (a != null)
                Assert.AreEqual(foo.Equals(bar), res);
            if (b != null)
                Assert.AreEqual(bar.Equals(foo), res);
        }

        [RowTest]
        [Row("0", "0", true)]
        [Row("-DEADBEEF", "-DEADBEEF", true)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            "CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            true)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            , true)]
        [Row("0", "0BADF00D", false)]
        [Row("-DEADBEEF", "-0BADF00D", false)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            "0BADF00D", false)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            "-0BADF00D", false)]
        [Row("0", null, false)]
        [Row("-DEADBEEF", null, false)]
        [Row("CAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABECAFEBABE",
            null, false)]
        [Row(
            "-FEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACEFEEDFACE"
            ,
            null, false)]
        [Row(null, null, true)]
        public void OperatorEquals_ShouldWork(string a, string b, bool res)
        {
            BigInteger foo = a == null ? null : new BigInteger(a, 16);
            BigInteger bar = b == null ? null : new BigInteger(b, 16);
            Assert.AreEqual(foo == bar, res);
            Assert.AreEqual(bar == foo, res);
        }
    }
}
