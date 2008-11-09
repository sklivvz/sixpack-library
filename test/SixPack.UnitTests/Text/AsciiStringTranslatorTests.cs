// AsciiStringTranslatorTests.cs 
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
using MbUnit.Framework;
using SixPack.Text;

namespace SixPack.UnitTests.Text
{
	[TestFixture]
	public class AsciiStringTranslatorTests
	{
		[RowTest]
		[Row(null, null, false, ExpectedException = typeof(ArgumentException))]
		[Row("ABC", null, false, ExpectedException = typeof(ArgumentException))]
		[Row(null, "ABC", false, ExpectedException = typeof(ArgumentException))]
		[Row("", "", false, ExpectedException = typeof(ArgumentException))]
		[Row("", "ABC", false, ExpectedException = typeof(ArgumentException))]
		[Row("ABC", "", false, ExpectedException = typeof(ArgumentException))]
		[Row("ABC", "X", false, ExpectedException = typeof(ArgumentException))]
		[Row("aA", "12", false)]
		[Row("aA", "12", true, ExpectedException = typeof(ArgumentException))]
		[Row("ABC", "DEF", false)]
		public void AsciiStringTranslator_ValidatesAplhabets(string initialAlphabet, string finalAlphabet, bool caseInvariant)
		{
			AsciiStringTranslator translator = new AsciiStringTranslator(initialAlphabet, finalAlphabet, caseInvariant);
			Assert.AreEqual(initialAlphabet, translator.InitialAlphabet);
			Assert.AreEqual(finalAlphabet, translator.FinalAlphabet);
			Assert.AreEqual(caseInvariant, translator.IsCaseInvariant);
		}

		[RowTest]
		[Row("123456", "ABCDEF", false, "421", "DBA")]
		[Row("0123456789BCDFGHJKMNPQRSTUVWXZ", "0123456789ABCDEFGHIJKLMNOPQRST", false, "89BCDFGHJKMNPQRS", "89ABCDEFGHIJKLMN")]
		[Row("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ ", false, "Hello world", "HELLO WORLD")]
		[Row("aBc", "123", true, "abcABC", "123123")]
		[Row("aBc", "123", false, "", "")]
		public void Translate_TranslatesCorrectly(string initialAlphabet, string finalAlphabet, bool caseInvariant, string input, string expectedOutput)
		{
			AsciiStringTranslator translator = new AsciiStringTranslator(initialAlphabet, finalAlphabet, caseInvariant);
			Assert.AreEqual(expectedOutput, translator.Translate(input), "The translated text is wrong.");
		}

		[RowTest]
		[Row("aBc", "123", true, null, ExpectedException = typeof(ArgumentNullException))]
		[Row("aBc", "123", true, "x", ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row("aBc", "123", false, "A", ExpectedException = typeof(ArgumentOutOfRangeException))]
		public void Translate_ValidatesInput(string initialAlphabet, string finalAlphabet, bool caseInvariant, string input)
		{
			AsciiStringTranslator translator = new AsciiStringTranslator(initialAlphabet, finalAlphabet, caseInvariant);
			translator.Translate(input);
		}
	}
}
