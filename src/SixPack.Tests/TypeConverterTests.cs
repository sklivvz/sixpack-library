using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixPack.ComponentModel;
using System;

namespace SixPack.Tests
{
	[TestClass]
	public class TypeConverterTests
	{
		private class MyConvertible
		{
		}

		private class MyConverter : GenericTypeConverter<MyConvertible>
		{
			public MyConverter()
			{
				CanConvertTo<int>(_ => 1);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidCastException))]
		public void UnregisteredTypeConverterFails()
		{
			TypeConverter.ChangeType<int>(new MyConvertible());
		}

		[TestMethod]
		public void RegisteringTypeConverterSucceeds()
		{
			TypeConverter.RegisterTypeConverter<MyConvertible, MyConverter>();
	
			var result = TypeConverter.ChangeType<int>(new MyConvertible());
			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void RegisteringTypeConverterTwiceSucceeds()
		{
			TypeConverter.RegisterTypeConverter<MyConvertible, MyConverter>();
			TypeConverter.RegisterTypeConverter<MyConvertible, MyConverter>();

			var result = TypeConverter.ChangeType<int>(new MyConvertible());
			Assert.AreEqual(1, result);
		}
	}
}
