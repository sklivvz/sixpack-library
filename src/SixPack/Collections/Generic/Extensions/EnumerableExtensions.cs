// EnumerableExtensions.cs
//
//  Copyright (C) 2011 Antoine Aubry
//  Author: Antoine Aubry
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
using System.Collections.Generic;
using System.Linq;

namespace SixPack.Collections.Generic.Extensions
{
	/// <summary>
	/// Extension methods for sequences.
	/// </summary>
	public static class EnumerableExtensions
	{
		#region ToTree
		/// <summary>
		/// Builds a tree from a sequence. The order of the items is preserved within each tree level.
		/// </summary>
		/// <typeparam name="TItem">The type of the sequence items.</typeparam>
		/// <typeparam name="TKey">The type of the item key.</typeparam>
		/// <param name="source">The source sequence.</param>
		/// <param name="getKey">A function that returns the key of the item.</param>
		/// <param name="getParentKey">A function that returns the key of the parent of the item.</param>
		/// <returns>Returns the list of all nodes that do not have a parent. An item does not have a parent when its key is null.</returns>
		public static IList<TreeNode<TItem>> ToTree<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> getKey, Func<TItem, TKey> getParentKey)
		{
			return source.ToTree(
				getKey,
				getParentKey,
				i => new TreeNode<TItem> { Value = i },
				(p, c) =>
				{
					p.Children.Add(c);
					c.Parent = p;
				}
			);
		}

		/// <summary>
		/// Builds a tree from a sequence. The order of the items is preserved within each tree level.
		/// </summary>
		/// <typeparam name="TNode">The type of the tree nodes.</typeparam>
		/// <typeparam name="TItem">The type of the sequence items.</typeparam>
		/// <typeparam name="TKey">The type of the item key.</typeparam>
		/// <param name="source">The source sequence.</param>
		/// <param name="getKey">A function that returns the key of the item.</param>
		/// <param name="getParentKey">A function that returns the key of the parent of the item.</param>
		/// <param name="createNode">A function that transforms an item into a tree node.</param>
		/// <param name="addChildNode">An action that adds a child node to a parent node. The first argument is the parent node, and the second is the child node to be added.</param>
		/// <returns>Returns the list of all nodes that do not have a parent. An item does not have a parent when its key is null.</returns>
		public static IList<TNode> ToTree<TNode, TItem, TKey>(
			this IEnumerable<TItem> source,
			Func<TItem, TKey> getKey,
			Func<TItem, TKey> getParentKey,
			Func<TItem, TNode> createNode,
			Action<TNode, TNode> addChildNode
		)
		{
			var treeNodes = source.ToDictionary(getKey, i => new { Key = getParentKey(i), Node = createNode(i) });
			var result = new List<TNode>();
			foreach (var node in treeNodes.Values)
			{
				var parentKey = node.Key;
				if (parentKey != null)
				{
					var parent = treeNodes.TryGetValue(parentKey);
					if (!parent.Found)
					{
						throw new ArgumentException(string.Format("ToTree failed because an item has parentKey = [{0}] and no element with that key has been encountered yet.", parentKey), "source");
					}

					addChildNode(parent.Result.Node, node.Node);
				}
				else
				{
					result.Add(node.Node);
				}
			}
			return result;
		}
		#endregion

		public static IEnumerable<T> TreeWalk<T>(this IEnumerable<T> root, Func<T, IEnumerable<T>> getChildren)
		{
			foreach (var node in root)
			{
				yield return node;
				foreach (var child in getChildren(node).TreeWalk(getChildren))
				{
					yield return child;
				}
			}
		}

		/// <summary>
		/// Indexes the specified source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<IndexedItem<T>> Index<T>(this IEnumerable<T> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				int itemIndex = 0;
				if (enumerator.MoveNext())
				{
					var previousItem = enumerator.Current;
					while (enumerator.MoveNext())
					{
						yield return new IndexedItem<T>(previousItem, itemIndex, itemIndex == 0, false);
						++itemIndex;
						previousItem = enumerator.Current;
					}
					yield return new IndexedItem<T>(previousItem, itemIndex, itemIndex == 0, true);
				}
			}
		}

		/// <summary>
		/// Applies an accumulator function over a sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">An <see cref="IEnumerable{T}"/> to aggregate over.</param>
		/// <param name="func">An accumulator function to be invoked on each element.</param>
		/// <param name="valueIfEmpty">The value to return if source is empty.</param>
		/// <returns>The final accumulator value.</returns>
		/// <exception cref="ArgumentNullException">source or func is null.</exception>
		public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func, TSource valueIfEmpty)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (func == null)
			{
				throw new ArgumentNullException("func");
			}

			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var accumulator = enumerator.Current;
					while (enumerator.MoveNext())
					{
						accumulator = func(accumulator, enumerator.Current);
					}
					return accumulator;
				}
				else
				{
					return valueIfEmpty;
				}
			}
		}

		/// <summary>
		/// Generates all the combinations of <paramref name="n"/> elements of the specified sequence.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <param name="n">The number of items to be picked on each combination.</param>
		/// <returns>
		/// Returns a sequence of sequences of <paramref name="n"/> elements picked from <paramref name="sequence"/>, without repetitions.
		/// </returns>
		public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> sequence, int n)
		{
			if (n == 0)
			{
				yield return Enumerable.Empty<T>();
			}
			else
			{
				var index = 0;
				foreach (var item in sequence)
				{
					++index;
					foreach (var combination in Combinations(sequence.Skip(index), n - 1))
					{
						yield return Enumerable.Repeat(item, 1).Concat(combination);
					}
				}
			}
		}

		/// <summary>
		/// Appends elements to a sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
		/// <param name="source">The sequence to which the items are to be appended.</param>
		/// <param name="itemsToAppend">The items to append.</param>
		/// <returns>
		/// Returns a new sequence that will yield the elements in <paramref name="source"/>
		/// followed by the elements in <paramref name="itemsToAppend"/>.
		/// </returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] itemsToAppend)
		{
			return source.Concat(itemsToAppend);
		}

		/// <summary>
		/// Prepends elements to a sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
		/// <param name="source">The sequence to which the items are to be prepended.</param>
		/// <param name="itemsToPrepend">The items to prepend.</param>
		/// <returns>
		/// Returns a new sequence that will yield the elements in <paramref name="itemsToPrepend"/>
		/// followed by the elements in <paramref name="source"/>.
		/// </returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] itemsToPrepend)
		{
			return itemsToPrepend.Concat(source);
		}
	}
}