using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SixPack.Reflection
{
	/// <summary>
	/// Gives access to <see cref="MethodInfo"/> from expressions. This is useful
	/// because it allows static checking of the existence of the method.
	/// </summary>
	public static class MethodReference
	{

		private static MethodInfo GetMethodFromExpression(LambdaExpression method)
		{
			var methodCall = method.Body as MethodCallExpression;
			if(methodCall == null)
			{
				throw new ArgumentException("The expression must be a method call.", "method");
			}
			return methodCall.Method;
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1>(Expression<Action<T1>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, TResult>(Expression<Func<T1, TResult>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2>(Expression<Action<T1, T2>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2, T3>(Expression<Action<T1, T2, T3>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2, T3, T4>(Expression<Action<T1, T2, T3, T4>> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> method)
		{
			return GetMethodFromExpression(method);
		}

	}
}