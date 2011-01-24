// StringExtensions.cs
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
using System.Reflection;
using System.Text.RegularExpressions;

namespace SixPack.Text
{
	/// <summary>
	/// Extension methods for strings.
	/// </summary>
	public static class StringExtensions
	{
		#region FormatWith lambda
		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(n => GetColumnValue(n));
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="getArgument">Example: n => GetColumnValue(n)</param>
		/// <returns></returns>
		public static string FormatWith(this string format, Func<string, object> getArgument)
		{
			return format.FormatWith(null, getArgument);
		}

		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(n => GetColumnValue(n));
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="getArgument">Example: n => GetColumnValue(n)</param>
		/// <returns></returns>
		public static string FormatWith(this string format, IFormatProvider provider, Func<string, object> getArgument)
		{
			if (getArgument == null)
				throw new ArgumentNullException("getArgument");

			return format.FormatWith(provider, new LambdaValueProvider(getArgument));
		}

		private class LambdaValueProvider : IValueProvider
		{
			private readonly Func<string, object> _getArgument;

			public LambdaValueProvider(Func<string, object> getArgument)
			{
				_getArgument = getArgument;
			}

			#region IValueProvider Members

			object IValueProvider.this[string name]
			{
				get
				{
					return _getArgument(name);
				}
			}

			#endregion
		}
		#endregion

		#region FormatWith object
		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(new { name });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="arguments">Example: new { name = "Jack" }</param>
		/// <returns></returns>
		public static string FormatWith(this string format, object arguments)
		{
			return format.FormatWith(null, arguments);
		}

		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(new { name });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="arguments">Example: new { name = "Jack" }</param>
		/// <returns></returns>
		public static string FormatWith(this string format, IFormatProvider provider, object arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			return format.FormatWith(provider, new ReflectionValueProvider(arguments));
		}

		private class ReflectionValueProvider : IValueProvider
		{
			private readonly object _value;
			private readonly Type _type;

			public ReflectionValueProvider(object value)
			{
				_value = value;
				_type = value.GetType();
			}

			#region IValueProvider Members

			object IValueProvider.this[string name]
			{
				get
				{
					var property = _type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					return property.GetValue(_value, null);
				}
			}

			#endregion
		}
		#endregion

		#region FormatWith dictionary
		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   "Hello, {name}".FormatWith(new Dictionary&lt;string, object&gt; { { "name", "Jack" } });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="arguments">Example: new Dictionary&lt;string, object&gt; { { "name", "Jack" } }</param>
		/// <returns></returns>
		public static string FormatWith<T>(this string format, IDictionary<string, T> arguments)
		{
			return format.FormatWith(null, arguments);
		}

		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(new Dictionary&lt;string, object&gt; { { "name", "Jack" } });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="arguments">Example: new Dictionary&lt;string, object&gt; { { "name", "Jack" } }</param>
		/// <returns></returns>
		public static string FormatWith<T>(this string format, IFormatProvider provider, IDictionary<string, T> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			return format.FormatWith(provider, new DictionaryValueProvider<T>(arguments));
		}

		private class DictionaryValueProvider<T> : IValueProvider
		{
			private readonly IDictionary<string, T> _value;

			public DictionaryValueProvider(IDictionary<string, T> value)
			{
				_value = value;
			}

			#region IValueProvider Members

			object IValueProvider.this[string name]
			{
				get
				{
					return _value[name];
				}
			}

			#endregion
		}
		#endregion

		#region FormatWith IValueProvider
		private static readonly Regex _formatParser = new Regex(@"(?<prefix>(?:^|[^{])\{)(?<name>[^{:}]+)", RegexOptions.Compiled | RegexOptions.Singleline);

		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(new { name });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="arguments">The named arguments.</param>
		public static string FormatWith(this string format, IValueProvider arguments)
		{
			return format.FormatWith(null, arguments);
		}

		/// <summary>
		/// Formats a string with named placeholders.
		/// Example:
		///   var name = "Jack";
		///   "Hello, {name}".FormatWith(new { name });
		/// </summary>
		/// <param name="format">Example: "Hello, {name}"</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="arguments">The named arguments.</param>
		public static string FormatWith(this string format, IFormatProvider provider, IValueProvider arguments)
		{
			if (format == null)
				throw new ArgumentNullException("format");

			if (arguments == null)
				throw new ArgumentNullException("arguments");

			var args = new List<object>();
			var indexes = new Dictionary<string, int>();

			var stringFormat = _formatParser.Replace(format, match =>
			{
				var name = match.Groups["name"].Value;

				int index;
				if (!indexes.TryGetValue(name, out index))
				{
					index = indexes.Count;
					indexes.Add(name, index);
					args.Add(arguments[name]);
				}
				return match.Groups["prefix"].Value + index.ToString();
			});

			if (provider == null)
			{
				return string.Format(stringFormat, args.ToArray());
			}
			else
			{
				return string.Format(provider, stringFormat, args.ToArray());
			}
		}
		#endregion
	}

	/// <summary>
	/// Defines the interface of an object that provides values to fill placeholders.
	/// </summary>
	public interface IValueProvider
	{
		/// <summary>
		/// Gets the <see cref="System.Object"/> with the specified name.
		/// </summary>
		/// <value></value>
		object this[string name] { get; }
	}
}