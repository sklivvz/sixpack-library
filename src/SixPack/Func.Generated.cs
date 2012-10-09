using System;

namespace SixPack
{
	/// <summary>
	/// Provides a syntax to declare variables of type Func&lt;...&gt; that return anonymous types.
	/// </summary>
	public static class Func
	{
	

		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<TResult> CreateAnonymous<TResult>(Func<TResult> func)
		{
			return func;
		}



		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, TResult> CreateAnonymous<T1, TResult>(Func<T1, TResult> func)
		{
			return func;
		}



		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, TResult> CreateAnonymous<T1, T2, TResult>(Func<T1, T2, TResult> func)
		{
			return func;
		}



		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, TResult> CreateAnonymous<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
		{
			return func;
		}



		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, T4, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, T4, TResult> CreateAnonymous<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
		{
			return func;
		}


#if NET_40
		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, T4, T5, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, T4, T5, TResult> CreateAnonymous<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
		{
			return func;
		}
#endif

#if NET_40
		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, T4, T5, T6, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, T4, T5, T6, TResult> CreateAnonymous<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func)
		{
			return func;
		}
#endif

#if NET_40
		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, T4, T5, T6, T7, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> CreateAnonymous<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
		{
			return func;
		}
#endif

#if NET_40
		/// <summary>
		/// Provides a syntax to declare variables of type Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, TResult&gt; that return anonymous types.
		/// </summary>
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> CreateAnonymous<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
		{
			return func;
		}
#endif

	}
}