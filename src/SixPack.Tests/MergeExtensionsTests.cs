using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixPack.Collections.Algorithms;

namespace SixPack.Tests
{

	[TestClass]
	public class MergeExtensionsTests
	{
		[TestMethod]
		public void SimpleMergeSucceeds()
		{
			var lengths = new[] { 1, 3, 4, 6 };
			var words = new[] { "car", "dog", "free", "sixpack" };

			var uniqueLengths = new HashSet<int>();
			var uniqueWords = new HashSet<string>();
			var pairs = new HashSet<Tuple<int, string>>();

			lengths.Merge(
				words,
				l => l,
				w => w.Length,
				l => uniqueLengths.Add(l),
				w => uniqueWords.Add(w),
				(l, w) => pairs.Add(Tuple.Create(l, w))
			);

			Assert.AreEqual(2, uniqueLengths.Count);
			Assert.IsTrue(uniqueLengths.Contains(1));
			Assert.IsTrue(uniqueLengths.Contains(6));

			Assert.AreEqual(1, uniqueWords.Count);
			Assert.IsTrue(uniqueWords.Contains("sixpack"));

			Assert.AreEqual(3, pairs.Count);
			Assert.IsTrue(pairs.Contains(Tuple.Create(3, "car")));
			Assert.IsTrue(pairs.Contains(Tuple.Create(3, "dog")));
			Assert.IsTrue(pairs.Contains(Tuple.Create(4, "free")));
		}

		[TestMethod]
		public void MergeWithNullKeysSucceeds()
		{
			var wordsA = new[] { "car", "dog", "free", null };
			var wordsB = new[] { "car", "dog", "free", "sixpack" };

			var uniqueA = new HashSet<string>();
			var uniqueB = new HashSet<string>();
			var pairs = new HashSet<string>();

			wordsA.Merge(
				wordsB,
				a => a,
				b => b,
				a => uniqueA.Add(a),
				b => uniqueB.Add(b),
				(a, b) => pairs.Add(a)
			);

			Assert.AreEqual(1, uniqueA.Count);
			Assert.IsTrue(uniqueA.Contains(null));

			Assert.AreEqual(1, uniqueB.Count);
			Assert.IsTrue(uniqueB.Contains("sixpack"));

			Assert.AreEqual(3, pairs.Count);
			Assert.IsTrue(pairs.Contains("car"));
			Assert.IsTrue(pairs.Contains("dog"));
			Assert.IsTrue(pairs.Contains("free"));
		}
	}
}
