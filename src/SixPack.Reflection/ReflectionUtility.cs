using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SixPack.Reflection
{
	/// <summary>
	/// Utility methods for reflection.
	/// </summary>
	public static class ReflectionUtility
	{
		private class GetMethodVisitor : ExpressionVisitor
		{
			public MethodInfo Method;

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				Method = node.Method;
				return base.VisitMethodCall(node);
			}
		}

		/// <summary>
		/// Gets the method that is referenced by the specified expression.
		/// This method provides a type-safe way of obtaining a <see cref="MethodInfo"/>.
		/// </summary>
		public static MethodInfo GetMethodFromExpression(Expression expression)
		{
			var visitor = new GetMethodVisitor();
			visitor.Visit(expression);
			return visitor.Method;
		}

		/// <summary>
		/// Gets the method that is referenced by the specified expression.
		/// This method provides a type-safe way of obtaining a <see cref="MethodInfo"/>.
		/// </summary>
		public static MethodInfo GetMethodFromExpression<T>(Expression<Action<T>> expression)
		{
			return GetMethodFromExpression((Expression)expression);
		}
	}
}