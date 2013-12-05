// AggregateExtensions.cs
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
	/// <summary/>
	public static class AggregateExtensions
	{

		/// <summary>
		/// Returns the maximum value in a sequence of decimal values.
		/// </summary>
		/// <param name="source">A sequence of decimal values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static decimal Max(this IEnumerable<decimal> source, decimal defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of decimal values.
		/// </summary>
		/// <param name="source">A sequence of decimal values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static decimal Min(this IEnumerable<decimal> source, decimal defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum decimal value.
		/// </summary>
		/// <param name="source">A sequence of decimal values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum decimal value.
		/// </summary>
		/// <param name="source">A sequence of decimal values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of decimal values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, decimal> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of decimal values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, decimal> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of decimal? values.
		/// </summary>
		/// <param name="source">A sequence of decimal? values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static decimal? Max(this IEnumerable<decimal?> source, decimal? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of decimal? values.
		/// </summary>
		/// <param name="source">A sequence of decimal? values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static decimal? Min(this IEnumerable<decimal?> source, decimal? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum decimal? value.
		/// </summary>
		/// <param name="source">A sequence of decimal? values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum decimal? value.
		/// </summary>
		/// <param name="source">A sequence of decimal? values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of decimal? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, decimal?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of decimal? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, decimal?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of double values.
		/// </summary>
		/// <param name="source">A sequence of double values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static double Max(this IEnumerable<double> source, double defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of double values.
		/// </summary>
		/// <param name="source">A sequence of double values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static double Min(this IEnumerable<double> source, double defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum double value.
		/// </summary>
		/// <param name="source">A sequence of double values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum double value.
		/// </summary>
		/// <param name="source">A sequence of double values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of double values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, double> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of double values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, double> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of double? values.
		/// </summary>
		/// <param name="source">A sequence of double? values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static double? Max(this IEnumerable<double?> source, double? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of double? values.
		/// </summary>
		/// <param name="source">A sequence of double? values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static double? Min(this IEnumerable<double?> source, double? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum double? value.
		/// </summary>
		/// <param name="source">A sequence of double? values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum double? value.
		/// </summary>
		/// <param name="source">A sequence of double? values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of double? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, double?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of double? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, double?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of long values.
		/// </summary>
		/// <param name="source">A sequence of long values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static long Max(this IEnumerable<long> source, long defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of long values.
		/// </summary>
		/// <param name="source">A sequence of long values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static long Min(this IEnumerable<long> source, long defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum long value.
		/// </summary>
		/// <param name="source">A sequence of long values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum long value.
		/// </summary>
		/// <param name="source">A sequence of long values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of long values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, long> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of long values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, long> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of long? values.
		/// </summary>
		/// <param name="source">A sequence of long? values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static long? Max(this IEnumerable<long?> source, long? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of long? values.
		/// </summary>
		/// <param name="source">A sequence of long? values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static long? Min(this IEnumerable<long?> source, long? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum long? value.
		/// </summary>
		/// <param name="source">A sequence of long? values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum long? value.
		/// </summary>
		/// <param name="source">A sequence of long? values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of long? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, long?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of long? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, long?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of int values.
		/// </summary>
		/// <param name="source">A sequence of int values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static int Max(this IEnumerable<int> source, int defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of int values.
		/// </summary>
		/// <param name="source">A sequence of int values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static int Min(this IEnumerable<int> source, int defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum int value.
		/// </summary>
		/// <param name="source">A sequence of int values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum int value.
		/// </summary>
		/// <param name="source">A sequence of int values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of int values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, int> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of int values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, int> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of int? values.
		/// </summary>
		/// <param name="source">A sequence of int? values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static int? Max(this IEnumerable<int?> source, int? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of int? values.
		/// </summary>
		/// <param name="source">A sequence of int? values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static int? Min(this IEnumerable<int?> source, int? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum int? value.
		/// </summary>
		/// <param name="source">A sequence of int? values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, int? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum int? value.
		/// </summary>
		/// <param name="source">A sequence of int? values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, int? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of int? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, int?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of int? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, int?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of float values.
		/// </summary>
		/// <param name="source">A sequence of float values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static float Max(this IEnumerable<float> source, float defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of float values.
		/// </summary>
		/// <param name="source">A sequence of float values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static float Min(this IEnumerable<float> source, float defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum float value.
		/// </summary>
		/// <param name="source">A sequence of float values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum float value.
		/// </summary>
		/// <param name="source">A sequence of float values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of float values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, float> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of float values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, float> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a sequence of float? values.
		/// </summary>
		/// <param name="source">A sequence of float? values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static float? Max(this IEnumerable<float?> source, float? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minumim value in a sequence of float? values.
		/// </summary>
		/// <param name="source">A sequence of float? values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static float? Min(this IEnumerable<float?> source, float? defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum float? value.
		/// </summary>
		/// <param name="source">A sequence of float? values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the minimum float? value.
		/// </summary>
		/// <param name="source">A sequence of float? values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float? defaultValue)
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the greatest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of float? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, float?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey > bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}

		/// <summary>
		/// Returns the value in a sequence whose key is the smallest.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
		/// <param name="source">A sequence of float? values to determine the maximum value of.</param>
		/// <param name="keyExtractor">A function that extracts the key of a value.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static T MinBy<T>(this IEnumerable<T> source, Func<T, float?> keyExtractor, T defaultValue = default(T))
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					var bestValue = enumerator.Current;
					var bestKey = keyExtractor(bestValue);

					while (enumerator.MoveNext())
					{
						var currentKey = keyExtractor(enumerator.Current);
						if (currentKey < bestKey)
						{
							bestValue = enumerator.Current;
							bestKey = currentKey;
						}
					}

					return bestValue;
				}

				return defaultValue;
			}
		}
		/// <summary>
		/// Returns the maximum value in a generic sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The maximum value in the sequence.</returns>
		public static TSource Max<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		/// Returns the minimum value in a generic sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value that is returned if the sequence is empty.</param>
		/// <returns>The minimum value in the sequence.</returns>
		public static TSource Min<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

	}
}