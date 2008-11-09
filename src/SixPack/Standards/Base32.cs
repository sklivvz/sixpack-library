// Base32.cs 
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

namespace SixPack.Standards
{
	/// <summary>
	/// This class provides functions for Base32 encoding and decoding.
	/// </summary>
	public static class Base32
	{
		/// <summary>
		/// First valid character that can be indexed in decode lookup table
		/// </summary>
		private const int charDigitsBase = '2';

		private const String ERROR_INVALID_CHAR = "invalid character in Base32-encoded string.";
		private const String ERROR_NON_CANONICAL_END = "Non canonical bits at end of Base32-encoded string.";
		private const String ERROR_NON_CANONICAL_LENGTH = "Non canonical Base32-encoded string length.";

		/// <summary>
		/// Lookup table used to canonically encode() groups of data bits
		/// </summary>
		private static readonly char[] canonicalChars = {
		                                                	'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
		                                                	'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '2', '3',
		                                                	'4', '5', '6', '7'
		                                                };

		/// <summary>
		/// Lookup table used to decode() characters in encoded strings
		/// </summary>
		private static readonly int[] charDigits = {
		                                           	26, 27, 28, 29, 30, 31, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4,
		                                           	5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
		                                           	25, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
		                                           	14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25
		                                           };

		/// <summary>
		/// Encode an array of binary bytes into a Base32 string.
		/// Should not fail (the only possible exception is that the
		/// returned string cannot be allocated in memory).
		/// </summary>
		/// <param name="value">Array of bytes to encode.</param>
		/// <returns>Base32 string corresponding to the byte array passed.</returns>
		public static String Encode(byte[] value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			int bytesOffset = 0;
			int bytesLen = value.Length;
			int charsOffset = 0;
			int charsLen = ((bytesLen << 3) + 4)/5;
			char[] chars = new char[charsLen];

			while (bytesLen != 0)
			{
				int digit, lastDigit;
				// INVARIANTS FOR EACH STEP n in [0..5[; digit in [0..31[;
				// The remaining n bits are already aligned on top positions
				// of the 5 least bits of digit, the other bits are 0.

				// STEP n = 0: insert new 5 bits, leave 3 bits
				digit = value[bytesOffset] & 255;
				chars[charsOffset] = canonicalChars[(byte) ((uint) digit >> 3)];
				lastDigit = (digit & 7) << 2;
				if (bytesLen == 1)
				{
					// put the last 3 bits
					chars[charsOffset + 1] = canonicalChars[lastDigit];
					break;
				}

				// STEP n = 3: insert 2 new bits, then 5 bits, leave 1 bit
				digit = value[bytesOffset + 1] & 255;
				chars[charsOffset + 1] = canonicalChars[(byte) ((uint) digit >> 6) | lastDigit];
				chars[charsOffset + 2] = canonicalChars[(byte) ((uint) digit >> 1) & 31];
				lastDigit = (digit & 1) << 4;
				if (bytesLen == 2)
				{
					// put the last 1 bit
					chars[charsOffset + 3] = canonicalChars[lastDigit];
					break;
				}
				////// STEP n = 1: insert 4 new bits, leave 4 bit
				digit = value[bytesOffset + 2] & 255;
				chars[charsOffset + 3] = canonicalChars[(byte) ((uint) digit >> 4) | lastDigit];
				lastDigit = (digit & 15) << 1;
				if (bytesLen == 3)
				{
					// put the last 1 bits
					chars[charsOffset + 4] = canonicalChars[lastDigit];
					break;
				}
				////// STEP n = 4: insert 1 new bit, then 5 bits, leave 2 bits
				digit = value[bytesOffset + 3] & 255;
				chars[charsOffset + 4] = canonicalChars[(byte) ((uint) digit >> 7) | lastDigit];
				chars[charsOffset + 5] = canonicalChars[(byte) ((uint) digit >> 2) & 31];
				lastDigit = (digit & 3) << 3;
				if (bytesLen == 4)
				{
					// put the last 2 bits
					chars[charsOffset + 6] = canonicalChars[lastDigit];
					break;
				}
				////// STEP n = 2: insert 3 new bits, then 5 bits, leave 0 bit
				digit = value[bytesOffset + 4] & 255;
				chars[charsOffset + 6] = canonicalChars[(byte) ((uint) digit >> 5) | lastDigit];
				chars[charsOffset + 7] = canonicalChars[digit & 31];
				//// This point is always reached for bytes.length multiple of 5
				bytesOffset += 5;
				charsOffset += 8;
				bytesLen -= 5;
			}
			return new String(chars);
		}

