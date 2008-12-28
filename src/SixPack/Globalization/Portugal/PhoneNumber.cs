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

using System;
using System.Text.RegularExpressions;
using SixPack.Properties;

namespace SixPack.Globalization.Portugal
{
	/// <summary>
	/// Validates portuguese phone numbers
	/// </summary>
	public static class PhoneNumber
	{
		private static readonly Regex internationalPhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_International, RegexOptions.Compiled);
		private static readonly Regex shortPhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_ShortNumber, RegexOptions.Compiled);
		private static readonly Regex fixedPhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_FixedService, RegexOptions.Compiled);
		private static readonly Regex nomadicPhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_NomadicService, RegexOptions.Compiled);
		private static readonly Regex audioTextPhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_AudioText, RegexOptions.Compiled);
		private static readonly Regex privatePhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_PrivateVoiceNetwork, RegexOptions.Compiled);
		private static readonly Regex freePhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_FreeServices, RegexOptions.Compiled);
		private static readonly Regex mobilePhonePattern = new Regex(Settings.Default.Globalization_Portugal_PhoneNumber_PhonePattern_MobileCommunicationServices, RegexOptions.Compiled);

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
