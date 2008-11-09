// DataGeneratorTests.cs 
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
using SixPack.Collections.Generic;
using MbUnit.Framework;
using SixPack.Security.Cryptography;
using SixPack.Collections;
using System.Xml;

namespace SixPack.UnitTests.Security.Cryptography
{
	[TestFixture]
	public class DataGeneratorTests
	{
		private delegate T GeneratorDelegate<T>();

		private static void ValuesMustBeDifferentTest<T>(GeneratorDelegate<T> generateValue)
		{
			T initialValue = generateValue();
			for (int i = 0; i < 100; i++)
			{
				T currentValue = generateValue();
				if (!currentValue.Equals(initialValue))
				{
					return;
				}
			}

			Assert.Fail("All values were the same");
		}

		[Test]
		public void RandomInt32_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<int>(DataGenerator.RandomInt32);
		}

		[Test]
		public void RandomInt64_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<long>(DataGenerator.RandomInt64);
		}

		[Test]
		public void RandomInt64_AllBitsMustChange()
		{
			long mask = 1;
			for (int bitIndex = 0; bitIndex < 64; bitIndex++)
			{
				bool bitHasChanged = false;
				long initialValue = DataGenerator.RandomInt64();
				for (int i = 0; i < 100; i++)
				{
					long currentValue = DataGenerator.RandomInt64();
					if ((currentValue & mask) != (initialValue & mask))
					{
						bitHasChanged = true;
						break;
					}
				}
				Assert.IsTrue(bitHasChanged, "Bit {0} never changes", bitIndex);

				mask <<= 1;
			}
		}

		[RowTest]
		[Row(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomSimpleString_LengthIsCorrect(int length)
		{
			string text = DataGenerator.RandomSimpleString(length);
			Assert.AreEqual(length, text.Length, "The length of the string is not what we asked for");
		}

		[RowTest]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomSimpleString_ValuesMustBeDifferent(int length)
		{
			ValuesMustBeDifferentTest(delegate()
			{
				return DataGenerator.RandomSimpleString(length);
			});
		}

		[RowTest]
		[Row(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomAsciiString_LengthIsCorrect(int length)
		{
			string text = DataGenerator.RandomAsciiString(length);
			Assert.AreEqual(length, text.Length, "The length of the string is not what we asked for");
		}

		[RowTest]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomAsciiString_ValuesMustBeDifferent(int length)
		{
			ValuesMustBeDifferentTest(delegate()
			{
				return DataGenerator.RandomAsciiString(length);
			});
		}

		[RowTest]
		[Row(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomUnicodeString_LengthIsCorrect(int length)
		{
			string text = DataGenerator.RandomUnicodeString(length);
			Assert.AreEqual(length, text.Length, "The length of the string is not what we asked for");
		}

		[RowTest]
		[Row(1)]
		[Row(25)]
		[Row(100000)]
		public void RandomUnicodeString_ValuesMustBeDifferent(int length)
		{
			ValuesMustBeDifferentTest(delegate()
			{
				return DataGenerator.RandomUnicodeString(length);
			});
		}

		[Test]
		public void RandomGuid_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<Guid>(DataGenerator.RandomGuid);
		}

		[Test]
		public void RandomBool_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<bool>(DataGenerator.RandomBool);
		}

		[Test]
		public void RandomDecimal_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<decimal>(DataGenerator.RandomDecimal);
		}

		[Test]
		public void RandomDateTime_ValuesMustBeDifferent()
		{
			ValuesMustBeDifferentTest<DateTime>(DataGenerator.RandomDateTime);
		}

		[Test]
		public void RandomDateTime_AllMonthsAreGenerated()
		{
			Set<int> months = new Set<int>();

			for (int i = 0; i < 1000; i++)
			{
				DateTime currentValue = DataGenerator.RandomDateTime();

				if(!months.Contains(currentValue.Month))
				{
					months.Add(currentValue.Month);

					if(months.Count == 12)
					{
						return;
					}
				}
			}

			Assert.Fail("Not all months were generated");
		}

		[Test]
		public void RandomDateTime_AllDaysAreGenerated()
		{
			Set<int> days = new Set<int>();

			for (int i = 0; i < 10000; i++)
			{
				DateTime currentValue = DataGenerator.RandomDateTime();

				if (!days.Contains(currentValue.Day))
				{
					days.Add(currentValue.Day);

					if (days.Count == 31)
					{
						return;
					}
				}
			}

			Assert.Fail("Not all days were generated");
		}

		[Test]
		public void RandomDateTime_TimeIsNotZero()
		{
			for (int i = 0; i < 100; i++)
			{
				DateTime currentValue = DataGenerator.RandomDateTime();

				if (currentValue.Hour != 0 && currentValue.Minute != 0 && currentValue.Second != 0 && currentValue.Millisecond != 0)
				{
					return;
				}
			}

			Assert.Fail("The time is always zero");
		}

		[RowTest]
		[Row(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(14)]
		[Row(25)]
		[Row(100000)]
		public void RandomXmlString_LengthIsCorrect(int length)
		{
			string text = DataGenerator.RandomXmlString(length);
			Assert.AreEqual(length, text.Length, "The length of the string is not what we asked for");
		}

		[RowTest]
		[Row(14)]
		[Row(25)]
		[Row(100000)]
		public void RandomXmlString_ValuesMustBeDifferent(int length)
		{
			ValuesMustBeDifferentTest(delegate()
			{
				return DataGenerator.RandomXmlString(length);
			});
		}

		[RowTest]
		[Row(14)]
		[Row(25)]
		[Row(100000)]
		public void RandomXmlString_IsValidXml(int length)
		{
			string text = DataGenerator.RandomXmlString(length);
			XmlDocument document = new XmlDocument();
			document.LoadXml(text);
		}

		[RowTest]
		[Row(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[Row(14)]
		[Row(25)]
		[Row(100000)]
		public void RandomSyllables_LengthIsCorrect(int length)
		{
			string text = DataGenerator.RandomSyllables(length);
			Assert.IsTrue(length * 2 <= text.Length && text.Length <= length * 3, "The length of the string is not what we asked for");
		}

		[RowTest]
		[Row(1)]
		[Row(25)]
		[Row(1000)]
		public void RandomSyllables_ValuesMustBeDifferent(int length)
		{
			ValuesMustBeDifferentTest(delegate()
			{
				return DataGenerator.RandomSyllables(length);
			});
		}
	}
}
