// Enums.cs
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
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace SixPack
{
	/// <summary>
	/// This class is not called directly. Instead, use the Enums class. It has the same methods.
	/// </summary>
	/// <remarks>
	/// This class is a hack to work around a limitation in the C# compiler that does not let us
	/// declare generics type restrictions that force a type parameter to be an enum.
	/// </remarks>
	public partial class /* Enums */ EnumsInternalDoNotUse<TEnumType>
	{
		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to be parsed.</param>
		/// <returns></returns>
		public static TEnum Parse<TEnum>(string value) where TEnum : struct, TEnumType
		{
			return (TEnum)Enum.Parse(typeof(TEnum), value);
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to be parsed.</param>
		/// <param name="ignoreCase">if set to <c>true</c> ignore case.</param>
		/// <returns></returns>
		public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct, TEnumType
		{
			return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
		}

		/// <summary>
		/// Gets the names and values of the constants in a specified enumeration.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <returns></returns>
		public static TEnum[] GetValues<TEnum>() where TEnum : struct, TEnumType
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}

		/// <summary>
		/// Gets the value associated to the specified enum field. Values are associated using <see cref="EnumValueAttribute"/>.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value associated with the key.</param>
		/// <returns>
		/// If there is a value associated with the key, returns true; otherwise returns false.
		/// </returns>
		public static bool TryGetAssociatedValue<TEnum>(TEnum key, out object value) where TEnum : struct, TEnumType
		{
			var field = typeof(TEnum).GetField(key.ToString(), BindingFlags.Static | BindingFlags.Public);
			if (field == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'{0}' is not a valid member of {1}", key, typeof(TEnum).FullName), "key");
			}

			var attribute = (EnumValueAttribute)field.GetCustomAttributes(typeof(EnumValueAttribute), false).FirstOrDefault();
			if (attribute == null)
			{
				value = null;
				return false;
			}

			value = attribute.Value;
			return true;
		}

		/// <summary>
		/// Gets the value associated to the specified enum field. Values are associated using <see cref="EnumValueAttribute"/>.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static object GetAssociatedValue<TEnum>(TEnum key) where TEnum : struct, TEnumType
		{
			object value;
			if (!TryGetAssociatedValue<TEnum>(key, out value))
			{
				throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "No value is associated with {0}.{1}", typeof(TEnum).FullName, key));
			}
			return value;
		}
	}

	#region Implementation details
	[Obsolete("Do not use this class directly. Use the Enums class instead.")]
	public abstract partial class EnumsInternalDoNotUse<TEnumType> where TEnumType : class
	{
		internal EnumsInternalDoNotUse() { throw new InvalidOperationException("This type should never be instantiated."); }
	}

#pragma warning disable 0618 // Obsolete class used
	// HACK: This class exists to work around a limitation of the C# language, which does not allow to user System.Enum as a constraint.
	/// <summary>
	/// Utility class for manipulating enums.
	/// </summary>
	public abstract class Enums : EnumsInternalDoNotUse<Enum>
	{
		private Enums() { throw new InvalidOperationException("This type should never be instantiated."); }
	}
#pragma warning restore 0618
	#endregion
}