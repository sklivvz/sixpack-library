// CreditCard.cs 
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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SixPack.Banking
{
	/// <summary>
	/// Performs validation of credit card numbers
	/// </summary>
	public static class CreditCard
	{
		/// <summary>
		/// Gets the type of the credit card with the specified number.
		/// </summary>
		/// <param name="creditCardNumber">The credit card number.</param>
		/// <returns></returns>
		public static CreditCardTypes GetType(string creditCardNumber)
		{
			if (creditCardNumber == null)
			{
				throw new ArgumentNullException("creditCardNumber");
			}

			// AMEX -- 34 or 37 -- 15 length
			if (Regex.IsMatch(creditCardNumber, "^(34|37)") && 15 == creditCardNumber.Length)
			{
				return CreditCardTypes.AmericanExpress;
			}

			// MasterCard -- 51 through 55 -- 16 length
			if (Regex.IsMatch(creditCardNumber, "^(51|52|53|54|55)") && 16 == creditCardNumber.Length)
			{
				return CreditCardTypes.MasterCard;
			}

			// VISA -- 4 -- 13 and 16 length
			if (Regex.IsMatch(creditCardNumber, "^(4)") && (13 == creditCardNumber.Length || 16 == creditCardNumber.Length))
			{
				return CreditCardTypes.Visa;
			}

			// Diners Club -- 300-305, 36 or 38 -- 14 length
			if (Regex.IsMatch(creditCardNumber, "^(300|301|302|303|304|305|36|38)") && 14 == creditCardNumber.Length)
			{
				return CreditCardTypes.DinersClub;
			}

			// enRoute -- 2014,2149 -- 15 length
			if (Regex.IsMatch(creditCardNumber, "^(2014|2149)") && 15 == creditCardNumber.Length)
			{
				return CreditCardTypes.DinersClub;
			}

			// Discover -- 6011 -- 16 length
			if (Regex.IsMatch(creditCardNumber, "^(6011)") && 16 == creditCardNumber.Length)
			{
				return CreditCardTypes.Discover;
			}

			// JCB -- 3 -- 16 length
			if (Regex.IsMatch(creditCardNumber, "^(3)") && 16 == creditCardNumber.Length)
			{
				return CreditCardTypes.Jcb;
			}

			// JCB -- 2131, 1800 -- 15 length
			if (Regex.IsMatch(creditCardNumber, "^(2131|1800)") && 15 == creditCardNumber.Length)
			{
				return CreditCardTypes.Jcb;
			}

			// Card type wasn't recognised.
			return CreditCardTypes.Unknown;
		}

		/// <summary>
		/// Determines whether the specified credit card number is valid.
		/// </summary>
		/// <param name="creditCardNumber">The credit card number.</param>
		/// <param name="validTypes">The valid types.</param>
		/// <param name="validateCheckDigit">if set to <c>true</c> validate the check digit.</param>
		/// <returns>
		/// 	<c>true</c> if the specified credit card number is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string creditCardNumber, CreditCardTypes validTypes, bool validateCheckDigit)
		{
			return IsValid(creditCardNumber, validTypes) && (validateCheckDigit || IsCheckDigitValid(creditCardNumber));
		}

		/// <summary>
		/// Determines whether the specified credit card number is valid. Does not validate the check digit.
		/// </summary>
		/// <param name="creditCardNumber">The credit card number.</param>
		/// <param name="validTypes">The valid types.</param>
		/// <returns>
		/// 	<c>true</c> if the specified credit card number is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string creditCardNumber, CreditCardTypes validTypes)
		{
			return (GetType(creditCardNumber) & validTypes) != 0;
		}

		#region Private implementation
		/// <summary>
		/// Performs a validation using Luhn's Formula.
		/// </summary>
		private static bool IsCheckDigitValid(string cardNumber)
		{
			if (cardNumber == null)
				throw new ArgumentNullException("cardNumber");

			try
			{
				// Array to contain individual numbers
				List<int> CheckNumbers = new List<int>();

				// So, get length of card
				int CardLength = cardNumber.Length;

				// Double the value of alternate digits, starting with the second digit
				// from the right, i.e. back to front.

				// Loop through starting at the end
				for (int i = CardLength - 2; i >= 0; i = i - 2)
				{
					// Now read the contents at each index, this
					// can then be stored as an array of integers

					// Double the number returned
					CheckNumbers.Add(Int32.Parse(cardNumber[i].ToString(), CultureInfo.InvariantCulture) * 2);
				}

				int CheckSum = 0; // Will hold the total sum of all checksum digits

				// Second stage, add separate digits of all products
				for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++)
				{
					int _count = 0; // will hold the sum of the digits

					// determine if current number has more than one digit
					if (CheckNumbers[iCount] > 9)
					{
						int _numLength = (CheckNumbers[iCount]).ToString(CultureInfo.InvariantCulture).Length;
						// add count to each digit
						for (int x = 0; x < _numLength; x++)
						{
							_count = _count +
									 Int32.Parse(
										(CheckNumbers[iCount]).ToString(CultureInfo.InvariantCulture)[x].ToString(
											CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
						}
					}
					else
						_count = CheckNumbers[iCount]; // single digit, just add it by itself

					CheckSum = CheckSum + _count; // add sum to the total sum
				}

				// Stage 3, add the unaffected digits
				// Add all the digits that we didn't double still starting from the right
				// but this time we'll start from the rightmost number with alternating digits
				int OriginalSum = 0;
				for (int y = CardLength - 1; y >= 0; y = y - 2)
					OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString(), CultureInfo.InvariantCulture);

				// Perform the final calculation, if the sum Mod 10 results in 0 then
				// it's valid, otherwise its false.
				return (((OriginalSum + CheckSum) % 10) == 0);
			}
			catch (ApplicationException)
			{
				return false;
			}
		}
		#endregion
	}
}
