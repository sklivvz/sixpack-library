// ExpressionExtensions.cs 
//
//  Copyright (C) 2011, 2012 Antoine Aubry
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SixPack.Reflection
{
	/// <summary>
	/// Estension methods that add functionality to expressions.
	/// </summary>
	public static class ExpressionExtensions
	{
		#region Visitors
		private sealed class ReplaceParameterVisitor : ExpressionVisitor
		{
			private readonly ParameterExpression _original;
			private readonly Expression _replacement;

			public ReplaceParameterVisitor(ParameterExpression original, Expression replacement)
			{
				_original = original;
				_replacement = replacement;
			}

			protected override Expression VisitParameter(ParameterExpression node)
			{
				if (node == _original)
				{
					return _replacement;
				}

				return base.VisitParameter(node);
			}
		}

		private sealed class MemberInitVisitor<TInput, TProperty> : ExpressionVisitor
		{
			private readonly MemberInfo _assignedProperty;
			private Expression<Func<TInput, TProperty>> _assignedValue;

			public MemberInitVisitor(
				MemberInfo assignedProperty,
				Expression<Func<TInput, TProperty>> assignedValue
			)
			{
				_assignedProperty = assignedProperty;
				_assignedValue = assignedValue;
			}

			protected override Expression VisitLambda<T>(Expression<T> node)
			{
				// Change the parameter of the assigned value accessor to match the current lambda expression.
				_assignedValue = _assignedValue.ReplaceParameter(node.Parameters.Single());
				return base.VisitLambda(node);
			}

			protected override Expression VisitMemberInit(MemberInitExpression node)
			{
				return Expression.MemberInit(
					node.NewExpression,
					node.Bindings.Concat(
						new MemberBinding[]
						{
							Expression.Bind(
								_assignedProperty,
								_assignedValue.Body
							)
						}
					)
				);
			}
		}

		private sealed class AsMemberExpressionVisitor<TMember> : ExpressionVisitor
			where TMember : MemberInfo
		{
			public TMember Member { get; private set; }

			protected override Expression VisitMember(MemberExpression node)
			{
				Member = (TMember)node.Member;
				return node;
			}
		}
		#endregion

		/// <summary>
		/// Returns a new expression where references to the specified parameter
		/// have been replaced by references to another one.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="original">The original parameter.</param>
		/// <param name="replacement">The replacement parameter.</param>
		/// <returns></returns>
		public static Expression ReplaceParameter(
			this Expression expression,
			ParameterExpression original,
			Expression replacement
		)
		{
			var visitor = new ReplaceParameterVisitor(original, replacement);
			return visitor.Visit(expression);
		}

		/// <summary>
		/// Returns a new expression where references to the first parameter
		/// have been replaced by references to another one.
		/// </summary>
		/// <typeparam name="TDelegate">The type of the delegate.</typeparam>
		/// <param name="expression">The expression.</param>
		/// <param name="replacement">The replacement parameter.</param>
		/// <returns></returns>
		public static Expression<TDelegate> ReplaceParameter<TDelegate>(
			this Expression<TDelegate> expression,
			ParameterExpression replacement
		)
		{
			return (Expression<TDelegate>)expression.ReplaceParameter(expression.Parameters.Single(), replacement);
		}

		/// <summary>
		/// Adds a member initialization to an existing object initialization expression.
		/// </summary>
		/// <example>
		/// Expression&lt;Func&lt;Original, Result&gt;&gt; transform = o => new Result { Prop1 = o.Prop1 };
		/// transform = transform.AddMemberInitialization(r => r.Prop2, o => o.Prop1.ToString());
		/// 
		/// Transform is now equivalent to
		///    o => new Result { Prop1 = o.Prop1, Prop2 = o.Prop1.ToString() };
		/// </example>
		/// <typeparam name="TInput">The type of the input.</typeparam>
		/// <typeparam name="TOutput">The type of the output.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="query">The initialization expression to be transformed.</param>
		/// <param name="assignedProperty">The property to be assigned.</param>
		/// <param name="assignedValue">The value to assign to the property.</param>
		/// <returns></returns>
		public static Expression<Func<TInput, TOutput>> AddMemberInitialization<TInput, TOutput, TProperty>(
			this Expression<Func<TInput, TOutput>> query,
			Expression<Func<TOutput, TProperty>> assignedProperty,
			Expression<Func<TInput, TProperty>> assignedValue
		)
		{
			return query.AddMemberInitialization(((MemberExpression)assignedProperty.Body).Member, assignedValue);
		}

		/// <summary>
		/// Adds a member initialization to an existing object initialization expression.
		/// </summary>
		/// <example>
		/// Expression&lt;Func&lt;Original, Result&gt;&gt; transform = o => new Result { Prop1 = o.Prop1 };
		/// transform = transform.AddMemberInitialization(r => r.Prop2, o => o.Prop1.ToString());
		/// 
		/// Transform is now equivalent to
		///    o => new Result { Prop1 = o.Prop1, Prop2 = o.Prop1.ToString() };
		/// </example>
		/// <typeparam name="TInput">The type of the input.</typeparam>
		/// <typeparam name="TOutput">The type of the output.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="query">The initialization expression to be transformed.</param>
		/// <param name="assignedProperty">The property to be assigned.</param>
		/// <param name="assignedValue">The value to assign to the property.</param>
		/// <returns></returns>
		public static Expression<Func<TInput, TOutput>> AddMemberInitialization<TInput, TOutput, TProperty>(
			this Expression<Func<TInput, TOutput>> query,
			MemberInfo assignedProperty,
			Expression<Func<TInput, TProperty>> assignedValue
		)
		{
			var visitor = new MemberInitVisitor<TInput, TProperty>(
				assignedProperty,
				assignedValue
			);
			return (Expression<Func<TInput, TOutput>>)visitor.Visit(query);
		}

		/// <summary>
		/// Inserts the specified expression in place of the parameter of the target expression, creating a new expression.
		/// </summary>
		/// <typeparam name="TSource">The type of the new parameter.</typeparam>
		/// <typeparam name="TParameter">The type of the original parameter.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="targetExpression">The expression.</param>
		/// <param name="parameterValueExpression">The parameter value.</param>
		/// <returns></returns>
		public static Expression<Func<TSource, TResult>> UseAsParameterOf<TSource, TParameter, TResult>(
			this Expression<Func<TSource, TParameter>> parameterValueExpression,
			Expression<Func<TParameter, TResult>> targetExpression
		)
		{
			return Expression.Lambda<Func<TSource, TResult>>(
				targetExpression.Body.ReplaceParameter(targetExpression.Parameters[0], parameterValueExpression.Body),
				parameterValueExpression.Parameters[0]
			);
		}

		#region AsField
		/// <summary>
		/// Returns the field that is being referenced by the specified expression.
		/// </summary>
		/// <typeparam name="TParent">The type of the parent.</typeparam>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public static FieldInfo AsField<TParent, TField>(this Expression<Func<TParent, TField>> expression)
		{
			var visitor = new AsMemberExpressionVisitor<FieldInfo>();
			visitor.Visit(expression);
			return visitor.Member;
		}

		/// <summary>
		/// Returns the name of the property that is being referenced by the specified expression.
		/// </summary>
		public static string FieldName<TParent, TField>(this Expression<Func<TParent, TField>> expression)
		{
			return expression.AsField().Name;
		}
		#endregion

		#region AsProperty
		/// <summary>
		/// Returns the property that is being referenced by the specified expression.
		/// </summary>
		/// <typeparam name="TParent">The type of the parent.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public static PropertyInfo AsProperty<TParent, TProperty>(this Expression<Func<TParent, TProperty>> expression)
		{
			var visitor = new AsMemberExpressionVisitor<PropertyInfo>();
			visitor.Visit(expression);
			return visitor.Member;
		}

		/// <summary>
		/// Returns the name of the property that is being referenced by the specified expression.
		/// </summary>
		public static string PropertyName<TParent, TProperty>(this Expression<Func<TParent, TProperty>> expression)
		{
			return expression.AsProperty().Name;
		}
		#endregion

		#region RemoveConversion

		private class RemoveConversionToSelfVisitor : ExpressionVisitor
		{
			private readonly Type targetType;

			public RemoveConversionToSelfVisitor(Type targetType)
			{
				this.targetType = targetType;
			}

			protected override Expression VisitUnary(UnaryExpression expression)
		    {
				switch (expression.NodeType)
				{
					case ExpressionType.Convert:
						if (expression.Operand.Type == targetType)
						{
							return expression.Operand;
						}
						break;
				}
				return base.VisitUnary(expression);
			}
		}

		/// <summary>
		/// Removes casts from TSource to a base class or interface that are used to call a member
		/// when TSource also defines that member.
		/// </summary>
		/// <example>
		/// Given the following code and expression:
		/// <code>
		/// private interface IHasIdentifier
		/// {
		/// 	int Id { get; }
		/// }
		/// 
		/// private class Entity : IHasIdentifier
		/// {
		/// 	public int Id { get; set; }
		/// }
		/// 
		/// Expression&lt;Func&lt;Entity, int&gt;&gt; getId = entity => ((IHasIdentifier)entity).Id
		/// </code>
		/// 
		/// getId.RemoveConversionToSelf() will return
		/// <code>
		/// getId = entity => entity.Id
		/// </code>
		/// </example>
		/// <remarks>
		/// This method is useful when working with Entity Framework and interfaces are used to
		/// mark entities with standard properties, like the primary key.
		/// In that case, it is common to have a generic method that uses properties from the interface
		/// to generate queries. Because the property access expression will cast entities to the type of
		/// the interface, the query generation fails. Removing the cast will solve the issue.
		/// </remarks>
		public static Expression<Func<TSource, TProperty>> RemoveConversionToSelf<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return (Expression<Func<TSource, TProperty>>)new RemoveConversionToSelfVisitor(typeof(TSource)).Visit(expression);
		}
		#endregion

		#region MakeSetter
		/// <summary>
		/// Converts an expression that reads from a property into an expression that writes to that property.
		/// </summary>
		/// <param name="getter"></param>
		/// <returns></returns>
		public static Action<TParent, TProperty> MakePropertySetter<TParent, TProperty>(this Expression<Func<TParent, TProperty>> getter)
		{
			var setter = getter.AsProperty().GetSetMethod();
			return (Action<TParent, TProperty>)Delegate.CreateDelegate(typeof(Action<TParent, TProperty>), setter);
		}
		#endregion
	}
}