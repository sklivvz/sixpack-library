using System;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Implementation of <see cref="IEqualityComparer{T}"/> based on lambda expressions.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _equals;
		private readonly Func<T, int> _getHashCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="LambdaEqualityComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="equals">The Equals() implementation.</param>
		/// <param name="getHashCode">The GetHashCode() implementation.</param>
		public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
		{
			_equals = equals;
			_getHashCode = getHashCode;
		}

		#region IEqualityComparer<T> Members

		/// <summary/>
		public bool Equals(T x, T y)
		{
			return _equals(x, y);
		}

		/// <summary/>
		public int GetHashCode(T obj)
		{
			return _getHashCode(obj);
		}

		#endregion
	}

	/// <summary>
	/// Factory to create instances of <see cref="LambdaEqualityComparer{T}"/>
	/// </summary>
	public static class LambdaEqualityComparer
	{
		/// <summary>
		/// Creates a new instance of the <see cref="LambdaEqualityComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="equals">The Equals() implementation.</param>
		/// <param name="getHashCode">The GetHashCode() implementation.</param>
		public static LambdaEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
		{
			return  new LambdaEqualityComparer<T>(equals, getHashCode);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="LambdaEqualityComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="getKey">A function that extracts the key of an object.</param>
		/// <returns></returns>
		public static LambdaEqualityComparer<TCompared> Create<TCompared, TKey>(Func<TCompared, TKey> getKey)
		{
			return new LambdaEqualityComparer<TCompared>((x, y) => getKey(x).Equals(getKey(y)), obj => getKey(obj).GetHashCode());
		}
	}
}
