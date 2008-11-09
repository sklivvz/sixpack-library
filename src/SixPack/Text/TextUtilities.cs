// TextUtilities.cs 
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

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
	}
}
