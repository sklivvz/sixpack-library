using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if! DOTNET_4_0
using SixPack.Collections.Generic;
#endif

namespace SixPack.Reflection
{
#if! DOTNET_4_0
	/// <summary>
	/// Drop-in replacement for the .NET 4.0 ExpressionVisitor class.
	/// </summary>
	public class ExpressionVisitor
	{
		/// <summary>
		/// Visits the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public Expression Visit(Expression expression)
		{
			if (expression == null)
			{
				return null;
			}

			switch (expression.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.Equal:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					return VisitBinary((BinaryExpression)expression);

				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.UnaryPlus:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
					return VisitUnary((UnaryExpression)expression);

				case ExpressionType.Call:
					return VisitMethodCall((MethodCallExpression)expression);

				case ExpressionType.Conditional:
					return VisitConditional((ConditionalExpression)expression);

				case ExpressionType.Constant:
					return VisitConstant((ConstantExpression)expression);

				case ExpressionType.Invoke:
					return VisitInvocation((InvocationExpression)expression);

				case ExpressionType.Lambda:
					return VisitLambdaInternal((LambdaExpression)expression);

				case ExpressionType.ListInit:
					return VisitListInit((ListInitExpression)expression);

				case ExpressionType.MemberAccess:
					return VisitMember((MemberExpression)expression);

				case ExpressionType.MemberInit:
					return VisitMemberInit((MemberInitExpression)expression);

				case ExpressionType.New:
					return VisitNew((NewExpression)expression);

				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
					return VisitNewArray((NewArrayExpression)expression);

				case ExpressionType.Parameter:
					return VisitParameter((ParameterExpression)expression);

				case ExpressionType.TypeIs:
					return VisitTypeIs((TypeBinaryExpression)expression);

				default:
					throw new ArgumentOutOfRangeException("expression", expression.NodeType, "Invalid expression type");
			}
		}

		private static readonly SingletonFactory<Type, Func<ExpressionVisitor, LambdaExpression, Expression>> _visitLambdaInvokers =
			new SingletonFactory<Type, Func<ExpressionVisitor, LambdaExpression, Expression>>(CreateVisitLambdaInvoker);

		private Expression VisitLambdaInternal(LambdaExpression expression)
		{
			var invokeVisitLambda = _visitLambdaInvokers[expression.Type];
			return invokeVisitLambda(this, expression);
		}

		private static Func<ExpressionVisitor, LambdaExpression, Expression> CreateVisitLambdaInvoker(Type type)
		{
			// Create a delegate that invokes the the generic method InvokeVisitLambda<T>

			var visitLambdaGeneric = typeof(ExpressionVisitor).GetMethod(
				"InvokeVisitLambda",
				BindingFlags.Static | BindingFlags.NonPublic
			);
			
			var visitLambdaConstructed = visitLambdaGeneric.MakeGenericMethod(type);

			return (Func<ExpressionVisitor, LambdaExpression, Expression>)Delegate.CreateDelegate(
				typeof(Func<ExpressionVisitor, LambdaExpression, Expression>),
				visitLambdaConstructed
			);
		}

		// Method invoked using reflection
		// ReSharper disable UnusedMember.Local
		private static Expression InvokeVisitLambda<T>(ExpressionVisitor visitor, LambdaExpression expression)
		{
			return visitor.VisitLambda((Expression<T>)expression);
		}
		// ReSharper restore UnusedMember.Local

		/// <summary>
		/// Visits the binary expression.
		/// </summary>
		protected virtual Expression VisitBinary(BinaryExpression expression)
		{
			var conversion = expression.Conversion != null ? (LambdaExpression)VisitLambdaInternal(expression.Conversion) : null;
			var left = Visit(expression.Left);
			var right = Visit(expression.Right);

			return Expression.MakeBinary(
				expression.NodeType,
				left,
				right,
				expression.IsLiftedToNull,
				expression.Method,
				conversion
			);
		}

		/// <summary>
		/// Visits the unary expression.
		/// </summary>
		protected virtual Expression VisitUnary(UnaryExpression expression)
		{
			var operand = Visit(expression.Operand);
			return Expression.MakeUnary(expression.NodeType, operand, expression.Type, expression.Method);
		}

		/// <summary>
		/// Visits the method call expression.
		/// </summary>
		protected virtual Expression VisitMethodCall(MethodCallExpression expression)
		{
			var instance = Visit(expression.Object);
			var arguments = expression.Arguments.Select(Visit);
			return Expression.Call(instance, expression.Method, arguments);
		}

