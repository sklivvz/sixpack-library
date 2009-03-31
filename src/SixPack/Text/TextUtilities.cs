// TextUtilities.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
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
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace SixPack.Text
{
	/// <summary>
	/// This class contains text related utility methods
	/// </summary>
	public static class TextUtilities
	{
		#region Clip

		/// <summary>
		/// Clips the text to the specified length, adding a terminator (e.g. "...").
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">Text length to clip to.</param>
		/// <param name="terminator">The terminator.</param>
		/// <param name="respectWordBoundaries">if set to <c>true</c> it will respect word boundaries.</param>
		/// <returns></returns>
		public static string Clip(string text, int maxLength, string terminator, bool respectWordBoundaries)
		{
			if (text == null)
				return string.Empty;
			if (terminator == null)
				terminator = string.Empty;
			if (maxLength < 0)
				throw new ArgumentOutOfRangeException("maxLength", "Max Length must not be negative");

			if (respectWordBoundaries)
			{
				if (text.Length > maxLength && maxLength > terminator.Length)
					return Regex.Replace(text.Substring(0, maxLength - terminator.Length + 1), " +[^ ]*?$", String.Empty) + terminator;
				else if (text.Length > maxLength)
					return Regex.Replace(text.Substring(0, maxLength + 1), " +[^ ]*?$", String.Empty);
			}
			else
			{
				if (text.Length > maxLength)
				{
					if (terminator.Length < maxLength)
						return text.Substring(0, maxLength - terminator.Length) + terminator;
					else
						return text.Substring(0, maxLength);
				}
			}
			return text;
		}

		/// <summary>
		/// Clips the text to the specified length.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">Text length to clip to.</param>
		/// <param name="respectWordBoundaries">if set to <c>true</c> it will respect word boundaries.</param>
		/// <returns></returns>
		public static string Clip(string text, int maxLength, bool respectWordBoundaries)
		{
			return Clip(text, maxLength, String.Empty, respectWordBoundaries);
		}

		/// <summary>
		/// Clips the text to the specified length, respecting word boundaries.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">Text length to clip to.</param>
		/// <returns></returns>
		public static string Clip(string text, int maxLength)
		{
			return Clip(text, maxLength, true);
		}

		/// <summary>
		/// Clips the text to the specified length, adding a terminator (e.g. "..."), respecting word boundaries.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">Text length to clip to.</param>
		/// <param name="terminator">The terminator.</param>
		/// <returns></returns>
		public static string Clip(string text, int maxLength, string terminator)
		{
			return Clip(text, maxLength, terminator, true);
		}

		#endregion

		///// <summary>
		///// Replaces each occurence of each string in <paramref name="oldValues" /> by the corresponding string in <paramref name="newValues" />.
		///// </summary>
		///// <param name="original">The string to be searched.</param>
		///// <param name="oldValues">The values to search for.</param>
		///// <param name="newValues">The values to replace with.</param>
		///// <returns></returns>
		//public static string ReplaceAll(string original, string[] oldValues, string[] newValues)
		//{
		//    if (oldValues.Length != newValues.Length)
		//    {
		//        throw new ArgumentException("oldValues and newValues must have the same length");
		//    }

		//    StringBuilder replaced = new StringBuilder(original);
		//    for (int i = 0; i < oldValues.Length; i++)
		//    {
		//        replaced.Replace(oldValues[i], newValues[i]);
		//    }
		//    return replaced.ToString();
		//}

		/// <summary>
		/// Removes the HTML from a text string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string RemoveHtml(string text)
		{
			return Regex.Replace(text, "<[^>]*>", String.Empty);
		}

		/// <summary>
		/// Converts camel case text to a space separated string.
		/// </summary>
		/// <param name="camelCaseText">The camel case text.</param>
		/// <returns></returns>
		public static string ConvertCamelCaseToSpaceSeparated(string camelCaseText)
		{
			return Regex.Replace(camelCaseText, @"\B([A-Z])", " $1");
		}

		/// <summary>
		/// Converts the specified string to base-64 using UTF-8 encoding.
		/// </summary>
		/// <param name="plaintext">The text to be converted.</param>
		/// <returns>Returns a string with the base-64 representation of the plain text.</returns>
		public static string ToBase64(string plaintext)
		{
			byte[] encodedText = Encoding.UTF8.GetBytes(plaintext);
			return Convert.ToBase64String(encodedText);
		}

		/// <summary>
		/// Converts the specified base-64 string to plain text using UTF-8 encoding.
		/// </summary>
		/// <param name="base64Text">The base-64 encoded text.</param>
		/// <returns>Returns a string with the decoded text.</returns>
		public static string FromBase64(string base64Text)
		{
			byte[] encodedText = Convert.FromBase64String(base64Text);
			return Encoding.UTF8.GetString(encodedText);
		}

		/// <summary>
		/// Converts the specified array to an hexadecimal string.
		/// </summary>
		/// <param name="data">The array to convert.</param>
		/// <returns></returns>
		public static string ToHex(byte[] data)
		{
			if(data == null)
			{
				throw new ArgumentNullException("data");
			}

			StringBuilder hexadecimal = new StringBuilder(data.Length * 2);
			foreach (byte part in data)
			{
				hexadecimal.AppendFormat(CultureInfo.InvariantCulture, "{0:X02}", part);
			}
			return hexadecimal.ToString();
		}

		/// <summary>
		/// Converts the specified hexadimal string to an array.
		/// </summary>
		/// <param name="hexadecimal">The hexadecimal string to be converted.</param>
		/// <returns></returns>
		public static byte[] FromHex(string hexadecimal)
		{
			if (hexadecimal == null)
			{
				throw new ArgumentNullException("hexadecimal");
			}

			if ((hexadecimal.Length % 2) != 0)
			{
				throw new ArgumentException("The string to convert must contain an even number of hexadecimal digits.", "hexadecimal");
			}

			byte[] data = new byte[hexadecimal.Length / 2];
			for(int i = 0; i < data.Length; ++i)
			{
				string part = hexadecimal.Substring(i * 2, 2);
				data[i] = byte.Parse(part, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return data;
		}
		
		/// <summary>
		/// Sanitizes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="sanitationMode">The sanitation mode.</param>
		/// <returns></returns>
		public static string Sanitize(string text, TextSanitationMode sanitationMode)
		{
			switch (sanitationMode)
			{
				case TextSanitationMode.Alphanumeric:
					return Regex.Replace(text, "[^a-z0-9]+", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
				default:
					throw new NotImplementedException("The requested sanitation mode has not been implemented.");
			}
		}

		/// <summary>
		/// Normalizes a string for use in code.
		/// </summary>
		/// <param name="text">The text to be normalized.</param>
		/// <param name="normalizationType">Type of the normalization.</param>
		/// <returns></returns>
		public static string NormalizeForCode(string text, TextNormalizationType normalizationType)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text", "Null or empty strings cannot be normalized for code.");

			string work = Regex.Replace(text, "[^a-z0-9_]+", "_", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			if (work == "_")
				throw new ArgumentException("A correct name cannot be created from the text specified.", "text");

			switch (normalizationType)
			{
				case TextNormalizationType.Field:
					if (work.Length == 1)
						return work.ToLowerInvariant();
					return work.ToLowerInvariant()[0] + work.Substring(1);
				default:
					if (work.Length == 1)
						return work.ToUpperInvariant();
					return work.ToUpperInvariant()[0] + work.Substring(1);
			}
		}
	}
}
