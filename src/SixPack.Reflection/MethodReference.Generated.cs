using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SixPack.Reflection
{
	/// <summary>
	/// Gives access to <see cref="MethodInfo"/> from expressions. This is useful
	/// because it allows static checking of the existence of the method.
	/// </summary>
	partial class MethodReference
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

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get(Expression<Action> method)
		{
			return GetMethodFromExpression(method);
		}

		/// <summary>
		/// Gets the method referenced by the specified expression.
		/// </summary>
		public static MethodInfo Get<TResult>(Expression<Func<TResult>> method)
		{
			return GetMethodFromExpression(method);
		}

	}

  public static class __MethodReferenceHelpers
  {
		public sealed class GenericMethodInvokerBuilder0<>
		{
			public GenericMethodInvokerBuilder1<T1, object> WithDynamicTypeArgument()
			{
				return new GenericMethodInvokerBuilder1<T1, object>();
			}

			public GenericMethodInvokerBuilder1<T1, TArgument> WithTypeArgument<TArgument>()
			{
				return new GenericMethodInvokerBuilder1<T1, TArgument>();
			}

			public Func<T1, TResult> InvokesStaticMethod<TResult>(Expression<Func<T1, TResult>> methodToInvoke)
			{
				var method = GetMethodFromExpression(methodToInvoke).GetGenericMethodDefinition();
				if (!method.IsStatic)
				{
					throw new ArgumentException(string.Format("Method '{0}' is not static.", method.Name));
				}

				return (T1 a1) => (TResult)method
					.MakeGenericMethod(typeof(T1))
					.Invoke(null, new object[] { a1 });
			}
		}

		public sealed class GenericMethodInvokerBuilder1<T1>
		{
			public GenericMethodInvokerBuilder2<T1, object> WithDynamicTypeArgument()
			{
				return new GenericMethodInvokerBuilder2<T1, object>();
			}

			public GenericMethodInvokerBuilder2<T1, TArgument> WithTypeArgument<TArgument>()
			{
				return new GenericMethodInvokerBuilder2<T1, TArgument>();
			}

			public Func<T1, TResult> InvokesStaticMethod<TResult>(Expression<Func<T1, TResult>> methodToInvoke)
			{
				var method = GetMethodFromExpression(methodToInvoke).GetGenericMethodDefinition();
				if (!method.IsStatic)
				{
					throw new ArgumentException(string.Format("Method '{0}' is not static.", method.Name));
				}

				return (T1 a1) => (TResult)method
					.MakeGenericMethod(typeof(T1))
					.Invoke(null, new object[] { a1 });
			}
		}

		public sealed class GenericMethodInvokerBuilder2<T1, T2>
		{
			public GenericMethodInvokerBuilder3<T1, object> WithDynamicTypeArgument()
			{
				return new GenericMethodInvokerBuilder3<T1, object>();
			}

			public GenericMethodInvokerBuilder3<T1, TArgument> WithTypeArgument<TArgument>()
			{
				return new GenericMethodInvokerBuilder3<T1, TArgument>();
			}

			public Func<T1, TResult> InvokesStaticMethod<TResult>(Expression<Func<T1, TResult>> methodToInvoke)
			{
				var method = GetMethodFromExpression(methodToInvoke).GetGenericMethodDefinition();
				if (!method.IsStatic)
				{
					throw new ArgumentException(string.Format("Method '{0}' is not static.", method.Name));
				}

				return (T1 a1) => (TResult)method
					.MakeGenericMethod(typeof(T1))
					.Invoke(null, new object[] { a1 });
			}
		}

		public sealed class GenericMethodInvokerBuilder3<T1, T2, T3>
		{
			public GenericMethodInvokerBuilder4<T1, object> WithDynamicTypeArgument()
			{
				return new GenericMethodInvokerBuilder4<T1, object>();
			}

			public GenericMethodInvokerBuilder4<T1, TArgument> WithTypeArgument<TArgument>()
			{
				return new GenericMethodInvokerBuilder4<T1, TArgument>();
			}

			public Func<T1, TResult> InvokesStaticMethod<TResult>(Expression<Func<T1, TResult>> methodToInvoke)
			{
				var method = GetMethodFromExpression(methodToInvoke).GetGenericMethodDefinition();
				if (!method.IsStatic)
				{
					throw new ArgumentException(string.Format("Method '{0}' is not static.", method.Name));
				}

				return (T1 a1) => (TResult)method
					.MakeGenericMethod(typeof(T1))
					.Invoke(null, new object[] { a1 });
			}
		}

		public sealed class GenericMethodInvokerBuilder4<T1, T2, T3, T4>
		{
			public GenericMethodInvokerBuilder5<T1, object> WithDynamicTypeArgument()
			{
				return new GenericMethodInvokerBuilder5<T1, object>();
			}

			public GenericMethodInvokerBuilder5<T1, TArgument> WithTypeArgument<TArgument>()
			{
				return new GenericMethodInvokerBuilder5<T1, TArgument>();
			}

			public Func<T1, TResult> InvokesStaticMethod<TResult>(Expression<Func<T1, TResult>> methodToInvoke)
			{
				var method = GetMethodFromExpression(methodToInvoke).GetGenericMethodDefinition();
				if (!method.IsStatic)
				{
					throw new ArgumentException(string.Format("Method '{0}' is not static.", method.Name));
				}

				return (T1 a1) => (TResult)method
					.MakeGenericMethod(typeof(T1))
					.Invoke(null, new object[] { a1 });
			}
		}

  }
}