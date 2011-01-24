// CompositeKey.cs
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
using System.Text;
using SixPack.Text;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// A class that can be used as a composite key for a dictionary or another simmilar data structure.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CompositeKey<T> : List<T>
	{
		private readonly IList<string> _descriptor;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeKey&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="descriptor">The descriptor.</param>
		public CompositeKey(IList<string> descriptor = null)
		{
			_descriptor = descriptor;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeKey&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="descriptor">The descriptor.</param>
		public CompositeKey(IEnumerable<T> values, IList<string> descriptor = null)
			: base(values)
		{
			_descriptor = descriptor;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeKey&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		public CompositeKey(params T[] values)
			: base(values)
		{
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return HashCode.AddRange(this);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			var other = obj as CompositeKey<T>;
			if (ReferenceEquals(other, null))
			{
				return false;
			}

			if (Count != other.Count)
			{
				return false;
			}

			for (int i = 0; i < Count; i++)
			{
				var mine = this[i];
				var theirs = other[i];
				if (mine == null)
				{
					if (theirs != null)
					{
						return false;
					}
				}
				else if (!mine.Equals(theirs))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(CompositeKey<T> first, CompositeKey<T> second)
		{
			if (ReferenceEquals(first, null))
			{
				return ReferenceEquals(second, null);
			}
			else
			{
				return first.Equals(second);
			}
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(CompositeKey<T> first, CompositeKey<T> second)
		{
			return !(first == second);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (_descriptor != null)
			{
				return this
					.Zip(_descriptor, (value, name) => string.Format("{0}: {1}", name, value))
					.DelimitWith(prefix: "[", suffix: "]");
			}
			else
			{
				return this.DelimitWith(prefix: "[", suffix: "]");
			}
		}

		/// <summary>
		/// Performs an implicit conversion from T[] to <see cref="SixPack.Collections.Generic.CompositeKey&lt;T&gt;"/>.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator CompositeKey<T>(T[] values)
		{
			return new CompositeKey<T>(values);
		}

		/// <summary>
		/// Gets the index of the specified column.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public int IndexOfColumn(string columnName)
		{
			if (_descriptor == null)
			{
				throw new InvalidOperationException("The composite key does not have column descriptors. Therefore, it cannot be indexed by name.");
			}
			for (int i = 0; i < _descriptor.Count; i++)
			{
				if (_descriptor[i] == columnName)
				{
					return i;
				}
			}
			throw new KeyNotFoundException(string.Format("The given key [{0}] was not present in the composite key descriptor.", columnName));
		}

		/// <summary>
		/// Gets the value the specified column.
		/// </summary>
		/// <value></value>
		public T this[string columnName]
		{
			get
			{
				return this[IndexOfColumn(columnName)];
			}
		}
	}

}
