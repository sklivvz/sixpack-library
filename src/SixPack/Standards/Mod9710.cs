// Mod9710.cs 
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
using System.Globalization;

namespace SixPack.Standards
{
	/// <summary>
	/// This class implements the MOD9010 checksum "check digits" algorythm.
	/// </summary>
	public static class Mod9710
	{
		private static readonly byte[] weights = {
		                                         	1, 10, 3, 30, 9, 90, 27, 76, 81, 34, 49, 5, 50, 15, 53, 45, 62, 38, 89, 17,
		                                         	73, 51, 25, 56, 75, 71, 31, 19, 93, 57, 85, 74, 61, 28, 86, 84, 64, 58, 95,
		                                         	77, 91, 37, 79, 14, 43, 42, 32, 29, 96, 87, 94, 67, 88, 7, 70, 21, 16, 63,
		                                         	48, 92, 47, 82, 44, 52, 35, 59, 8, 80, 24, 46, 72, 41, 22, 26, 66, 78, 4,
		                                         	40, 12, 23, 36, 69, 11, 13, 33, 39, 2, 20, 6, 60, 18, 83, 54, 55, 65, 68
		                                         };

		private static byte weight(int position)
		{
			return weights[position%weights.Length];
		}

		/// <summary>
		/// Calculates the Check Digits of a digit string.
		/// </summary>
		/// <param name="digits">The string to be checksummed.</param>
		/// <returns>A string containing the check digits.</returns>
		public static string Checksum(string digits)
		{
			if (digits == null)
				throw new ArgumentNullException("digits");
			long sum = 0;
			for (int i = 0; i < digits.Length; i++)
				sum += int.Parse(digits[digits.Length - i - 1].ToString(), CultureInfo.InvariantCulture)*weight(i);
			return String.Format(CultureInfo.InvariantCulture, "{0:00}", (98 - sum%97));
		}
	}
}
