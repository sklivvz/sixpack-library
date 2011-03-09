using System;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SixPack.Data.Entity
{
	/// <summary>
	/// Extension methods for the <see cref="ObjectQuery{T}"/> class.
	/// </summary>
	public static class ObjectQueryExtensions
	{
		/// <summary>
		/// Orders the query results by the specified criteria.
		/// </summary>
		/// <param name="query">The query to be ordered.</param>
		/// <param name="property">The name of the property.</param>
		/// <param name="descending">if set to <c>true</c> reverse the ordering.</param>
		/// <returns></returns>
		public static ObjectQuery<T> OrderBy<T>(this ObjectQuery<T> query, string property, bool descending = false)
		{
			return query.OrderBy(
				string.Format("it.{0} {1}", property, descending ? "desc" : "asc")
			);
		}

		/// <summary>
		/// Specifies the related objects to include in the query results.
		/// </summary>
		/// <typeparam name="TSet">The type of the set.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="repository">The repository.</param>
		/// <param name="path">Expression that accesses the desired property.</param>
		/// <returns>
		/// A new <see cref="T:System.Data.Objects.ObjectQuery`1"/> with the defined query path.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="path"/> is empty.</exception>
		public static ObjectQuery<TSet> Include<TSet, TProperty>(this ObjectQuery<TSet> repository, Expression<Func<TSet, TProperty>> path)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}

			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			var propertyPath = new StringBuilder();

			try
			{
				AddToPath(path.Body, propertyPath);
			}
			catch (ArgumentException err)
			{

				throw new ArgumentException(string.Format("The specified property path [{0}] is invalid.", path), "path", err);
			}

			return repository.Include(propertyPath.ToString());
		}

		#region Helpers

		private static void AddToPath(Expression expression, StringBuilder path)
		{
			if (expression == null || expression is ParameterExpression)
			{
				return;
			}

			var memberExpression = expression as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException(
					string.Format(
						"The specified expression is not a MemberExpression. Actual type = {0}, expression = {1}",
						expression.GetType().Name,
						expression
					),
					"expression"
				);
			}

			if (memberExpression.Member.MemberType != MemberTypes.Property)
			{
				throw new ArgumentException(
					string.Format(
						"The specified expression does not reference a property. Actual type = {0}, expression = {1}",
						memberExpression.Member.MemberType,
						expression
					),
					"expression"
				);
			}

			AddToPath(memberExpression.Expression, path);

			if (path.Length > 0)
			{
				path.Append('.');
			}

			path.Append(memberExpression.Member.Name);
		}

		#endregion
	}
}
