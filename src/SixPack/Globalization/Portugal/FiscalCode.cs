// FiscalCode.cs 
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
using System.Text.RegularExpressions;
using SixPack.Properties;

namespace SixPack.Globalization.Portugal
{
	/// <summary>
	/// Validates portuguese postal codes
	/// </summary>
	public static class FiscalCode
	{
		/// <summary>
		/// Regex expression to validate a Portuguese Postal Code
		/// </summary>
		private static readonly Regex fiscalCodePattern = new Regex(Settings.Default.Globalization_Portugal_FiscalCode_FiscalCodePattern, RegexOptions.Compiled);

		/// <summary>
		/// Determines whether the specified fiscal code is valid.
		/// </summary>
		/// <param name="fiscalCode">The fiscal code.</param>
		/// <returns>
		/// 	<c>true</c> if the specified fiscal code is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string fiscalCode)
		{
			return fiscalCodePattern.IsMatch(fiscalCode);
		}
	}
}