		/// <summary>
		/// Decode a Base32 encoded string into an array of binary bytes.
		/// May fail if the parameter is a non canonical Base32 string
		/// (the only other possible exception is that the
		/// returned array cannot be allocated in memory)
		/// </summary>
		/// <param name="encoded">The encoded value.</param>
		/// <returns></returns>
		public static byte[] Decode(String encoded)
		{
			if (encoded == null)
				throw  new ArgumentNullException("encoded");
			// clean up
			// all to uppercase, 1 and 0 do not exist in the vocabulary, probably mistyped?
			encoded = encoded.ToUpperInvariant().Replace('0', 'O').Replace('1', 'I');

			char[] chars = encoded.ToCharArray(); // avoids using charAt()
			int charsLen = chars.Length;
			// Note that the code below detects could detect non canonical
			// Base32 length within the loop. However canonical Base32 length
			// can be tested before entering the loop.
			// A canonical Base32 length modulo 8 cannot be:
			// 1 (aborts discarding 5 bits at STEP n=0 which produces no byte),
			// 3 (aborts discarding 7 bits at STEP n=2 which produces no byte),
			// 6 (aborts discarding 6 bits at STEP n=1 which produces no byte).
			switch (charsLen & 7)
			{
					// test the length of last subblock
				case 1: //  5 bits in subblock:  0 useful bits but 5 discarded
				case 3: // 15 bits in subblock:  8 useful bits but 7 discarded
				case 6: // 30 bits in subblock: 24 useful bits but 6 discarded
					throw new ArgumentException(ERROR_NON_CANONICAL_LENGTH);
			}
			int charDigitsLen = charDigits.Length;
			int bytesLen = (charsLen*5) >> 3;
			byte[] bytes = new byte[bytesLen];
			int bytesOffset = 0, charsOffset = 0;
			// Also the code below does test that other discarded bits
			// (1 to 4 bits at end) are effectively 0.
			while (charsLen > 0)
			{
				int digit, lastDigit;
				// STEP n = 0: Read the 1st Char in a 8-Chars subblock
				// Leave 5 bits, asserting there's another encoding Char
				if ((digit = chars[charsOffset] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				lastDigit = digit << 3;
				// STEP n = 5: Read the 2nd Char in a 8-Chars subblock
				// Insert 3 bits, leave 2 bits, possibly trailing if no more Char
				if ((digit = chars[charsOffset + 1] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				bytes[bytesOffset] = (byte) ((digit >> 2) | lastDigit);
				lastDigit = (digit & 3) << 6;
				if (charsLen == 2)
				{
					if (lastDigit != 0)
						throw new ArgumentException(ERROR_NON_CANONICAL_END);
					break; // discard the 2 trailing null bits
				}
				// STEP n = 2: Read the 3rd Char in a 8-Chars subblock
				// Leave 7 bits, asserting there's another encoding Char
				if ((digit = chars[charsOffset + 2] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				lastDigit |= (byte) (digit << 1);
				// STEP n = 7: Read the 4th Char in a 8-chars Subblock
				// Insert 1 bit, leave 4 bits, possibly trailing if no more Char
				if ((digit = chars[charsOffset + 3] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				bytes[bytesOffset + 1] = (byte) ((digit >> 4) | lastDigit);
				lastDigit = (byte) ((digit & 15) << 4);
				if (charsLen == 4)
				{
					if (lastDigit != 0)
						throw new ArgumentException(ERROR_NON_CANONICAL_END);
					break; // discard the 4 trailing null bits
				}
				// STEP n = 4: Read the 5th Char in a 8-Chars subblock
				// Insert 4 bits, leave 1 bit, possibly trailing if no more Char
				if ((digit = chars[charsOffset + 4] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				bytes[bytesOffset + 2] = (byte) ((digit >> 1) | lastDigit);
				lastDigit = (byte) ((digit & 1) << 7);
				if (charsLen == 5)
				{
					if (lastDigit != 0)
						throw new ArgumentException(ERROR_NON_CANONICAL_END);
					break; // discard the 1 trailing null bit
				}
				// STEP n = 1: Read the 6th Char in a 8-Chars subblock
				// Leave 6 bits, asserting there's another encoding Char
				if ((digit = chars[charsOffset + 5] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				lastDigit |= (byte) (digit << 2);
				// STEP n = 6: Read the 7th Char in a 8-Chars subblock
				// Insert 2 bits, leave 3 bits, possibly trailing if no more Char
				if ((digit = chars[charsOffset + 6] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				bytes[bytesOffset + 3] = (byte) ((digit >> 3) | lastDigit);
				lastDigit = (byte) ((digit & 7) << 5);
				if (charsLen == 7)
				{
					if (lastDigit != 0)
						throw new ArgumentException(ERROR_NON_CANONICAL_END);
					break; // discard the 3 trailing null bits
				}
				// STEP n = 3: Read the 8th Char in a 8-Chars subblock
				// Insert 5 bits, leave 0 bit, next encoding Char may not exist
				if ((digit = chars[charsOffset + 7] - charDigitsBase) < 0
				    || digit >= charDigitsLen
				    || (digit = charDigits[digit]) == -1)
					throw new ArgumentException(ERROR_INVALID_CHAR);
				bytes[bytesOffset + 4] = (byte) (digit | lastDigit);
				//// This point is always reached for chars.length multiple of 8
				charsOffset += 8;
				bytesOffset += 5;
				charsLen -= 8;
			}
			// On loop exit, discard the n trailing null bits
			return bytes;
		}
	}
}
