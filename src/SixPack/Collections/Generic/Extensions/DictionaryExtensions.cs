// DictionaryExtensions.cs
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
using System.Collections.ObjectModel;

namespace SixPack.Collections.Generic.Extensions
{
	/// <summary>
	/// Extension methods for the <see cref="System.Collections.Generic.IDictionary{TK, TV}"/> type.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <remarks>
		/// This method is simmilar to IDictionary.TryGetValue(key, out value), except that
		/// it does not use an output parameter. This is useful when one needs to use type
		/// inference.
		/// </remarks>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static DictionaryLookupResult<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue result;
			bool found = dictionary.TryGetValue(key, out result);
			return new DictionaryLookupResult<TValue>(found, result);
		}

		/// <summary>
		/// If the dictionary contains the specified key, returns the value associated to that key; otherwise returns default(TValue).
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			return ValueOrDefault(dictionary, key, default(TValue));
		}

		/// <summary>
		/// If the dictionary contains the specified key, returns the value associated to that key; otherwise returns default(TValue).
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : defaultValue;
		}

		/// <summary>
		/// Adds a value to a dictionary of collections by adding the value to the collection that is associated to the
		/// specified key. If no collection is associated to the key, a new collection is added and associated.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <typeparam name="TCollection">The type of the collection.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public static void Add<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> dictionary, TKey key, TValue value)
			where TCollection : class, ICollection<TValue>, new()
		{
			TCollection keyItems;
			if (!dictionary.TryGetValue(key, out keyItems))
			{
				keyItems = new TCollection();
				dictionary.Add(key, keyItems);
			}
			keyItems.Add(value);
		}

		/// <summary>
		/// Returns a read-only wrapper of the specified dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns></returns>
		public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			return new ReadOnlyDictionary<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Returns a read-only wrapper of the specified list.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the list.</typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static IList<T> AsReadOnly<T>(this IList<T> list)
		{
			return new ReadOnlyCollection<T>(list);
		}
	}
}