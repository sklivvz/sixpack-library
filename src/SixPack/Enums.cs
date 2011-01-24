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

namespace SixPack
{
	/// <summary>
	/// This class is not called directly. Instead, use the Enums class. It has the same methods.
	/// </summary>
	/// <remarks>
	/// This class is a hack to work around a limitation in the C# compiler that soes not let us
	/// declare generics type restrictions that force a type parameter to be an enum.
	/// </remarks>
	public partial class /* Enums */ EnumsInternalDoNotUse<TTemp>
	{
		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to be parsed.</param>
		/// <returns></returns>
		public static TEnum Parse<TEnum>(string value) where TEnum : struct, TTemp
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
		public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct, TTemp
		{
			return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
		}

		/// <summary>
		/// Gets the names and values of the constants in a specified enumeration.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <returns></returns>
		public static TEnum[] GetValues<TEnum>() where TEnum : struct, TTemp
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}
	}

	#region Implementation details
	[Obsolete("Do not use this class directly. Use the Enums class instead.")]
	public abstract partial class EnumsInternalDoNotUse<TTemp> where TTemp : class
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