// VatCode.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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

using System.Text.RegularExpressions;

namespace SixPack.Globalization.Portugal
{
    /// <summary>
    /// Validates portuguese postal codes
    /// </summary>
    public static class VatCode
	{
		/// <summary>
		/// Regex expression to validate a Portuguese Postal Code
		/// </summary>
		private static readonly Regex vatCodePattern = new Regex(@"^(PT){0,1}[0-9]{9}$", RegexOptions.Compiled);

		/// <summary>
		/// Determines whether the specified VAT code is valid.
		/// </summary>
		/// <param name="vatCode">The VAT code.</param>
		/// <returns>
		/// 	<c>true</c> if the specified VAT code is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string vatCode)
		{
			return vatCodePattern.IsMatch(vatCode);
		}
	}
}
