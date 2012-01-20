// Trie.cs 
//
//  Copyright (C) 2012 Antoine Aubry
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
using System.Collections;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Generic implementation of a trie, or prefix tree (http://en.wikipedia.org/wiki/Trie).
	/// </summary>
	/// <typeparam name="TAtom">The type of the atom.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	public class Trie<TAtom, TValue>
	{
		private readonly ITrieDomain<TAtom> domain;
		private readonly TrieNode root;

		/// <summary>
		/// Initializes a new instance of the <see cref="Trie&lt;TAtom, TValue&gt;"/> class.
		/// </summary>
		/// <param name="domain">The domain of the atoms.</param>
		public Trie(ITrieDomain<TAtom> domain)
		{
			this.domain = domain;
			root = new TrieNode();
		}

		/// <summary>
		/// Adds the specified key-value pair to the trie.
		/// </summary>
		public void Add(IEnumerable<TAtom> key, TValue value)
		{
			TrieNode current = root;
			foreach (TAtom atom in key)
			{
				TrieNode[] children = current.Children;
				if (children == null)
				{
					children = new TrieNode[domain.Size];
					current.Children = children;
				}

				int index = domain.MapFromDomain(atom);
				TrieNode next = children[index];
				if (next == null)
				{
					next = new TrieNode();
					children[index] = next;
				}
				current = next;
			}

			if (current.Values == null)
			{
				current.Values = new List<TValue>();
			}
			current.Values.Add(value);
		}

		/// <summary>
		/// Searches for all the key-value pairs whose key starts with the specified prefix.
		/// </summary>
		public ITrieEnumerator<TAtom, TValue> PrefixMatch(IEnumerable<TAtom> prefix)
		{
			TrieNode current = root;

			foreach (TAtom atom in prefix)
			{
				int index = domain.MapFromDomain(atom);
				if (current.Children == null)
				{
					return new EmptyEnumerator();
				}

				current = current.Children[index];
				if (current == null)
				{
					return new EmptyEnumerator();
				}
			}

			return new ValuesEnumerator(new NodeEnumerator(new TrieNodeCursor(domain, current.Children, -1)));
		}

		/// <summary>
		/// Gets all the key-value pairs currently in the trie.
		/// </summary>
		public ITrieEnumerator<TAtom, TValue> All
		{
			get
			{
				return new ValuesEnumerator(new NodeEnumerator(new TrieNodeCursor(domain, root.Children, -1)));
			}
		}

		#region TrieNode
		private sealed class TrieNode
		{
			public TrieNode[] Children;
			public List<TValue> Values;
		}
		#endregion

		#region TrieNodeCursor
		private sealed class TrieNodeCursor
		{
			private class CursorPathComponent
			{
				public TrieNode[] Children;
				public int ChildIndex;
			}

			private readonly ITrieDomain<TAtom> domain;
			private readonly Stack<CursorPathComponent> path = new Stack<CursorPathComponent>();

			public TrieNodeCursor(ITrieDomain<TAtom> domain, TrieNode[] currentChildren, int childIndex)
			{
				this.domain = domain;

				CursorPathComponent currentPath = new CursorPathComponent();
				currentPath.Children = currentChildren;
				currentPath.ChildIndex = childIndex;
				path.Push(currentPath);
			}

			public TrieNode Current
			{
				get
				{
					CursorPathComponent currentLocation = path.Peek();
					TrieNode[] children = currentLocation.Children;
					if (children != null)
					{
						return children[currentLocation.ChildIndex];
					}
					return null;
				}
			}

			public IEnumerable<TAtom> CurrentKey
			{
				get
				{
					TAtom[] key = new TAtom[path.Count];

					int index = key.Length;
					foreach (CursorPathComponent location in path)
					{
						key[--index] = domain.MapToDomain(location.ChildIndex);
					}

					return key;
				}
			}

			public bool MoveLeft()
			{
				CursorPathComponent currentLocation = path.Peek();
				int newIndex = currentLocation.ChildIndex;

				TrieNode[] children = currentLocation.Children;
				if (children != null)
				{
					while (--newIndex >= 0)
					{
						if (children[newIndex] != null)
						{
							currentLocation.ChildIndex = newIndex;
							return true;
						}
					}
					currentLocation.ChildIndex = newIndex;
				}
				return false;
			}

			public bool MoveRight()
			{
				CursorPathComponent currentLocation = path.Peek();
				int newIndex = currentLocation.ChildIndex;

				TrieNode[] children = currentLocation.Children;
				if (children != null)
				{
					while (++newIndex < children.Length)
					{
						if (children[newIndex] != null)
						{
							currentLocation.ChildIndex = newIndex;
							return true;
						}
					}
					currentLocation.ChildIndex = newIndex;
				}
				return false;
			}

			public bool MoveUp()
			{
				if (path.Count > 1)
				{
					path.Pop();
					return true;
				}
				return false;
			}

			public bool MoveDown(bool toFirstChild)
			{
				CursorPathComponent currentLocation = path.Peek();

				if (currentLocation.ChildIndex >= 0)
				{
					TrieNode currentChild = currentLocation.Children[currentLocation.ChildIndex];

					if (currentChild.Children != null)
					{
						CursorPathComponent newLocation = new CursorPathComponent();
						newLocation.Children = currentChild.Children;

						if (toFirstChild)
						{
							newLocation.ChildIndex = -1;
							path.Push(newLocation);
							MoveRight();
						}
						else
						{
							newLocation.ChildIndex = currentChild.Children.Length;
							path.Push(newLocation);
							MoveLeft();
						}

						return true;
					}
				}
				return false;
			}
		}
		#endregion

		#region NodeEnumerator
		private sealed class NodeEnumerator : ITrieEnumerator<TAtom, IList<TValue>>
		{
			private readonly TrieNodeCursor cursor;

			public NodeEnumerator(TrieNodeCursor cursor)
			{
				this.cursor = cursor;
			}

			public KeyValuePair<IEnumerable<TAtom>, IList<TValue>> Current
			{
				get { return new KeyValuePair<IEnumerable<TAtom>, IList<TValue>>(cursor.CurrentKey, cursor.Current.Values); }
			}

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public bool MoveNext()
			{
				if (cursor.MoveDown(true))
				{
					return true;
				}

				do
				{
					if (cursor.MoveRight())
					{
						return true;
					}
				} while (cursor.MoveUp());

				return false;
			}

			public bool MovePrevious()
			{
				if (cursor.MoveLeft())
				{
					while (cursor.MoveDown(false)) ;
					return true;
				}

				return cursor.MoveUp();
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}
		#endregion

		#region ValuesEnumerator
		private sealed class ValuesEnumerator : ITrieEnumerator<TAtom, TValue>
		{
			private readonly NodeEnumerator enumerator;
			private int currentValueIndex = -1;

			public ValuesEnumerator(NodeEnumerator enumerator)
			{
				this.enumerator = enumerator;
			}

			public KeyValuePair<IEnumerable<TAtom>, TValue> Current
			{
				get
				{
					KeyValuePair<IEnumerable<TAtom>, IList<TValue>> current = enumerator.Current;
					return new KeyValuePair<IEnumerable<TAtom>, TValue>(current.Key, current.Value[currentValueIndex]);
				}
			}

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public bool MoveNext()
			{
				if (currentValueIndex >= 0)
				{
					if (++currentValueIndex < enumerator.Current.Value.Count)
					{
						return true;
					}
				}

				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Value != null)
					{
						currentValueIndex = 0;
						return true;
					}
				}

				currentValueIndex = -1;

				return false;
			}

			public bool MovePrevious()
			{
				if (currentValueIndex >= 0)
				{
					if (--currentValueIndex >= 0)
					{
						return true;
					}
				}

				while (enumerator.MovePrevious())
				{
					IList<TValue> values = enumerator.Current.Value;
					if (values != null)
					{
						currentValueIndex = values.Count - 1;
						return true;
					}
				}

				currentValueIndex = -1;

				return false;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}
		#endregion

		#region EmptyEnumerator
		private sealed class EmptyEnumerator : ITrieEnumerator<TAtom, TValue>
		{
			public KeyValuePair<IEnumerable<TAtom>, TValue> Current
			{
				get { throw new InvalidOperationException(); }
			}

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public bool MoveNext()
			{
				return false;
			}

			public bool MovePrevious()
			{
				return false;
			}

			public void Reset()
			{
			}
		}
		#endregion
	}
}