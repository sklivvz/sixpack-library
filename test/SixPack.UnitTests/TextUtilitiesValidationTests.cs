// TextUtilitiesValidationTests.cs 
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

using SixPack.Text;
using MbUnit.Framework;

namespace SixPack.UnitTests
{
	[TestFixture]
	public class TextUtilitiesValidationTests
	{
		[RowTest]
		[Row("abcdefghijklmnopqrstuvwyxz", 3, "abc")]
		[Row("1234567890", 9, "123456789")]
		[Row("fullsix@fullsix.com", 7, "fullsix")]
		[Row("1", 0, "")]
		[Row("unit testing is so cool", 5, "unit ")]
		[Row("unit testing is so cool", 4, "unit")]
		[Row("unit testing is so cool", 6, "unit t")]
		public void ClipValidationWithoutRespectingWordBoundaries(string text, int maxLength, string result)
		{
			Assert.AreEqual(result, TextUtilities.Clip(text, maxLength, false));
		}

		[RowTest]
		[Row("abcdefghijklmnopqrstuvwyxz", 3, "abcd")]
		[Row("1234567890", 9, "1234567890")]
		[Row("fullsix@fullsix.com", 7, "fullsix@")]
		[Row("1", 0, "1")]
		[Row("unit testing is so cool", 5, "unit")]
		[Row("unit testing is so cool", 4, "unit")]
		[Row("unit testing is so cool", 6, "unit")]
		public void ClipValidationRespectingWordBoundaries(string text, int maxLength, string result)
		{
			Assert.AreEqual(result, TextUtilities.Clip(text, maxLength, true));
		}

		[RowTest]
		[Row("abcdefghijklmnopqrstuvwyxz", 3, "abc", "...")]
		[Row("abcdefghijklmnopqrstuvwyxz", 6, "abc...", "...")]
		[Row("abcdefghijklmnopqrstuvwyxz", 5, "ab...", "...")]
		[Row("1234567890", 9, "123456...", "...")]
		[Row("fullsix@fullsix.com", 7, "fulls:)", ":)")]
		[Row("1", 0, "", ":)")]
		[Row("unit testing is so cool", 5, "unit.", ".")]
		[Row("unit testing is so cool", 4, "uni.", ".")]
		[Row("unit testing is so cool", 6, "unit .", ".")]
		public void ClipValidationWithTerminatorWithoutRespectingWordBoundaries(string text, int maxLength, string result,
		                                                                        string term)
		{
			Assert.AreEqual(result, TextUtilities.Clip(text, maxLength, term, false));
		}

		[RowTest]
		[Row("abcdefghijklmnopqrstuvwyxz", 3, "abcd", "")]
		[Row("abcdefghijklmnopqrstuvwyxz", 5, "abcdef", "")]
		[Row("1234567890", 9, "1234567890", "")]
		[Row("unit testing is so cool", 6, "unit", "")]
		public void ClipValidationWithEmptyTerminatorRespectingWordBoundaries(string text, int maxLength, string result,
		                                                                      string term)
		{
			Assert.AreEqual(result, TextUtilities.Clip(text, maxLength, term, true));
		}

		[RowTest]
		[Row("<html>hi there &copy; </html>", "hi there &copy; ")]
		[Row("<script>alert('buhhh');</script>", "alert('buhhh');")]
		[Row("&lt;script&gt;alert('buhhh');</script>", "&lt;script&gt;alert('buhhh');")]
		[Row("testing<body>purposes", "testingpurposes")]
		[Row("testing <body> purposes", "testing  purposes")]
		public void RemoveHtmlValidtion(string text, string result)
		{
			Assert.AreEqual(result, TextUtilities.RemoveHtml(text));
		}

		[RowTest]
		[Row("CamelCase", "Camel Case")]
		[Row("WhatAGreatCamelCaseTextThisExampleIsDon'tYouThinkSo?",
			"What A Great Camel Case Text This Example Is Don't You Think So?")]
		[Row("AndWhat For ThisSituation", "And What For This Situation")]
		[Row("Camel123This", "Camel123 This")]
		[Row("TheLastOne", "The Last One")]
		public void CamelCaseToSpaceSeparatedConverterTest(string text, string result)
		{
			Assert.AreEqual(result, TextUtilities.ConvertCamelCaseToSpaceSeparated(text));
		}

		//[Test]
		//public void ReplaceAll()
		//{
		//    string original = "Hello world!";
		//    string replaced = TextUtilities.ReplaceAll(original, new string[] { "el", "o" }, new string[] { "o", "a" });

		//    Assert.AreEqual("Hola warld!", replaced);
		//}
	}
}
