// DataGenerator.cs 
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

using System;
using System.Globalization;
using System.Text;
using SixPack.Text;

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// A utility class to generate data.
	/// The data generated is not cryptographically secure.
	/// </summary>
	public static class DataGenerator
	{
		private static readonly string[] ptSyllables =
			new string[]
				{
					"ba", "be", "bi", "bo", "bu", "ca", "ce", "ci", "co", "cu", "da", "de", "di", "do", "du", "fa", "fe"
					,
					"fi", "fo", "fu", "ga", "ge", "gi", "go", "gu", "ja", "je", "ji", "jo", "ju", "ka", "ke", "ki", "ko"
					,
					"ku", "la", "le", "li", "lo", "lu", "ma", "me", "mi", "mo", "mu", "na", "ne", "ni", "no", "nu", "pa"
					,
					"pe", "pi", "po", "pu", "ra", "re", "ri", "ro", "ru", "sa", "se", "si", "so", "su", "ta", "te", "ti"
					,
					"to", "tu", "va", "ve", "vi", "vo", "vu", "xa", "xe", "xi", "xo", "xu", "za", "ze", "zi", "zo", "zu"
					,
					"qua", "que", "qui", "quo", "gua", "gue", "gui", "guo"
				};

		private static readonly int ptSyllablesLength = ptSyllables.Length;
		private static readonly Random random = new Random();

		/// <summary>
		/// Returns a random Int32.
		/// </summary>
		/// <returns></returns>
		public static int RandomInt32()
		{
			return random.Next(int.MinValue, int.MaxValue);
		}

		/// <summary>
		/// Returns a random Int64.
		/// </summary>
		/// <returns></returns>
		public static long RandomInt64()
		{
			unchecked
			{
				return (long)RandomInt32() << 32 | (uint)RandomInt32();
			}
		}

		/// <summary>
		/// Returns a random simple String.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static string RandomSimpleString(int length)
		{
			if(length <= 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}

			const string alphabet =
				"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz !�$%^*()-_=+[{]};:'@#~/?.,|";
			int alphabetLength = alphabet.Length;
			StringBuilder sb = new StringBuilder(length);
			for (int i = 0; i < length; i++)
				sb.Append(alphabet.Substring(random.Next(alphabetLength), 1));
			return sb.ToString();
		}

		/// <summary>
		/// Returns a random ASCII String.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static string RandomAsciiString(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			
			StringBuilder sb = new StringBuilder(length);
			for (int i = 0; i < length; i++)
				sb.Append((char) random.Next(32, 127));
			return sb.ToString();
		}
		
		/// <summary>
		/// Returns a random Unicode String.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static string RandomUnicodeString(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			
			StringBuilder sb = new StringBuilder(length);
			UnicodeSemantics us = new UnicodeSemantics(); // all ranges ok
			for (int i = 0; i < length; i++)
			{
				int candidate;
				// will not generate all possible uints, but the one we exclude are not interesting for this purpose.
				do
				{
					candidate = random.Next((int)us.MinValue, (int)us.MaxValue);
				} while (!us.IsInRange((uint)candidate));
				sb.Append((char) candidate);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Returns a random Guid.
		/// </summary>
		/// <returns></returns>
		public static Guid RandomGuid()
		{
			return Guid.NewGuid();
		}

		/// <summary>
		/// Returns a random bool.
		/// </summary>
		/// <returns></returns>
		public static bool RandomBool()
		{
			return (random.Next(0, 2) == 1);
		}

		/// <summary>
		/// Returns a random Decimal.
		/// </summary>
		/// <returns></returns>
		public static decimal RandomDecimal()
		{
			return new decimal(random.Next(int.MinValue, int.MaxValue));
		}

		/// <summary>
		/// Returns a random DateTime.
		/// </summary>
		/// <returns></returns>
		public static DateTime RandomDateTime()
		{
			int month = random.Next(1, 13);
			int year = random.Next(1753, 10000);
			int hour = random.Next(0, 24);
			int minute = random.Next(0, 60);
			int second = random.Next(0, 60);
			int millisecond = random.Next(0, 1000);

			DateTime date = new DateTime(year, month, 1, hour, minute, second, millisecond);
			int daysOfMonth = date.AddMonths(1).AddDays(-1).Day;
			return date.AddDays(random.Next(0, daysOfMonth));
		}

		/// <summary>
		/// Returns a random XML String.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static string RandomXmlString(int length)
		{
			if (length < 14)
				throw new ArgumentOutOfRangeException("length", length, "Parameter should be at least 14");
			return String.Format(CultureInfo.InvariantCulture, @"<test>{0}</test>", RandomSimpleString(length - 13));
		}

		/// <summary>
		/// Returns a pronounceable string made up of syllables
		/// </summary>
		/// <param name="syllables">The number of syllables to generate</param>
		/// <returns></returns>
		public static string RandomSyllables(int syllables)
		{
			if (syllables <= 0)
			{
				throw new ArgumentOutOfRangeException("syllables");
			}
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < syllables; i++)
				sb.Append(ptSyllables[random.Next(ptSyllablesLength)]);
			return sb.ToString();
		}
	}
}
