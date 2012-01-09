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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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

			var implementations = new List<Type>(1);
			GetImplementationRecursive(target, genericType, implementations, false);
			Debug.Assert(implementations.Count <= 1);
			return implementations.FirstOrDefault();
		}

		/// <summary>
		/// Gets all the implementations of a generic type definition.
		/// </summary>
		/// <param name="target">The type whose implementation is to be retrieved.</param>
		/// <param name="genericType">Type of the generic type that is being queried.</param>
		/// <returns></returns>
		/// <example>
		/// var listType = typeof(List&lt;int%gt;);
		/// var iListType = listType.GetImplementations(typeof(IList&lt;%gt;));
		/// iListType.Contains(typeof(IList&lt;int%gt;));	// Expression is true
		/// </example>
		public static ICollection<Type> GetImplementations(this Type target, Type genericType)
		{
			if (!genericType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The type must be a generic type definition.", "genericType");
			}

			var implementations = new HashSet<Type>();
			GetImplementationRecursive(target, genericType, implementations, true);
			return implementations;
		}

		private static bool GetImplementationRecursive(Type target, Type genericType, ICollection<Type> results, bool findAll)
		{
			if (target == null || target == typeof(object))
			{
				return true;
			}

			if (target.IsGenericType && target.GetGenericTypeDefinition() == genericType)
			{
				results.Add(target);
				return findAll;
			}

			if (genericType.IsClass)
			{
				return GetImplementationRecursive(target.BaseType, genericType, results, findAll);
			}
			else
			{
				foreach (var itf in target.GetInterfaces())
				{
					var continueSearch = GetImplementationRecursive(itf, genericType, results, findAll);
					if (!continueSearch)
					{
						return false;
					}
				}

				return true;
			}
		}
		#endregion

		/// <summary>
		/// Gets all the types that are subtypes of the specified type. A subtype is a type that either implements
		/// a given interface or that extends a given class or interface.
		/// </summary>
		/// <param name="baseType">The base type.</param>
		/// <param name="searchAssemblies">The assemblies where subtypes are to be looked for. Defaults to the assembly that contains <paramref name="baseType"/> if omitted.</param>
		/// <returns></returns>
		public static IEnumerable<Type> GetSubTypes(this Type baseType, params Assembly[] searchAssemblies)
		{
			if (searchAssemblies == null || searchAssemblies.Length == 0)
			{
				searchAssemblies = new[] { baseType.Assembly };
			}

			foreach (var assembly in searchAssemblies)
			{
				foreach (var subType in assembly.GetTypes())
				{
					if (baseType != subType && baseType.IsAssignableFrom(subType))
					{
						yield return subType;
					}
				}
			}
		}

		private const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;

		#region GetProperties
		/// <summary>
		/// Gets all the properties of a type except the ones that have index parameters.
		/// </summary>
		public static IEnumerable<PropertyInfo> GetNonIndexedProperties(this Type type, BindingFlags bindingAttr = DefaultBindingFlags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			return type.GetProperties(bindingAttr)
				.Where(p => p.GetIndexParameters().Length == 0);
		}

		/// <summary>
		/// Gets the readable properties of a type that are not indexed.
		/// </summary>
		public static IEnumerable<PropertyInfo> GetReadableProperties(this Type type, BindingFlags bindingAttr = DefaultBindingFlags)
		{
			return type.GetNonIndexedProperties(bindingAttr)
				.Where(p => p.CanRead);
		}

		/// <summary>
		/// Gets the writable properties of a type that are not indexed.
		/// </summary>
		public static IEnumerable<PropertyInfo> GetWritableProperties(this Type type, BindingFlags bindingAttr = DefaultBindingFlags)
		{
			return type.GetNonIndexedProperties(bindingAttr)
				.Where(p => p.CanWrite);
		}

		/// <summary>
		/// Gets the readable and writable properties of a type that are not indexed.
		/// </summary>
		public static IEnumerable<PropertyInfo> GetReadableAndWritableProperties(this Type type, BindingFlags bindingAttr = DefaultBindingFlags)
		{
			return type.GetNonIndexedProperties(bindingAttr)
				.Where(p => p.CanRead && p.CanWrite);
		}
		#endregion
	}
}