		/// <summary>
		/// Visits the conditional expression.
		/// </summary>
		protected virtual Expression VisitConditional(ConditionalExpression expression)
		{
			var test = Visit(expression.Test);
			var ifTrue = Visit(expression.IfTrue);
			var ifFalse = Visit(expression.IfFalse);
			return Expression.Condition(test, ifTrue, ifFalse);
		}

		/// <summary>
		/// Visits the constant expression.
		/// </summary>
		protected virtual Expression VisitConstant(ConstantExpression expression)
		{
			return Expression.Constant(expression.Value);
		}

		/// <summary>
		/// Visits the invocation expression.
		/// </summary>
		protected virtual Expression VisitInvocation(InvocationExpression expression)
		{
			var invokedExpression = Visit(expression.Expression);
			var arguments = expression.Arguments.Select(Visit);
			return Expression.Invoke(invokedExpression, arguments);
		}

		/// <summary>
		/// Visits the lambda expression.
		/// </summary>
		protected virtual Expression VisitLambda<T>(Expression<T> expression)
		{
			var body = Visit(expression.Body);
			return Expression.Lambda(expression.Type, body, expression.Parameters);
		}

		/// <summary>
		/// Visits the list init expression.
		/// </summary>
		protected virtual Expression VisitListInit(ListInitExpression expression)
		{
			var newExpression = (NewExpression)VisitNew(expression.NewExpression);
			var initializers = expression.Initializers.Select(VisitElementInit);
			return Expression.ListInit(newExpression, initializers);
		}

		/// <summary>
		/// Visits the element init.
		/// </summary>
		private ElementInit VisitElementInit(ElementInit init)
		{
			var arguments = init.Arguments.Select(Visit);
			return Expression.ElementInit(init.AddMethod, arguments);
		}

		/// <summary>
		/// Visits the member access expression.
		/// </summary>
		protected virtual Expression VisitMember(MemberExpression expression)
		{
			var expr = Visit(expression.Expression);
			return Expression.MakeMemberAccess(expr, expression.Member);
		}

		/// <summary>
		/// Visits the member init expression.
		/// </summary>
		protected virtual Expression VisitMemberInit(MemberInitExpression expression)
		{
			var newExpression = (NewExpression)Visit(expression.NewExpression);
			var bindings = expression.Bindings.Select(VisitBinding);
			return Expression.MemberInit(newExpression, bindings);
		}

		/// <summary>
		/// Visits the binding.
		/// </summary>
		private MemberBinding VisitBinding(MemberBinding binding)
		{
			switch (binding.BindingType)
			{
				case MemberBindingType.Assignment:
					return VisitMemberAssignment((MemberAssignment)binding);

				case MemberBindingType.MemberBinding:
					return VisitMemberMemberBinding((MemberMemberBinding)binding);

				case MemberBindingType.ListBinding:
					return VisitMemberListBinding((MemberListBinding)binding);

				default:
					throw new ArgumentOutOfRangeException("binding", binding.BindingType, "Invalid binding type.");
			}
		}

		/// <summary>
		/// Visits the member assignment.
		/// </summary>
		private MemberBinding VisitMemberAssignment(MemberAssignment binding)
		{
			var expr = Visit(binding.Expression);
			return Expression.Bind(binding.Member, expr);
		}

		/// <summary>
		/// Visits the member member binding.
		/// </summary>
		private MemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			var bindings = binding.Bindings.Select(VisitBinding);
			return Expression.MemberBind(binding.Member, bindings);
		}

		/// <summary>
		/// Visits the member list binding.
		/// </summary>
		private MemberBinding VisitMemberListBinding(MemberListBinding binding)
		{
			var initializers = binding.Initializers.Select(VisitElementInit);
			return Expression.ListBind(binding.Member, initializers);
		}

		/// <summary>
		/// Visits the new expression.
		/// </summary>
		protected virtual Expression VisitNew(NewExpression expression)
		{
			var arguments = expression.Arguments.Select(Visit);
			return Expression.New(expression.Constructor, arguments, expression.Members);
		}

		/// <summary>
		/// Visits the new array expression.
		/// </summary>
		protected virtual Expression VisitNewArray(NewArrayExpression expression)
		{
			var expressions = expression.Expressions.Select(Visit);
			return Expression.NewArrayInit(expression.Type.GetElementType(), expressions);
		}

		/// <summary>
		/// Visits the parameter expression.
		/// </summary>
		protected virtual Expression VisitParameter(ParameterExpression expression)
		{
			// A new parameter cannot be created because that would alter the behavior of the expression.
			return expression;
		}

		/// <summary>
		/// Visits the typeIs expression.
		/// </summary>
		protected virtual Expression VisitTypeIs(TypeBinaryExpression expression)
		{
			var expr = Visit(expression.Expression);
			return Expression.TypeIs(expr, expression.TypeOperand);
		}
	}
#endif
}
