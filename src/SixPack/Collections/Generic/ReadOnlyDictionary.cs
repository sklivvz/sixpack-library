using System;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Wraps an <see cref="IDictionary{TKey, TValue}"/> to allow read-only access.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	public sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _wrapped;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyDictionary&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="wrapped">The wrapped dictionary.</param>
		public ReadOnlyDictionary(IDictionary<TKey, TValue> wrapped)
		{
			_wrapped = wrapped;
		}

		#region IDictionary<TKey,TValue> Members

		/// <summary />
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			throw new NotSupportedException();
		}

		/// <summary />
		public bool ContainsKey(TKey key)
		{
			return _wrapped.ContainsKey(key);
		}

		/// <summary />
		public ICollection<TKey> Keys
		{
			get { return _wrapped.Keys; }
		}

		/// <summary />
		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			throw new NotSupportedException();
		}

		/// <summary />
		public bool TryGetValue(TKey key, out TValue value)
		{
			return _wrapped.TryGetValue(key, out value);
		}

		/// <summary />
		public ICollection<TValue> Values
		{
			get { return _wrapped.Values; }
		}

		/// <summary />
		public TValue this[TKey key]
		{
			get
			{
				return _wrapped[key];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		/// <summary />
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		/// <summary />
		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary />
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _wrapped.Contains(item);
		}

		/// <summary />
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_wrapped.CopyTo(array, arrayIndex);
		}

		/// <summary />
		public int Count
		{
			get { return _wrapped.Count; }
		}

		/// <summary />
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary />
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		/// <summary />
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _wrapped.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary />
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _wrapped.GetEnumerator();
		}

		#endregion
	}
}
