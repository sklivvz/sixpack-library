// HashCodeBuilder.cs
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

using System.Collections;

namespace SixPack
{
	/// <summary>
	/// Provides a fluent interface for generating a hash code based on multiple fields.
	/// </summary>
	public struct HashCodeBuilder
	{
		private int _hashCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="HashCodeBuilder"/> struct.
		/// </summary>
		/// <param name="hashCode">The hash code.</param>
		internal HashCodeBuilder(int hashCode)
		{
			_hashCode = hashCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashCodeBuilder"/> struct.
		/// </summary>
		internal HashCodeBuilder(object value)
			: this(SafeGetHashCode(value))
		{
		}

		/// <summary>
		/// Gets a hash code for the specified object.
		/// </summary>
		private static int SafeGetHashCode(object value)
		{
			return value != null ? value.GetHashCode() : 0;
		}

		/// <summary>
		/// Adds the specified hash code.
		/// </summary>
		/// <param name="hashCode">The hash code.</param>
		/// <returns></returns>
		public HashCodeBuilder Add(int hashCode)
		{
			_hashCode = HashCode.CombineHashCodes(_hashCode, hashCode);
			return this;
		}

		/// <summary>
		/// Adds the hash code of the specified object.
		/// </summary>
		/// <returns></returns>
		public HashCodeBuilder Add(object value)
		{
			return Add(SafeGetHashCode(value));
		}

		/// <summary>
		/// Adds the hash code of the specified objects.
		/// </summary>
		public HashCodeBuilder AddRange(IEnumerable values)
		{
			foreach (var item in values)
			{
				Add(item);
			}
			return this;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="HashCodeBuilder"/> to <see cref="System.Int32"/>.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator int(HashCodeBuilder builder)
		{
			return builder._hashCode;
		}
	}
}