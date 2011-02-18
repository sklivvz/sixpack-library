// IndexedItem.cs
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

namespace SixPack.Collections.Generic.Extensions
{
	/// <summary>
	/// Contains additional information about an item from a sequence-
	/// </summary>
	public struct IndexedItem<T>
	{
		private readonly T _item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public T Item
		{
			get
			{
				return _item;
			}
		}

		private readonly int _index;

		/// <summary>
		/// Gets or sets the index of the item in the sequence.
		/// </summary>
		/// <value>The index.</value>
		public int Index
		{
			get
			{
				return _index;
			}
		}

		private readonly bool _isFirst;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is the first one in the sequence.
		/// </summary>
		/// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
		public bool IsFirst
		{
			get
			{
				return _isFirst;
			}
		}

		private readonly bool _isLast;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is the last one in the sequence.
		/// </summary>
		/// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
		public bool IsLast
		{
			get
			{
				return _isLast;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexedItem&lt;T&gt;"/> struct.
		/// </summary>
		internal IndexedItem(T item, int index, bool isFirst, bool isLast)
		{
			_item = item;
			_index = index;
			_isFirst = isFirst;
			_isLast = isLast;
		}
	}
}