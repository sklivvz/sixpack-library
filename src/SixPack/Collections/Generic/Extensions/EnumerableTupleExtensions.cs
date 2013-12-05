// EnumerableTupleExtensions.cs
//
//  Copyright (C) 2013 Antoine Aubry
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
    public static class EnumerableTupleExtensions
    {
		#if NET_40

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2>(this IEnumerable<Tuple<T1, T2>> sequence, Action<T1, T2> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, TResult>(this IEnumerable<Tuple<T1, T2>> sequence, Func<T1, T2, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2);
			}
		}

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> sequence, Action<T1, T2, T3> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2, item.Item3);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, T3, TResult>(this IEnumerable<Tuple<T1, T2, T3>> sequence, Func<T1, T2, T3, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2, item.Item3);
			}
		}

		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2>> first, IEnumerable<T3> second)
		{
			return first.Zip(second, (item, last) => Tuple.Create(item.Item1, item.Item2, last));
		}

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> sequence, Action<T1, T2, T3, T4> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2, item.Item3, item.Item4);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, T3, T4, TResult>(this IEnumerable<Tuple<T1, T2, T3, T4>> sequence, Func<T1, T2, T3, T4, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2, item.Item3, item.Item4);
			}
		}

		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3>> first, IEnumerable<T4> second)
		{
			return first.Zip(second, (item, last) => Tuple.Create(item.Item1, item.Item2, item.Item3, last));
		}

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4, T5>> sequence, Action<T1, T2, T3, T4, T5> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, TResult>(this IEnumerable<Tuple<T1, T2, T3, T4, T5>> sequence, Func<T1, T2, T3, T4, T5, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
			}
		}

		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4>> first, IEnumerable<T5> second)
		{
			return first.Zip(second, (item, last) => Tuple.Create(item.Item1, item.Item2, item.Item3, item.Item4, last));
		}

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2, T3, T4, T5, T6>(this IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> sequence, Action<T1, T2, T3, T4, T5, T6> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(this IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> sequence, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6);
			}
		}

		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> Zip<T1, T2, T3, T4, T5, T6>(this IEnumerable<Tuple<T1, T2, T3, T4, T5>> first, IEnumerable<T6> second)
		{
			return first.Zip(second, (item, last) => Tuple.Create(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, last));
		}

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1, T2, T3, T4, T5, T6, T7>(this IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> sequence, Action<T1, T2, T3, T4, T5, T6, T7> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6, item.Item7);
			}
		}

		/// <summary>
		/// performs a projection on a sequence.
		/// </summary>
		public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(this IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> sequence, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
		{
			foreach (var item in sequence)
			{
				yield return selector(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6, item.Item7);
			}
		}

		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> Zip<T1, T2, T3, T4, T5, T6, T7>(this IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> first, IEnumerable<T7> second)
		{
			return first.Zip(second, (item, last) => Tuple.Create(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6, last));
		}


		/// <summary>
		/// Creates a new sequence of tuples containing the corresponding elements of two sequences.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
		{
			return first.Zip(second, (f, s) => Tuple.Create(f, s));
		}

		#endif

		/// <summary>
		/// Executes the specified action for each element.
		/// </summary>
		public static void ForAll<T1>(this IEnumerable<T1> sequence, Action<T1> elementProcessor)
		{
			foreach (var item in sequence)
			{
				elementProcessor(item);
			}
		}
	}
}
