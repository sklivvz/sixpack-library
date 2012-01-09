using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SixPack.Tests
{
	[TestClass]
	public class EnumsTests
	{
		public enum MyTestEnum
		{
			[EnumValue("MyValue")]
			HasValue,

			HasNoValue,
		}

		[TestMethod]
		public void TestGetValueFindsValue()
		{
			var result = Enums.GetAssociatedValue(MyTestEnum.HasValue);
			Assert.AreEqual("MyValue", result);
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void TestGetValueThrowsException()
		{
			Enums.GetAssociatedValue(MyTestEnum.HasNoValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TestGetValueRejectsInvalid()
		{
			Enums.GetAssociatedValue((MyTestEnum)int.MaxValue);
		}

		[TestMethod]
		public void TestTryGetValueFindsValue()
		{
			object result;
			var found = Enums.TryGetAssociatedValue(MyTestEnum.HasValue, out result);
			Assert.IsTrue(found);
			Assert.AreEqual("MyValue", result);
		}

		[TestMethod]
		public void TestTryGetValueThrowsException()
		{
			object result;
			var found = Enums.TryGetAssociatedValue(MyTestEnum.HasNoValue, out result);
			Assert.IsFalse(found);
			Assert.IsNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TestTryGetValueRejectsInvalid()
		{
			object result;
			Enums.TryGetAssociatedValue((MyTestEnum)int.MaxValue, out result);
		}
	}
}
