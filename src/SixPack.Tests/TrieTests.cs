using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixPack.Collections.Generic;

namespace SixPack.Tests
{
	[TestClass]
	public class TrieTests
	{
		[TestMethod]
		public void TestTrie()
		{
			Trie<char, string> wordIndex = new Trie<char, string>(new LettersDomain());

			wordIndex.Add("a", "a");
			wordIndex.Add("aa", "aa");
			wordIndex.Add("ab", "ab");
			wordIndex.Add("ab", "ab2");
			wordIndex.Add("abc", "abc");
			wordIndex.Add("b", "b");
			wordIndex.Add("ba", "ba");
			wordIndex.Add("ba", "ba2");
			wordIndex.Add("ba", "ba3");
			wordIndex.Add("ba", "ba4");
			wordIndex.Add("c", "c");
			wordIndex.Add("cd", "cd");

			int max;

			//PrintTreeRecursive(wordIndex.root, -1, "");
			//Console.WriteLine(" -------------------------------- ");

			//ITrieEnumerator<char, IList<string>> iterator = wordIndex.All1;

			//Console.WriteLine(" MoveNext: -------------------------------- ");

			//while (iterator.MoveNext())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			//Console.WriteLine(" MovePrevious: -------------------------------- ");

			//while (iterator.MovePrevious())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			//Console.WriteLine(" MoveNext: -------------------------------- ");

			//while (iterator.MoveNext())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			//Console.WriteLine(" MovePrevious: -------------------------------- ");

			//int max = 6;
			//while (--max > 0 && iterator.MovePrevious())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			//Console.WriteLine(" MoveNext: -------------------------------- ");

			//max = 4;
			//while (--max > 0 && iterator.MoveNext())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			//Console.WriteLine(" MovePrevious: -------------------------------- ");

			//max = 5;
			//while (--max > 0 && iterator.MovePrevious())
			//{
			//    Console.WriteLine("{0}: {1}", new string(new List<char>(iterator.Current.Key).ToArray()), iterator.Current.Value[0]);
			//}

			Console.WriteLine(" -------------------------------- ");
			Console.WriteLine(" -------------------------------- ");

			ITrieEnumerator<char, string> iterator2 = wordIndex.All;

			while (iterator2.MoveNext())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

			Console.WriteLine(" -------------------------------- ");

			while (iterator2.MovePrevious())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

			Console.WriteLine(" -------------------------------- ");

			while (iterator2.MoveNext())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

			Console.WriteLine(" -------------------------------- ");

			max = 6;
			while (--max > 0 && iterator2.MovePrevious())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

			Console.WriteLine(" MoveNext: -------------------------------- ");

			max = 4;
			while (--max > 0 && iterator2.MoveNext())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

			Console.WriteLine(" MovePrevious: -------------------------------- ");

			max = 5;
			while (--max > 0 && iterator2.MovePrevious())
			{
				Console.WriteLine(iterator2.Current.Value);
			}

		}

		[TestMethod]
		public void PrefixMatchTest()
		{
			string s = @"
				In computer science, a trie, or prefix tree,
				is an ordered tree data structure that is used to
				store an associative array where the keys are strings.
				Unlike a binary search tree, no node in the tree
				stores the key associated with that node;
				instead, its position in the tree shows what key
				it is associated with. All the descendants
				of any one node have a common prefix of the string
				associated with that node, and the root is associated
				with the empty string. Values are normally not associated
				with every node, only with leaves and some inner nodes
				that happen to correspond to keys of interest.
			";

			Trie<char, string> wordIndex = new Trie<char, string>(new LettersDomain());
			foreach (string word in s.Replace("\r", "").Replace("\n", " ").Replace("\t", "").Split(' ', ',', ';', '.'))
			{
				if (word.Length > 0)
				{
					wordIndex.Add(word, word);
				}
			}

			ITrieEnumerator<char, string> matches = wordIndex.PrefixMatch("p");
			while (matches.MoveNext())
			{
				Console.WriteLine(matches.Current.Value);
			}
		}
	}
}
