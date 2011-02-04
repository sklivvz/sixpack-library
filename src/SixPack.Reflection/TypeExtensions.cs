// TypeExtensions.cs
//
//  Copyright (C) 2011 Antoine Aubry
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

namespace SixPack.Reflection
{
	/// <summary>
	/// Extension methods for the <see cref="Type"/> class.
	/// </summary>
	public static class TypeExtensions
	{
		#region GetImplementation
		/// <summary>
		/// Gets the implementation of a generic type definition.
		/// </summary>
		/// <param name="target">The type whose implementation is to be retrieved.</param>
		/// <param name="genericType">Type of the generic type that is being queried.</param>
		/// <returns></returns>
		/// <example>
		/// var listType = typeof(List&lt;int%gt;);
		/// var iListType = listType.GetImplementation(typeof(IList&lt;%gt;));
		/// iListType == typeof(IList&lt;int%gt;);	// Expression is true
		/// </example>
		public static Type GetImplementation(this Type target, Type genericType)
		{
			if (!genericType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The type must be a generic type definition.", "genericType");
			}

			return GetImplementationRecursive(target, genericType);
		}

		private static Type GetImplementationRecursive(Type target, Type genericType)
		{
			if (target == null || target == typeof(object))
			{
				return null;
			}

			if (target.IsGenericType && target.GetGenericTypeDefinition() == genericType)
			{
				return target;
			}

			if (genericType.IsClass)
			{
				return GetImplementationRecursive(target.BaseType, genericType);
			}
			else
			{
				foreach (var itf in target.GetInterfaces())
				{
					var implementation = GetImplementationRecursive(itf, genericType);
					if (implementation != null)
					{
						return implementation;
					}
				}

				return null;
			}
		}
		#endregion
	}
}