using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SixPack.Reflection
{
	/// <summary>
	/// Extension methods to <see cref="PropertyInfo"/>.
	/// </summary>
	public static class PropertyInfoExtensions
	{
		/// <summary>
		/// Creates a <see cref="Func{TSource, TProperty}" /> that reads from the specified property.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static Func<TSource, TProperty> GetReadAccessor<TSource, TProperty>(this PropertyInfo property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}

			if (!property.DeclaringType.IsAssignableFrom(typeof(TSource)))
			{
				throw new ArgumentException("TSource must be compatible with the type that contains the property.");
			}

			if (!typeof(TProperty).IsAssignableFrom(property.PropertyType))
			{
				throw new ArgumentException("TProperty must be compatible with the type of the property.");
			}

			var parameter = Expression.Parameter(typeof(TSource), "s");

			Expression propertyAccess = Expression.Property(parameter, property);
			if (typeof(TProperty) != property.PropertyType)
			{
				propertyAccess = Expression.Convert(propertyAccess, typeof(TProperty));
			}

			var accessor = Expression.Lambda<Func<TSource, TProperty>>(
				propertyAccess,
				parameter
			);

			return accessor.Compile();
		}

		///// <summary>
		///// Creates a <see cref="Func{TSource, TProperty}" /> that reads from the specified property.
		///// </summary>
		///// <typeparam name="TSource">The type of the source.</typeparam>
		///// <typeparam name="TProperty">The type of the property.</typeparam>
		///// <param name="property">The property.</param>
		///// <returns></returns>
		//public static Action<TSource, TProperty> GetWriteAccessor<TSource, TProperty>(this PropertyInfo property)
		//{
		//    if (property == null)
		//    {
		//        throw new ArgumentNullException("property");
		//    }

		//    if (!property.DeclaringType.IsAssignableFrom(typeof(TSource)))
		//    {
		//        throw new ArgumentException("TSource must be compatible with the type that contains the property.");
		//    }

		//    if (!typeof(TProperty).IsAssignableFrom(property.PropertyType))
		//    {
		//        throw new ArgumentException("TProperty must be compatible with the type of the property.");
		//    }

		//    var accessor = Delegate.CreateDelegate(typeof(Action<

		//    var sourceParameter = Expression.Parameter(typeof(TSource), "s");
		//    var valueParameter = Expression.Parameter(typeof(TProperty), "v");
		//    var accessor = Expression.Lambda<Action<TSource, TProperty>>(
		//        Expression.
		//        Expression.Property(sourceParameter, property),
		//        sourceParameter,
		//        valueParameter
		//    );

		//    return accessor.Compile();
		//}
	}
}