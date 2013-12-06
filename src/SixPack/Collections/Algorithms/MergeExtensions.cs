// MergeExtensions.cs
//
//  Copyright (C) 2012 Antoine Aubry
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
using SixPack.Collections.Generic.Extensions;

namespace SixPack.Collections.Algorithms
{
	/// <summary>
	/// Extension methods that allow to merge two sequences.
	/// </summary>
	public static class MergeExtensions
	{
		/// <summary>
		/// Correlates the elements of two sequences based on matching keys simmilarly to a "full outer join" in SQL.
		/// For each results, executes an action
		/// </summary>
		/// <typeparam name="TOuter">The type of the outer sequence elements.</typeparam>
		/// <typeparam name="TInner">The type of the inner sequence elements.</typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="outer">The outer sequence.</param>
		/// <param name="inner">The inner sequence.</param>
		/// <param name="outerKeySelector">The outer key selector.</param>
		/// <param name="innerKeySelector">The inner key selector.</param>
		/// <param name="uniqueOuterProcessor">An action that is invoked for each element from <paramref name="outer"/> that is not present in <paramref name="inner"/>.</param>
		/// <param name="uniqueInnerProcessor">An action that is invoked for each element from <paramref name="inner"/> that is not present in <paramref name="outer"/>.</param>
		/// <param name="matchProcessor">An action that is invoked for each element from <paramref name="inner"/> that is also present in <paramref name="outer"/>.</param>
		/// <param name="comparer">The comparer used for key comparisons. Defaults to EqualityComparer&lt;TKey&gt;.Default.</param>
		public static void Merge<TOuter, TInner, TKey>(
			this IEnumerable<TOuter> outer,
			IEnumerable<TInner> inner,
			Func<TOuter, TKey> outerKeySelector,
			Func<TInner, TKey> innerKeySelector,
			Action<TOuter> uniqueOuterProcessor,
			Action<TInner> uniqueInnerProcessor,
			Action<TOuter, TInner> matchProcessor,
			IEqualityComparer<TKey> comparer = null
		)
		{
			outer
				.OuterJoin<TOuter, TInner, TKey, Action>(
					inner,
					outerKeySelector,
					innerKeySelector,
					o => () => uniqueOuterProcessor(o),
					i => () => uniqueInnerProcessor(i),
					(o, i) => () => matchProcessor(o, i),
					comparer
				)
				.ForAll(p => p());
		}

		/// <summary>
		/// Correlates the elements of two sequences based on matching keys simmilarly to a "full outer join" in SQL.
		/// For each results, executes an action
		/// </summary>
		/// <typeparam name="T">The type of the sequence elements.</typeparam>
		/// <param name="outer">The outer sequence.</param>
		/// <param name="inner">The inner sequence.</param>
		/// <param name="uniqueOuterProcessor">An action that is invoked for each element from <paramref name="outer"/> that is not present in <paramref name="inner"/>.</param>
		/// <param name="uniqueInnerProcessor">An action that is invoked for each element from <paramref name="inner"/> that is not present in <paramref name="outer"/>.</param>
		/// <param name="matchProcessor">An action that is invoked for each element from <paramref name="inner"/> that is also present in <paramref name="outer"/>.</param>
		/// <param name="comparer">The comparer used for key comparisons. Defaults to EqualityComparer&lt;TKey&gt;.Default.</param>
		public static void Merge<T>(
			this IEnumerable<T> outer,
			IEnumerable<T> inner,
			Action<T> uniqueOuterProcessor,
			Action<T> uniqueInnerProcessor,
			Action<T, T> matchProcessor,
			IEqualityComparer<T> comparer = null
		)
		{
			outer.Merge(inner, i => i, o => o, uniqueOuterProcessor, uniqueInnerProcessor, matchProcessor, comparer);
		}

		/// <summary>
		/// Correlates the elements of two sequences based on matching keys simmilarly to a "full outer join" in SQL.
		/// For each results, executes an action
		/// </summary>
		/// <typeparam name="TOuter">The type of the outer sequence elements.</typeparam>
		/// <typeparam name="TInner">The type of the inner sequence elements.</typeparam>
		/// <param name="outer">The outer sequence.</param>
		/// <param name="inner">The inner sequence.</param>
		/// <param name="merge">The merge specification.</param>
		public static void Merge<TOuter, TInner>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<IMergeSyntax<TOuter, TInner>, IMergeSyntaxEnd> merge)
		{
			var syntax = new MergeSyntax<TOuter, TInner, object>();
			var executor = (IMergeExecutor<TOuter, TInner>)merge(syntax);
			executor.Execute(outer, inner);
		}

