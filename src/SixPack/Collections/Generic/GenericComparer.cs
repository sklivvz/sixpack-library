using System;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Generic implementation of <see cref="IComparer{T}"/> based on the <see cref="IComparable{T}"/> interface.
	/// </summary>
	/// <typeparam name="TCompared">The type of the compared objects.</typeparam>
	public sealed class GenericComparer<TCompared> : IComparer<TCompared> where TCompared : IComparable<TCompared>
	{
		private readonly ComparisonOrder order;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericComparer&lt;TCompared&gt;"/> class.
		/// </summary>
		/// <param name="order">The order of the comparison.</param>
		public GenericComparer(ComparisonOrder order)
		{
			this.order = order;
		}

		#region IComparer<TCompared> Members
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
		/// </returns>
		public int Compare(TCompared x, TCompared y)
		{
			return GenericComparer.Compare(x, y, order);
		}
		#endregion

		/// <summary>
		/// Gets an <see cref="IComparer{T}"/> that order elements in natural order.
		/// </summary>
		public static IComparer<TCompared> Natural
		{
			get
			{
				return new GenericComparer<TCompared>(ComparisonOrder.Natural);
			}
		}

		/// <summary>
		/// Gets an <see cref="IComparer{T}"/> that order elements in inverse order.
		/// </summary>
		public static IComparer<TCompared> Reverse
		{
			get
			{
				return new GenericComparer<TCompared>(ComparisonOrder.Reverse);
			}
		}
	}

	/// <summary>
	/// Generic methods for comparing two objects that implement the <see cref="IComparable{T}"/> interface.
	/// </summary>
	public static class GenericComparer
	{
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <param name="order">The order of the comparison.</param>
		/// <returns>
		/// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
		/// </returns>
		public static int Compare<TCompared>(TCompared x, TCompared y, ComparisonOrder order) where TCompared : IComparable<TCompared>
		{
			int result = x.CompareTo(y);
			return order == ComparisonOrder.Natural ? result : -result;
		}
	}

	/// <summary>
	/// Specifies the order of a comparison.
	/// </summary>
	public enum ComparisonOrder
	{
		/// <summary>
		/// Uses the natural ordering of the items.
		/// </summary>
		Natural,

		/// <summary>
		/// Uses the reverse ordering of the items.
		/// </summary>
		Reverse,
	}
}