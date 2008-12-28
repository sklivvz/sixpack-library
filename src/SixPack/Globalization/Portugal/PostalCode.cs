// PostalCode.cs 
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
using System.Text.RegularExpressions;
using SixPack.Properties;

namespace SixPack.Globalization.Portugal
{
	/// <summary>
	/// Validates portuguese postal codes
	/// </summary>
	public static class PostalCode
	{
		/// <summary>
		/// Regex expression to validate a Portuguese Postal Code
		/// </summary>
		private static readonly Regex postalCodePattern = new Regex(Settings.Default.Globalization_Portugal_PostalCode_PostalCodePattern, RegexOptions.Compiled);

		/// <summary>
		/// Determines whether the specified postal code is valid.
		/// </summary>
		/// <param name="postalCode">The postal code.</param>
		/// <returns>
		/// 	<c>true</c> if the specified postal code is valid Portuguese postal code; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string postalCode)
		{
			return postalCodePattern.IsMatch(postalCode);
		}
	}
}
