// TreeNode.cs
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

using System.Collections.Generic;

namespace SixPack.Collections.Generic.Extensions
{
	/// <summary>
	/// Default type used to build hierarchical trees.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TreeNode<T>
	{
		/// <summary>
		/// Gets or sets the parent of the current node.
		/// </summary>
		/// <value>The parent.</value>
		public TreeNode<T> Parent { get; set; }

		/// <summary>
		/// Gets or sets the children of the current node.
		/// </summary>
		/// <value>The children.</value>
		public IList<TreeNode<T>> Children { get; private set; }

		/// <summary>
		/// Gets or sets the value of the node.
		/// </summary>
		/// <value>The value.</value>
		public T Value { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeNode&lt;T&gt;"/> class.
		/// </summary>
		public TreeNode()
		{
			Children = new List<TreeNode<T>>();
		}

		/// <summary>
		/// Gets the level of depth of this node.
		/// </summary>
		/// <value>The level.</value>
		public int Level
		{
			get
			{
				return Parent != null ? Parent.Level + 1 : 0;
			}
		}

		/// <summary>
		/// Gets the previous sibling.
		/// </summary>
		/// <value>The previous sibling.</value>
		public TreeNode<T> PreviousSibling
		{
			get
			{
				if (Parent != null)
				{
					var siblingIndex = Parent.Children.IndexOf(this) - 1;
					if (siblingIndex >= 0)
					{
						return Parent.Children[siblingIndex];
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the next sibling.
		/// </summary>
		/// <value>The next sibling.</value>
		public TreeNode<T> NextSibling
		{
			get
			{
				if (Parent != null)
				{
					var siblingIndex = Parent.Children.IndexOf(this) + 1;
					if (Parent.Children.Count > siblingIndex)
					{
						return Parent.Children[siblingIndex];
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets an enumerator that returns all the nodes of the tree starting at this node.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TreeNode<T>> AllNodes()
		{
			return new[] { this }.AllNodes();
		}
	}

	/// <summary>
	/// Extension methods that simplyfy working with tree nodes.
	/// </summary>
	public static class TreeNodeExtensions
	{
		/// <summary>
		/// Gets an enumerator that returns all the nodes of the tree starting at the specified nodes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="nodes">The nodes.</param>
		/// <returns></returns>
		public static IEnumerable<TreeNode<T>> AllNodes<T>(this IEnumerable<TreeNode<T>> nodes)
		{
			foreach (var node in nodes)
			{
				yield return node;

				foreach (var childNode in node.Children.AllNodes())
				{
					yield return childNode;
				}
			}
		}
	}
}