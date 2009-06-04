// Pair.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
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

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// A class that holds a pair of objects.
	/// </summary>
	/// <remarks>
	/// This class implements <see cref="Equals(object)"/> and <see cref="GetHashCode"/> in a
	/// way that makes it suitable for usage as a key in an <see cref="IDictionary{TKey,TValue}"/>.
	/// </remarks>
	/// <typeparam name="TFirst">The type of the first object.</typeparam>
	/// <typeparam name="TSecond">The type of the second object.</typeparam>
	public class Pair<TFirst, TSecond>
	{
		private TFirst first;

		/// <summary>
		/// Gets or sets the first object.
		/// </summary>
		/// <value>The first object.</value>
		public TFirst First
		{
			get
			{
				return first;
			}
			set
			{
				first = value;
			}
		}

		private TSecond second;

		/// <summary>
		/// Gets or sets the second object.
		/// </summary>
		/// <value>The second object.</value>
		public TSecond Second
		{
			get
			{
				return second;
			}
			set
			{
				second = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair&lt;TFirst, TSecond&gt;"/> class.
		/// </summary>
		public Pair()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair&lt;TFirst, TSecond&gt;"/> class.
		/// </summary>
		/// <param name="first">The first object.</param>
		/// <param name="second">The second object.</param>
		public Pair(TFirst first, TSecond second)
		{
			this.first = first;
			this.second = second;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			Pair<TFirst, TSecond> other = obj as Pair<TFirst, TSecond>;
			if(other == null)
			{
				return false;
			}

			// If the types are value types, comparing them with null will allways return false,
			// which is what we want.
			// ReSharper disable CompareNonConstrainedGenericWithNull
			bool firstAreEqual = first == null ? other.first == null : first.Equals(other.first);
			bool secondAreEqual = second == null ? other.second == null : second.Equals(other.second);
			// ReSharper restore CompareNonConstrainedGenericWithNull

			return firstAreEqual && secondAreEqual;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			// If the types are value types, comparing them with null will allways return false,
			// which is what we want.
			// ReSharper disable CompareNonConstrainedGenericWithNull
			int firstHashCode = first == null ? 0 : first.GetHashCode();
			int secondHashCode = second == null ? 0 : second.GetHashCode();
			// ReSharper restore CompareNonConstrainedGenericWithNull

			return firstHashCode ^ secondHashCode;
		}
	}

	/// <summary>
	/// Helper class that creates instances of pairs.
	/// </summary>
	public static class Pair
	{
		/// <summary>
		/// Creates a new <see cref="Pair{TFirst,TSecond}"/>.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first object.</typeparam>
		/// <typeparam name="TSecond">The type of the second object.</typeparam>
		/// <param name="first">The first object.</param>
		/// <param name="second">The second object.</param>
		/// <returns>Returns the <see cref="Pair{TFirst,TSecond}"/> that was created.</returns>
		public static Pair<TFirst, TSecond> New<TFirst, TSecond>(TFirst first, TSecond second)
		{
			return new Pair<TFirst, TSecond>(first, second);
		}
	}
}
