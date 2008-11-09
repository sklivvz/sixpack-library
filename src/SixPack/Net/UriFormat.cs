// UriFormat.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Copyright (C) 2008 Marco Cecconi
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace SixPack.Net
{
	/// <summary>
	/// Validates an URI
	/// </summary>
	public static class UriFormat
	{
		/// <summary>
		/// Regex expression to validate a URI according to RFC 2396
		/// </summary>
		private static readonly Regex rfc2396UriPattern = new Regex(
			@"(([a-zA-Z][0-9a-zA-Z+\-\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\.\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\.\-_!~*'()%]+)?",
			RegexOptions.Compiled
		);

		/// <summary>
		/// Regex expression to validate a URI in a simple, but efficient manner
		/// </summary>
		private static readonly Regex simpleUriPattern = new Regex(
			@"(((ht|f)tp(s?):\/\/)|(www\.[^ \[\]\(\)
\r\t]+)|(([012]?[0-9]{1,2}\.){3}[012]?[0-9]{1,2})\/)([^ \[\]\(\),;""'&lt;&gt;
\r\t]+)([^\. \[\]\(\),;""'<>
\r\t])|(([012]?[0-9]{1,2}\.){3}[012]?[0-9]{1,2})",
			RegexOptions.Compiled
		);

		/// <summary>
		/// Determines whether the specified URI is valid.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <param name="mode">The mode.</param>
		/// <returns>
		/// 	<c>true</c> if the specified URI is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string uri, UriSyntaxValidationMode mode)
		{
			switch (mode)
			{
				case UriSyntaxValidationMode.Simple:
					return rfc2396UriPattern.IsMatch(uri);
	
				case UriSyntaxValidationMode.Rfc2396:
					return simpleUriPattern.IsMatch(uri);

				default:
					throw new NotImplementedException(String.Format(CultureInfo.InvariantCulture, "Validation mode {0} is not implemented.", mode));
			}
		}

		/// <summary>
		/// Determines whether the specified URI is valid.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>
		/// 	<c>true</c> if the specified URI is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string uri)
		{
			return IsValid(uri, UriSyntaxValidationMode.Simple);
		}
	}
}
