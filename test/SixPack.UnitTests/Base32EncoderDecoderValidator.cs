// Base32EncoderDecoderValidator.cs 
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
using SixPack.Standards;
using MbUnit.Framework;

namespace SixPack.UnitTests
{
	[TestFixture]
	public class Base32EncoderDecoderValidator
	{
		[RowTest]
		[Row("BI", new byte[] {10})]
		[Row("AFBA", new byte[] {0x01, 0x42})]
		public void DecoderValidator(string encodedStr, byte[] result)
		{
			//Console.WriteLine(Base32.Encode(result));
			ArrayAssert.AreEqual(result, Base32.Decode(encodedStr));
		}

		[RowTest]
		[Row("BI", new byte[] {10})]
		[Row("AFBA", new byte[] {0x01, 0x42})]
		public void EncoderValidator(string result, byte[] encode)
		{
			//Console.WriteLine(Base32.Decode(result));
			Assert.AreEqual(result, Base32.Encode(encode));
		}
	}
}