		private class MergeSyntax<TOuter, TInner, TKey>
			: IMergeSyntax<TOuter, TInner>
			, IMergeOuterSyntax<TOuter, TInner, TKey>
			, IMergeOuterInnerSyntax<TOuter, TInner, TKey>
			, IMergeSyntaxEnd
			, IMergeExecutor<TOuter, TInner>
		{
			private Func<TOuter, TKey> _outerKeySelector;
			private Func<TInner, TKey> _innerKeySelector;
			private IEqualityComparer<TKey> _comparer;
			private Action<TOuter> _uniqueOuterProcessor;
			private Action<TInner> _uniqueInnerProcessor;
			private Action<TOuter, TInner> _matchProcessor;

			public IMergeOuterSyntax<TOuter, TInner, TActualKey> OuterKey<TActualKey>(Func<TOuter, TActualKey> outerKeySelector)
			{
				return new MergeSyntax<TOuter, TInner, TActualKey>() { _outerKeySelector = outerKeySelector };
			}

			public IMergeOuterInnerSyntax<TOuter, TInner, TKey> InnerKey(Func<TInner, TKey> innerKeySelector)
			{
				_innerKeySelector = innerKeySelector;
				return this;
			}

			public IMergeOuterInnerUsingComparerSyntax<TOuter, TInner, TKey> UsingComparer(IEqualityComparer<TKey> comparer)
			{
				_comparer = comparer;
				return this;
			}

			public IMergeWhenNotMatchedByInnerSyntax<TOuter, TInner, TKey> WhenNotMatchedByInner(Action<TOuter> uniqueOuterProcessor)
			{
				_uniqueOuterProcessor = uniqueOuterProcessor;
				return this;
			}

			public IMergeWhenNotMatchedByOuterSyntax<TOuter, TInner, TKey> WhenNotMatchedByOuter(Action<TInner> uniqueInnerProcessor)
			{
				_uniqueInnerProcessor = uniqueInnerProcessor;
				return this;
			}

			public IMergeSyntaxEnd WhenMatched(Action<TOuter, TInner> matchProcessor)
			{
				_matchProcessor = matchProcessor;
				return this;
			}

			public void Execute(IEnumerable<TOuter> outer, IEnumerable<TInner> inner)
			{
				outer.Merge(
					inner,
					_outerKeySelector,
					_innerKeySelector,
					_uniqueOuterProcessor ?? new Action<TOuter>(o => { }),
					_uniqueInnerProcessor ?? new Action<TInner>(i => { }),
					_matchProcessor ?? new Action<TOuter, TInner>((o, i) => { })
				);
			}
		}

		/// <summary />
		public interface IMergeSyntax<TOuter, TInner>
		{
			/// <summary>
			/// Specifies the outer key selector.
			/// </summary>
			/// <typeparam name="TKey">The type of the key.</typeparam>
			IMergeOuterSyntax<TOuter, TInner, TKey> OuterKey<TKey>(Func<TOuter, TKey> outerKeySelector);
		}

		/// <summary />
		public interface IMergeOuterSyntax<TOuter, TInner, TKey>
		{
			/// <summary>
			/// Specifies the inner key selector.
			/// </summary>
			IMergeOuterInnerSyntax<TOuter, TInner, TKey> InnerKey(Func<TInner, TKey> innerKeySelector);
		}

		/// <summary />
		public interface IMergeOuterInnerSyntax<TOuter, TInner, TKey> : IMergeOuterInnerUsingComparerSyntax<TOuter, TInner, TKey>
		{
			/// <summary>
			/// Specifies the comparer used for key comparisons. Defaults to EqualityComparer&lt;TKey&gt;.Default.
			/// </summary>
			IMergeOuterInnerUsingComparerSyntax<TOuter, TInner, TKey> UsingComparer(IEqualityComparer<TKey> comparer);
		}

		/// <summary />
		public interface IMergeOuterInnerUsingComparerSyntax<TOuter, TInner, TKey> : IMergeWhenNotMatchedByInnerSyntax<TOuter, TInner, TKey>
		{
			/// <summary>
			/// Specifies an action that is invoked for each element from outer that is not present in inner.
			/// </summary>
			IMergeWhenNotMatchedByInnerSyntax<TOuter, TInner, TKey> WhenNotMatchedByInner(Action<TOuter> uniqueOuterProcessor);
		}

		/// <summary />
		public interface IMergeWhenNotMatchedByInnerSyntax<TOuter, TInner, TKey> : IMergeWhenNotMatchedByOuterSyntax<TOuter, TInner, TKey>
		{
			/// <summary>
			/// Specifies an action that is invoked for each element from inner that is not present in outer.
			/// </summary>
			IMergeWhenNotMatchedByOuterSyntax<TOuter, TInner, TKey> WhenNotMatchedByOuter(Action<TInner> uniqueInnerProcessor);
		}

		/// <summary />
		public interface IMergeWhenNotMatchedByOuterSyntax<TOuter, TInner, TKey> : IMergeSyntaxEnd
		{
			/// <summary>
			/// Specifies an action that is invoked for each element from inner that is also present in outer.
			/// </summary>
			IMergeSyntaxEnd WhenMatched(Action<TOuter, TInner> matchProcessor);
		}

		/// <summary />
		public interface IMergeSyntaxEnd
		{
		}

		private interface IMergeExecutor<TOuter, TInner>
		{
			void Execute(IEnumerable<TOuter> outer, IEnumerable<TInner> inner);
		}
	}
}