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
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (getKey == null)
			{
				throw new ArgumentNullException("getKey");
			}

			if (getParentKey == null)
			{
				throw new ArgumentNullException("getParentKey");
			}

			if (createNode == null)
			{
				throw new ArgumentNullException("createNode");
			}

			if (addChildNode == null)
			{
				throw new ArgumentNullException("addChildNode");
			}

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

		/// <summary />
		public delegate void AddChildNodeDelegate<TNode>(TNode parent, TNode child);
		#endregion

		#region	TreeWalk
		/// <summary>
		/// Enumerates every node of a tree structure using pre-order walk traversing method.
		/// </summary>
		/// <typeparam name="T">The type of the nodes.</typeparam>
		/// <param name="root">A sequence containing the root nodes.</param>
		/// <param name="getChildren">A function that returns the children of a node.</param>
		/// <returns></returns>
		public static IEnumerable<T> TreeWalk<T>(this IEnumerable<T> root, Func<T, IEnumerable<T>> getChildren)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}

			if (getChildren == null)
			{
				throw new ArgumentNullException("getChildren");
			}

			return TreeWalkImpl(root, getChildren);
		}

		private static IEnumerable<T> TreeWalkImpl<T>(IEnumerable<T> root, Func<T, IEnumerable<T>> getChildren)
		{
			foreach (var node in root)
			{
				yield return node;
				var children = getChildren(node);
				if (children != null)
				{
					foreach (var child in TreeWalkImpl(children, getChildren))
					{
						yield return child;
					}
				}
			}
		}

		/// <summary>
		/// Enumerates every node of a tree structure using pre-order walk traversing method.
		/// </summary>
		/// <typeparam name="T">The type of the nodes.</typeparam>
		/// <typeparam name="TNode">The type of the resulting nodes.</typeparam>
		/// <param name="root">A sequence containing the root nodes.</param>
		/// <param name="getChildren">A function that returns the children of a node.</param>
		/// <param name="createNode">A function that takes the .</param>
		/// <returns></returns>
		public static IEnumerable<TNode> TreeWalk<T, TNode>(this IEnumerable<T> root, Func<T, IEnumerable<T>> getChildren, CreateNodeDelegate<T, TNode> createNode)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}

			if (getChildren == null)
			{
				throw new ArgumentNullException("getChildren");
			}

			if (createNode == null)
			{
				throw new ArgumentNullException("createNode");
			}

			return TreeWalkImpl(root, getChildren, createNode, default(T));
		}

		private static IEnumerable<TNode> TreeWalkImpl<T, TNode>(IEnumerable<T> root, Func<T, IEnumerable<T>> getChildren, CreateNodeDelegate<T, TNode> createNode, T parent)
		{
			foreach (var node in root)
			{
				yield return createNode(parent, node);
				var children = getChildren(node);
				if (children != null)
				{
					foreach (var child in TreeWalkImpl(children, getChildren, createNode, node))
					{
						yield return child;
					}
				}
			}
		}

		/// <summary />
		public delegate TNode CreateNodeDelegate<T, TNode>(T parent, T child);
		#endregion

		/// <summary>
		/// Indexes the specified source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<IndexedItem<T>> Index<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			return IndexImpl(source);
		}

		private static IEnumerable<IndexedItem<T>> IndexImpl<T>(IEnumerable<T> source)
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

		#region Distinct
		/// <summary>
		/// Returns distinct elements from a sequence by extracting a key from each element and using the default equality comparer to compare values.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <typeparam name="TKey">The type of the keys of the elements of source.</typeparam>
		/// <param name="source">The sequence to remove duplicate elements from.</param>
		/// <param name="keyExtractor">A function that returns the key of a given item.</param>
		/// <returns></returns>
		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keyExtractor)
		{
			return source.Distinct(keyExtractor, EqualityComparer<TKey>.Default);
		}

		/// <summary>
		/// Returns distinct elements from a sequence by extracting a key from each element and using the specified equality comparer to compare values.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <typeparam name="TKey">The type of the keys of the elements of source.</typeparam>
		/// <param name="source">The sequence to remove duplicate elements from.</param>
		/// <param name="keyExtractor">A function that returns the key of a given item.</param>
		/// <param name="comparer">An System.Collections.Generic.IEqualityComparer{TKey} to compare keys.</param>
		/// <returns></returns>
		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keyExtractor, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (keyExtractor == null)
			{
				throw new ArgumentNullException("keyExtractor");
			}

			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			return DistinctImpl(source, keyExtractor, comparer);
		}

		private static IEnumerable<T> DistinctImpl<T, TKey>(IEnumerable<T> source, Func<T, TKey> keyExtractor, IEqualityComparer<TKey> comparer)
		{
			var visited = new HashSet<TKey>(comparer);
			foreach (var item in source)
			{
				if (visited.Add(keyExtractor(item)))
				{
					yield return item;
				}
			}
		}
		#endregion

		#region AddRange
		/// <summary>
		/// Adds all the elements from <paramref name="range"/> to <paramref name="collection"/>.
		/// </summary>
		/// <typeparam name="TCollection">The type of the collection.</typeparam>
		/// <typeparam name="TElement">The type of the element.</typeparam>
		/// <param name="collection">The collection where the elements are to be added.</param>
		/// <param name="range">The elements to be added to the collection.</param>
		/// <returns></returns>
		public static TCollection AddRange<TCollection, TElement>(this TCollection collection, IEnumerable<TElement> range)
			where TCollection : ICollection<TElement>
		{
			foreach (var item in range)
			{
				collection.Add(item);
			}
			return collection;
		}
		#endregion

		#region OuterJoin
		/// <summary>
		/// Correlates the elements of two sequences based on matching keys simmilarly to a "full outer join" in SQL.
		/// </summary>
		/// <typeparam name="TOuter">The type of the outer sequence elements.</typeparam>
		/// <typeparam name="TInner">The type of the inner sequence elements.</typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TResult">The type of the resulting sequence elements.</typeparam>
		/// <param name="outer">The outer sequence.</param>
		/// <param name="inner">The inner sequence.</param>
		/// <param name="outerKeySelector">The outer key selector.</param>
		/// <param name="innerKeySelector">The inner key selector.</param>
		/// <param name="uniqueOuterSelector">A selector that is invoked for each element from <paramref name="outer"/> that is not present in <paramref name="inner"/>.</param>
		/// <param name="uniqueInnerSelector">A selector that is invoked for each element from <paramref name="inner"/> that is not present in <paramref name="outer"/>.</param>
		/// <param name="matchSelector">A selector that is invoked for each element from <paramref name="inner"/> that is also present in <paramref name="outer"/>.</param>
		/// <param name="comparer">The comparer used for key comparisons. Defaults to EqualityComparer&lt;TKey&gt;.Default.</param>
		/// <returns>Returns a sequence of the results of <paramref name="uniqueOuterSelector"/>, <paramref name="uniqueInnerSelector"/> and <paramref name="matchSelector"/>.</returns>
		public static IEnumerable<TResult> OuterJoin<TOuter, TInner, TKey, TResult>(
			this IEnumerable<TOuter> outer,
			IEnumerable<TInner> inner,
			Func<TOuter, TKey> outerKeySelector,
			Func<TInner, TKey> innerKeySelector,
			Func<TOuter, TResult> uniqueOuterSelector,
			Func<TInner, TResult> uniqueInnerSelector,
			Func<TOuter, TInner, TResult> matchSelector,
			IEqualityComparer<TKey> comparer = null
		)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}

			var lookup = inner
				.GroupBy(innerKeySelector, comparer)
				.ToDictionary(g => new MergeKey<TKey>(g.Key), new MergeKeyComparer<TKey>(comparer));

			foreach (var outerItem in outer)
			{
				var key = outerKeySelector(outerItem);
				IGrouping<TKey, TInner> innerItems;
				if (lookup.TryGetValue(new MergeKey<TKey>(key), out innerItems))
				{
					foreach (var innerItem in innerItems)
					{
						yield return matchSelector(outerItem, innerItem);
					}
					lookup.Remove(new MergeKey<TKey>(key));
				}
				else
				{
					yield return uniqueOuterSelector(outerItem);
				}
			}

			foreach (var innerItem in lookup.SelectMany(g => g.Value))
			{
				yield return uniqueInnerSelector(innerItem);
			}
		}

		private sealed class MergeKeyComparer<TKey> : IEqualityComparer<MergeKey<TKey>>
		{
			private readonly IEqualityComparer<TKey> _comparer;

			public MergeKeyComparer(IEqualityComparer<TKey> comparer)
			{
				_comparer = comparer;
			}

			bool IEqualityComparer<MergeKey<TKey>>.Equals(MergeKey<TKey> x, MergeKey<TKey> y)
			{
				return ReferenceEquals(x.Key, null)
					? ReferenceEquals(y.Key, null)
					: !ReferenceEquals(y.Key, null) && _comparer.Equals(x.Key, y.Key);
			}

			int IEqualityComparer<MergeKey<TKey>>.GetHashCode(MergeKey<TKey> obj)
			{
				return ReferenceEquals(obj.Key, null) ? 0 : _comparer.GetHashCode(obj.Key);
			}
		}

		private struct MergeKey<TKey>
		{
			public readonly TKey Key;

			public MergeKey(TKey key)
			{
				Key = key;
			}
		}
		#endregion

		/// <summary />
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> sequence)
		{
			return sequence ?? Enumerable.Empty<T>();
		}
	}
}