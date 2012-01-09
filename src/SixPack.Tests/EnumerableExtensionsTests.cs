using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixPack.Collections.Generic.Extensions;

namespace SixPack.Tests
{
	[TestClass]
	public class EnumerableExtensionsTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void OuterJoinTests()
		{
			int[] outer = { 5, 3, 7, 8 };
			string[] inner = { "bee", "giraffe", "tiger", "badger", "ox", "cat", "dog" };

			var results = new HashSet<string>(
				outer
					.OuterJoin(
						inner,
						outerElement => outerElement,
						innerElement => innerElement.Length,
						outerElement => outerElement + ":-",
						innerElement => "-:" + innerElement,
						(outerElement, innerElement) => outerElement + ":" + innerElement
					)
			);

			AssertContains(results, "5:tiger");
			AssertContains(results, "3:bee");
			AssertContains(results, "3:cat");
			AssertContains(results, "3:dog");
			AssertContains(results, "7:giraffe");
			AssertContains(results, "8:-");
			AssertContains(results, "-:badger");
			AssertContains(results, "-:ox");

			foreach (var remaining in results)
			{
				TestContext.WriteLine(remaining);
			}
			Assert.AreEqual(0, results.Count);
		}

		private void AssertContains(HashSet<string> values, string value)
		{
			Assert.IsTrue(values.Contains(value), "Expected '" + value + "'");
			values.Remove(value);
		}
	}
}