// GenericTypeConverter.cs
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
using System.ComponentModel;
using System.Globalization;

namespace SixPack.ComponentModel
{
	/// <summary>
	/// Generic implementation of <see cref="System.ComponentModel.TypeConverter"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class GenericTypeConverter<T> : System.ComponentModel.TypeConverter
	{
		private readonly IDictionary<Type, Func<CultureInfo, object, T>> _convertFrom = new Dictionary<Type, Func<CultureInfo, object, T>>();
		private readonly IDictionary<Type, Func<CultureInfo, T, object>> _convertTo = new Dictionary<Type, Func<CultureInfo, T, object>>();

		/// <summary>
		/// Registers a conversion function that converts from the specified type.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="converter">The conversion function.</param>
		protected void CanConvertFrom<TSource>(Func<CultureInfo, TSource, T> converter)
		{
			_convertFrom.Add(typeof(TSource), (culture, source) => converter(culture, (TSource)source));
		}

		/// <summary>
		/// Registers a conversion function that converts from the specified type.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="converter">The conversion function.</param>
		protected void CanConvertFrom<TSource>(Func<TSource, T> converter)
		{
			_convertFrom.Add(typeof(TSource), (culture, source) => converter((TSource)source));
		}

		/// <summary>
		/// Registers a conversion function that converts to the specified type.
		/// </summary>
		/// <typeparam name="TDestination">The type of the destination.</typeparam>
		/// <param name="converter">The conversion function.</param>
		protected void CanConvertTo<TDestination>(Func<CultureInfo, T, TDestination> converter)
		{
			_convertTo.Add(typeof(TDestination), (culture, source) => converter(culture, source));
		}

		/// <summary>
		/// Registers a conversion function that converts to the specified type.
		/// </summary>
		/// <typeparam name="TDestination">The type of the destination.</typeparam>
		/// <param name="converter">The conversion function.</param>
		protected void CanConvertTo<TDestination>(Func<T, TDestination> converter)
		{
			_convertTo.Add(typeof(TDestination), (culture, source) => converter(source));
		}

		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return _convertFrom.ContainsKey(sourceType) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return _convertTo.ContainsKey(destinationType) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Func<CultureInfo, object, T> converter;
			return _convertFrom.TryGetValue(value.GetType(), out converter) ? converter(culture, value) : base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Func<CultureInfo, T, object> converter;
			return _convertTo.TryGetValue(destinationType, out converter) ? converter(culture, (T)value) : base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
