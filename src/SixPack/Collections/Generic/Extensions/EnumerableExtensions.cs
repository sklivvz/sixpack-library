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
	}
}