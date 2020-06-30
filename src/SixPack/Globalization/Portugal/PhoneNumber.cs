// PhoneNumber.cs 
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

using System.Text.RegularExpressions;

namespace SixPack.Globalization.Portugal
{
    /// <summary>
    /// Validates portuguese phone numbers
    /// </summary>
    public static class PhoneNumber
	{
		private static readonly Regex internationalPhonePattern = new Regex(@"^(\+|00)\d+$", RegexOptions.Compiled);
		private static readonly Regex shortPhonePattern = new Regex(@"^1\d+$", RegexOptions.Compiled);
		private static readonly Regex fixedPhonePattern = new Regex(@"^2[1-9]\d{7,}$", RegexOptions.Compiled);
		private static readonly Regex nomadicPhonePattern = new Regex(@"^3\d$+", RegexOptions.Compiled);
		private static readonly Regex audioTextPhonePattern = new Regex(@"^6\d+$", RegexOptions.Compiled);
		private static readonly Regex privatePhonePattern = new Regex(@"^7\d+$", RegexOptions.Compiled);
		private static readonly Regex freePhonePattern = new Regex(@"^8\d+$", RegexOptions.Compiled);
		private static readonly Regex mobilePhonePattern = new Regex(@"^9[1263]\d{7,}$", RegexOptions.Compiled);

		/// <summary>
		/// Gets the type of the specified phone number.
		/// </summary>
		/// <param name="phoneNumber">The phone number.</param>
		/// <returns></returns>
		public static PhoneNumberTypes GetType(string phoneNumber)
		{
			if (internationalPhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.International;
			}
			if (shortPhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.ShortNumber;
			}
			if (fixedPhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.FixedService;
			}
			if (nomadicPhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.NomadicService;
			}
			if (audioTextPhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.AudioText;
			}
			if (privatePhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.PrivateVoiceNetwork;
			}
			if (freePhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.FreeServices;
			}
			if (mobilePhonePattern.IsMatch(phoneNumber))
			{
				return PhoneNumberTypes.MobileCommunicationServices;
			}

			return PhoneNumberTypes.None;
		}

		/// <summary>
		/// Determines whether the specified phone number is valid.
		/// </summary>
		/// <param name="phoneNumber">The phone number.</param>
		/// <param name="validTypes">The valid types.</param>
		/// <returns>
		/// 	<c>true</c> if the specified phone number is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string phoneNumber, PhoneNumberTypes validTypes)
		{
			return (GetType(phoneNumber) & validTypes) != 0;
		}
	}
}
