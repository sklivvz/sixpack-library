// HashCode.cs
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
using System.Collections;

namespace SixPack
{
	/// <summary>
	/// Supports implementations of <see cref="Object.GetHashCode"/> by providing methods to combine two hash codes.
	/// </summary>
	public static class HashCode
	{
		/// <summary>
		/// Combines two hash codes.
		/// </summary>
		/// <param name="h1">The first hash code.</param>
		/// <param name="h2">The second hash code.</param>
		/// <returns></returns>
		public static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2)
		{
			return CombineHashCodes(o1.GetHashCode(), o2.GetHashCode());
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3)
		{
			return CombineHashCodes(CombineHashCodes(o1.GetHashCode(), o2.GetHashCode()), o3.GetHashCode());
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3, object o4)
		{
			return CombineHashCodes(CombineHashCodes(o1.GetHashCode(), o2.GetHashCode()), CombineHashCodes(o3.GetHashCode(), o4.GetHashCode()));
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3, object o4, object o5)
		{
			return CombineHashCodes(Combine(o1, o2, o3, o4), o5.GetHashCode());
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3, object o4, object o5, object o6)
		{
			return CombineHashCodes(Combine(o1, o2, o3, o4), Combine(o5, o6));
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3, object o4, object o5, object o6, object o7)
		{
			return CombineHashCodes(Combine(o1, o2, o3, o4), Combine(o5, o6, o7));
		}

		/// <summary>
		/// Combines the hash codes of multiple objects.
		/// </summary>
		public static int Combine(object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8)
		{
			return CombineHashCodes(Combine(o1, o2, o3, o4), Combine(o5, o6, o7, o8));
		}

		/// <summary>
		/// Creates a <see cref="HashCodeBuilder"/> that can be used to generate a hash code using a fluent interface.
		/// </summary>
		/// <param name="hashCode">The initial hash code.</param>
		/// <returns></returns>
		public static HashCodeBuilder Add(int hashCode)
		{
			return new HashCodeBuilder(hashCode);
		}

		/// <summary>
		/// Creates a <see cref="HashCodeBuilder"/> that can be used to generate a hash code using a fluent interface.
		/// </summary>
		/// <param name="value">The object whose hash code will be the initial hash code.</param>
		/// <returns></returns>
		public static HashCodeBuilder Add(object value)
		{
			return new HashCodeBuilder(value);
		}

		/// <summary>
		/// Creates a <see cref="HashCodeBuilder"/> that can be used to generate a hash code using a fluent interface.
		/// </summary>
		public static HashCodeBuilder AddRange(IEnumerable values)
		{
			return new HashCodeBuilder().AddRange(values);
		}
	}
}