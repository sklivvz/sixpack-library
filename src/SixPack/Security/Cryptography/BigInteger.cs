// BigInteger.cs 
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

#undef biginteger_test

//************************************************************************************
// BigInteger Class Version 1.03
//
// Copyright (c) 2002 Chew Keong TAN
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, provided that the above
// copyright notice(s) and this permission notice appear in all copies of
// the Software and that both the above copyright notice(s) and this
// permission notice appear in supporting documentation.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
// OF THIRD PARTY RIGHTS. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// HOLDERS INCLUDED IN THIS NOTICE BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL
// INDIRECT OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING
// FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT,
// NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION
// WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
//
//
// Disclaimer
// ----------
// Although reasonable care has been taken to ensure the correctness of this
// implementation, this code should never be used in any application without
// proper verification and testing.  I disclaim all liability and responsibility
// to any person or entity with respect to any loss or damage caused, or alleged
// to be caused, directly or indirectly, by the use of this BigInteger class.
//
// Comments, bugs and suggestions to
// (http://www.codeproject.com/csharp/biginteger.asp)
//
//
// Overloaded Operators +, -, *, /, %, >>, <<, ==, !=, >, <, >=, <=, &, |, ^, ++, --, ~
//
// Features
// --------
// 1) Arithmetic operations involving large signed integers (2's complement).
// 2) Primality test using Fermat little theorm, Rabin Miller's method,
//    Solovay Strassen's method and Lucas strong pseudoprime.
// 3) Modulo exponential with Barrett's reduction.
// 4) Inverse modulo.
// 5) Pseudo prime generation.
// 6) Co-prime generation.
//
//
// Known Problem
// -------------
// This pseudoprime passes my implementation of
// primality test but failed in JDK's isProbablePrime test.
//
//       byte[] pseudoPrime1 = { (byte)0x00,
//             (byte)0x85, (byte)0x84, (byte)0x64, (byte)0xFD, (byte)0x70, (byte)0x6A,
//             (byte)0x9F, (byte)0xF0, (byte)0x94, (byte)0x0C, (byte)0x3E, (byte)0x2C,
//             (byte)0x74, (byte)0x34, (byte)0x05, (byte)0xC9, (byte)0x55, (byte)0xB3,
//             (byte)0x85, (byte)0x32, (byte)0x98, (byte)0x71, (byte)0xF9, (byte)0x41,
//             (byte)0x21, (byte)0x5F, (byte)0x02, (byte)0x9E, (byte)0xEA, (byte)0x56,
//             (byte)0x8D, (byte)0x8C, (byte)0x44, (byte)0xCC, (byte)0xEE, (byte)0xEE,
//             (byte)0x3D, (byte)0x2C, (byte)0x9D, (byte)0x2C, (byte)0x12, (byte)0x41,
//             (byte)0x1E, (byte)0xF1, (byte)0xC5, (byte)0x32, (byte)0xC3, (byte)0xAA,
//             (byte)0x31, (byte)0x4A, (byte)0x52, (byte)0xD8, (byte)0xE8, (byte)0xAF,
//             (byte)0x42, (byte)0xF4, (byte)0x72, (byte)0xA1, (byte)0x2A, (byte)0x0D,
//             (byte)0x97, (byte)0xB1, (byte)0x31, (byte)0xB3,
//       };
//
//
// Change Log
// ----------
// 1) September 23, 2002 (Version 1.03)
//    - Fixed operator- to give correct data length.
//    - Added Lucas sequence generation.
//    - Added Strong Lucas Primality test.
//    - Added integer square root method.
//    - Added setBit/unsetBit methods.
//    - New isProbablePrime() method which do not require the
//      confident parameter.
//
// 2) August 29, 2002 (Version 1.02)
//    - Fixed bug in the exponentiation of negative numbers.
//    - Faster modular exponentiation using Barrett reduction.
//    - Added getBytes() method.
//    - Fixed bug in ToHexString method.
//    - Added overloading of ^ operator.
//    - Faster computation of Jacobi symbol.
//
// 3) August 19, 2002 (Version 1.01)
//    - Big integer is stored and manipulated as unsigned integers (4 bytes) instead of
//      individual bytes this gives significant performance improvement.
//    - Updated Fermat's Little Theorem test to use a^(p-1) mod p = 1
//    - Added isProbablePrime method.
//    - Updated documentation.
//
// 4) August 9, 2002 (Version 1.0)
//    - Initial Release.
//
//
// References
// [1] D. E. Knuth, "Seminumerical Algorithms", The Art of Computer Programming Vol. 2,
//     3rd Edition, Addison-Wesley, 1998.
//
// [2] K. H. Rosen, "Elementary Number Theory and Its Applications", 3rd Ed,
//     Addison-Wesley, 1993.
//
// [3] B. Schneier, "Applied Cryptography", 2nd Ed, John Wiley & Sons, 1996.
//
// [4] A. Menezes, P. van Oorschot, and S. Vanstone, "Handbook of Applied Cryptography",
//     CRC Press, 1996, www.cacr.math.uwaterloo.ca/hac
//
// [5] A. Bosselaers, R. Govaerts, and J. Vandewalle, "Comparison of Three Modular
//     Reduction Functions," Proc. CRYPTO'93, pp.175-186.
//
// [6] R. Baillie and S. S. Wagstaff Jr, "Lucas Pseudoprimes", Mathematics of Computation,
//     Vol. 35, No. 152, Oct 1980, pp. 1391-1417.
//
// [7] H. C. Williams, "douard Lucas and Primality Testing", Canadian Mathematical
//     Society Series of Monographs and Advance Texts, vol. 22, John Wiley & Sons, New York,
//     NY, 1998.
//
// [8] P. Ribenboim, "The new book of prime number records", 3rd edition, Springer-Verlag,
//     New York, NY, 1995.
//
// [9] M. Joye and J.-J. Quisquater, "Efficient computation of full Lucas sequences",
//     Electronics Letters, 32(6), 1996, pp 537-538.
//
//************************************************************************************

using System;
using System.Globalization;
using System.Text;

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// BigInteger Class
	/// Version 1.03
	/// Copyright (c) 2002 Chew Keong TAN
	/// </summary>
	[Serializable]
	public class BigInteger
	{
		// maximum length of the BigInteger in uint (4 bytes)
		// change this to suit the required level of precision.

		private const int maxLength = 500;

		// primes smaller than 2000 to test the generated prime number

		/// <summary>
		/// Primes smaller than 2000 to test the generated prime number
		/// </summary>
		private static readonly int[] primesBelow2000 = {
		                                                	2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61,
		                                                	67
		                                                	, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139
		                                                	,
		                                                	149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
		                                                	227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293,
		                                                	307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383,
		                                                	389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
		                                                	467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569,
		                                                	571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647,
		                                                	653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743,
		                                                	751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839,
		                                                	853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941,
		                                                	947, 953, 967, 971, 977, 983, 991, 997, 1009, 1013, 1019, 1021, 1031,
		                                                	1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097,
		                                                	1103
		                                                	, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193,
		                                                	1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279,
		                                                	1283
		                                                	, 1289, 1291, 1297, 1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367,
		                                                	1373, 1381, 1399, 1409, 1423, 1427, 1429, 1433, 1439, 1447, 1451,
		                                                	1453
		                                                	, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499, 1511, 1523, 1531,
		                                                	1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 1597, 1601, 1607,
		                                                	1609
		                                                	, 1613, 1619, 1621, 1627, 1637, 1657, 1663, 1667, 1669, 1693, 1697,
		                                                	1699, 1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759, 1777, 1783,
		                                                	1787
		                                                	, 1789, 1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877,
		                                                	1879, 1889, 1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973, 1979,
		                                                	1987
		                                                	, 1993, 1997, 1999
		                                                };

		private readonly uint[] data; // stores bytes from the Big Integer

		/// <summary>
		/// Number of actual chars used
		/// </summary>
		private int dataLength; // number of actual chars used

		//***********************************************************************
		// Constructor (Default value for BigInteger is 0
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		public BigInteger()
		{
			data = new uint[maxLength];
			dataLength = 1;
		}

		//***********************************************************************
		// Constructor (Default value provided by long)
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public BigInteger(long value)
		{
			unchecked
			{
				data = new uint[maxLength];
				long tempVal = value;

				// copy bytes from long to BigInteger without any assumption of
				// the length of the long datatype

				dataLength = 0;
				while (value != 0 && dataLength < maxLength)
				{
					data[dataLength] = (uint)(value & 0xFFFFFFFF);
					value >>= 32;
					dataLength++;
				}

				if (tempVal > 0) // overflow check for +ve value
				{
					if (value != 0 || (data[maxLength - 1] & 0x80000000) != 0)
						throw (new ArithmeticException("Positive overflow in constructor."));
				}
				else if (tempVal < 0) // underflow check for -ve value
				{
					if (value != -1 || (data[dataLength - 1] & 0x80000000) == 0)
						throw (new ArithmeticException("Negative underflow in constructor."));
				}

				if (dataLength == 0)
					dataLength = 1;
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by ulong)
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		[CLSCompliant(false)]
		public BigInteger(ulong value)
		{
			unchecked
			{
				data = new uint[maxLength];

				// copy bytes from ulong to BigInteger without any assumption of
				// the length of the ulong datatype

				dataLength = 0;
				while (value != 0 && dataLength < maxLength)
				{
					data[dataLength] = (uint)(value & 0xFFFFFFFF);
					value >>= 32;
					dataLength++;
				}

				if (value != 0 || (data[maxLength - 1] & 0x80000000) != 0)
					throw (new ArithmeticException("Positive overflow in constructor."));

				if (dataLength == 0)
					dataLength = 1;
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by BigInteger)
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="bi">The bi.</param>
		public BigInteger(BigInteger bi)
		{
			unchecked
			{
				if ((object)bi == null)
					throw new ArgumentNullException("bi");

				data = new uint[maxLength];

				dataLength = bi.dataLength;

				for (int i = 0; i < dataLength; i++)
					data[i] = bi.data[i];
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by a string of digits of the
		//              specified base)
		//
		// Example (base 10)
		// -----------------
		// To initialize "a" with the default value of 1234 in base 10
		//      BigInteger a = new BigInteger("1234", 10)
		//
		// To initialize "a" with the default value of -1234
		//      BigInteger a = new BigInteger("-1234", 10)
		//
		// Example (base 16)
		// -----------------
		// To initialize "a" with the default value of 0x1D4F in base 16
		//      BigInteger a = new BigInteger("1D4F", 16)
		//
		// To initialize "a" with the default value of -0x1D4F
		//      BigInteger a = new BigInteger("-1D4F", 16)
		//
		// Note that string values are specified in the <sign><magnitude>
		// format.
		//
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="radix">The radix.</param>
		public BigInteger(string value, int radix)
		{
			unchecked
			{
				if (value == null)
					throw new ArgumentNullException("value");
				BigInteger multiplier = new BigInteger(1);
				BigInteger result = new BigInteger();

				value = (value.ToUpperInvariant()).Trim();
				int limit = 0;

				if (value[0] == '-')
					limit = 1;

				for (int i = value.Length - 1; i >= limit; i--)
				{
					int posVal = value[i];

					if (posVal >= '0' && posVal <= '9')
						posVal -= '0';
					else if (posVal >= 'A' && posVal <= 'Z')
						posVal = (posVal - 'A') + 10;
					else
						posVal = 9999999; // arbitrary large

					if (posVal >= radix)
						throw (new ArithmeticException("Invalid string in constructor."));
					else
					{
						if (value[0] == '-')
							posVal = -posVal;

						result = result + (multiplier * posVal);

						if ((i - 1) >= limit)
							multiplier = multiplier * radix;
					}
				}

				if (value[0] == '-') // negative values
				{
					if ((result.data[maxLength - 1] & 0x80000000) == 0)
						throw (new ArithmeticException("Negative underflow in constructor."));
				}
				else // positive values
				{
					if ((result.data[maxLength - 1] & 0x80000000) != 0)
						throw (new ArithmeticException("Positive overflow in constructor."));
				}

				data = new uint[maxLength];
				for (int i = 0; i < result.dataLength; i++)
					data[i] = result.data[i];

				dataLength = result.dataLength;
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by an array of bytes)
		//
		// The lowest index of the input byte array (i.e [0]) should contain the
		// most significant byte of the number, and the highest index should
		// contain the least significant byte.
		//
		// E.g.
		// To initialize "a" with the default value of 0x1D4F in base 16
		//      byte[] temp = { 0x1D, 0x4F };
		//      BigInteger a = new BigInteger(temp)
		//
		// Note that this method of initialization does not allow the
		// sign to be specified.
		//
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="inData">The in data.</param>
		public BigInteger(byte[] inData)
		{
			unchecked
			{
				if (inData == null)
					throw new ArgumentNullException("inData");
				dataLength = inData.Length >> 2;

				int leftOver = inData.Length & 0x3;
				if (leftOver != 0) // length not multiples of 4
					dataLength++;

				if (dataLength > maxLength)
					throw (new ArithmeticException("Byte overflow in constructor."));

				data = new uint[maxLength];

				for (int i = inData.Length - 1, j = 0; i >= 3; i -= 4, j++)
				{
					data[j] = (uint)((inData[i - 3] << 24) + (inData[i - 2] << 16) +
						(inData[i - 1] << 8) + inData[i]);
				}

				if (leftOver == 1)
					data[dataLength - 1] = inData[0];
				else if (leftOver == 2)
					data[dataLength - 1] = (uint)((inData[0] << 8) + inData[1]);
				else if (leftOver == 3)
					data[dataLength - 1] = (uint)((inData[0] << 16) + (inData[1] << 8) + inData[2]);

				while (dataLength > 1 && data[dataLength - 1] == 0)
					dataLength--;

				//Console.WriteLine("Len = " + dataLength);
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by an array of bytes of the
		// specified length.)
		//***********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="inData">The in data.</param>
		/// <param name="inLen">The in len.</param>
		public BigInteger(byte[] inData, int inLen)
		{
			unchecked
			{
				if (inData == null)
					throw new ArgumentNullException("inData");
				dataLength = inLen >> 2;

				int leftOver = inLen & 0x3;
				if (leftOver != 0) // length not multiples of 4
					dataLength++;

				if (dataLength > maxLength || inLen > inData.Length)
					throw (new ArithmeticException("Byte overflow in constructor."));

				data = new uint[maxLength];

				for (int i = inLen - 1, j = 0; i >= 3; i -= 4, j++)
				{
					data[j] = (uint)((inData[i - 3] << 24) + (inData[i - 2] << 16) +
						(inData[i - 1] << 8) + inData[i]);
				}

				if (leftOver == 1)
					data[dataLength - 1] = inData[0];
				else if (leftOver == 2)
					data[dataLength - 1] = (uint)((inData[0] << 8) + inData[1]);
				else if (leftOver == 3)
					data[dataLength - 1] = (uint)((inData[0] << 16) + (inData[1] << 8) + inData[2]);

				if (dataLength == 0)
					dataLength = 1;

				while (dataLength > 1 && data[dataLength - 1] == 0)
					dataLength--;

				//Console.WriteLine("Len = " + dataLength);
			}
		}

		//***********************************************************************
		// Constructor (Default value provided by an array of unsigned integers)
		//*********************************************************************

		/// <summary>
		/// Initializes a new instance of the <see cref="BigInteger"/> class.
		/// </summary>
		/// <param name="inData">The in data.</param>
		[CLSCompliant(false)]
		public BigInteger(uint[] inData)
		{
			unchecked
			{
				if (inData == null)
					throw new ArgumentNullException("inData");
				dataLength = inData.Length;

				if (dataLength > maxLength)
					throw (new ArithmeticException("Byte overflow in constructor."));

				data = new uint[maxLength];

				for (int i = dataLength - 1, j = 0; i >= 0; i--, j++)
					data[j] = inData[i];

				while (dataLength > 1 && data[dataLength - 1] == 0)
					dataLength--;

				//Console.WriteLine("Len = " + dataLength);
			}
		}

		/// <summary>
		/// Number of actual chars used
		/// </summary>
		public int DataLength
		{
			get { return dataLength; }
			set { dataLength = value; }
		}

		//***********************************************************************
		// Overloading of the typecast operator.
		// For BigInteger bi = 10;
		//***********************************************************************

		/// <summary>
		/// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="SixPack.Security.Cryptography.BigInteger"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator BigInteger(long value)
		{
			return (new BigInteger(value));
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="System.UInt64"/> to <see cref="SixPack.Security.Cryptography.BigInteger"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigInteger(ulong value)
		{
			return (new BigInteger(value));
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="SixPack.Security.Cryptography.BigInteger"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator BigInteger(int value)
		{
			return (new BigInteger(value));
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="System.UInt32"/> to <see cref="SixPack.Security.Cryptography.BigInteger"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		[CLSCompliant(false)]
		public static implicit operator BigInteger(uint value)
		{
			return (new BigInteger((ulong) value));
		}

		//***********************************************************************
		// Overloading of addition operator
		//***********************************************************************

		/// <summary>
		/// Adds the specified operands.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public BigInteger Add(BigInteger operand)
		{
			return this + operand;
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator +(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					return null;
				if ((object)secondOperand == null)
					return null;
				BigInteger result = new BigInteger();

				result.dataLength = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;

				long carry = 0;
				for (int i = 0; i < result.dataLength; i++)
				{
					long sum = operand.data[i] + (long)secondOperand.data[i] + carry;
					carry = sum >> 32;
					result.data[i] = (uint)(sum & 0xFFFFFFFF);
				}

				if (carry != 0 && result.dataLength < maxLength)
				{
					result.data[result.dataLength] = (uint)(carry);
					result.dataLength++;
				}

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				// overflow check
				int lastPos = maxLength - 1;
				if ((operand.data[lastPos] & 0x80000000) == (secondOperand.data[lastPos] & 0x80000000) &&
					(result.data[lastPos] & 0x80000000) != (operand.data[lastPos] & 0x80000000))
					throw (new ArithmeticException());

				return result;
			}
		}

		//***********************************************************************
		// Overloading of the unary ++ operator
		//***********************************************************************

		/// <summary>
		/// Increments this.
		/// </summary>
		/// <returns></returns>
		public BigInteger Increment()
		{
			BigInteger operand = this;
			operand++;
			return operand;
		}

		/// <summary>
		/// Implements the operator ++.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator ++(BigInteger operand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				BigInteger result = new BigInteger(operand);

				long val, carry = 1;
				int index = 0;

				while (carry != 0 && index < maxLength)
				{
					val = (long)(result.data[index]);
					val++;

					result.data[index] = (uint)(val & 0xFFFFFFFF);
					carry = val >> 32;

					index++;
				}

				if (index > result.dataLength)
					result.dataLength = index;
				else
				{
					while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
						result.dataLength--;
				}

				// overflow check
				int lastPos = maxLength - 1;

				// overflow if initial value was +ve but ++ caused a sign
				// change to negative.

				if ((operand.data[lastPos] & 0x80000000) == 0 &&
					(result.data[lastPos] & 0x80000000) != (operand.data[lastPos] & 0x80000000))
					throw (new ArithmeticException("Overflow in ++."));
				return result;
			}
		}

		//***********************************************************************
		// Overloading of subtraction operator
		//***********************************************************************

		/// <summary>
		/// Subtracts the specified operand from this.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public BigInteger Subtract(BigInteger operand)
		{
			return this - operand;
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator -(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");
				BigInteger result = new BigInteger();

				result.dataLength = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;

				long carryIn = 0;
				for (int i = 0; i < result.dataLength; i++)
				{
					long diff;

					diff = operand.data[i] - (long)secondOperand.data[i] - carryIn;
					result.data[i] = (uint)(diff & 0xFFFFFFFF);

					if (diff < 0)
						carryIn = 1;
					else
						carryIn = 0;
				}

				// roll over to negative
				if (carryIn != 0)
				{
					for (int i = result.dataLength; i < maxLength; i++)
						result.data[i] = 0xFFFFFFFF;
					result.dataLength = maxLength;
				}

				// fixed in v1.03 to give correct datalength for a - (-b)
				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				// overflow check

				int lastPos = maxLength - 1;
				if ((operand.data[lastPos] & 0x80000000) != (secondOperand.data[lastPos] & 0x80000000) &&
					(result.data[lastPos] & 0x80000000) != (operand.data[lastPos] & 0x80000000))
					throw (new ArithmeticException());

				return result;
			}
		}

		//***********************************************************************
		// Overloading of the unary -- operator
		//***********************************************************************

		/// <summary>
		/// Decrements this.
		/// </summary>
		public BigInteger Decrement()
		{
			BigInteger wtf = this;
			wtf--;
			return wtf;
		}

		/// <summary>
		/// Implements the operator --.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator --(BigInteger operand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");

				BigInteger result = new BigInteger(operand);

				long val;
				bool carryIn = true;
				int index = 0;

				while (carryIn && index < maxLength)
				{
					val = (result.data[index]);
					val--;

					result.data[index] = (uint)(val & 0xFFFFFFFF);

					if (val >= 0)
						carryIn = false;

					index++;
				}

				if (index > result.dataLength)
					result.dataLength = index;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				// overflow check
				int lastPos = maxLength - 1;

				// overflow if initial value was -ve but -- caused a sign
				// change to positive.

				if ((operand.data[lastPos] & 0x80000000) != 0 &&
					(result.data[lastPos] & 0x80000000) != (operand.data[lastPos] & 0x80000000))
					throw (new ArithmeticException("Underflow in --."));

				return result;
			}
		}

		//***********************************************************************
		// Overloading of multiplication operator
		//***********************************************************************

		/// <summary>
		/// Multiplies this instance with the specified operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>The result of the operator.</returns>
		public BigInteger Multiply(BigInteger operand)
		{
			return this*operand;
		}

		/// <summary>
		/// Implements the operator *.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator *(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");
				int lastPos = maxLength - 1;
				bool operandNeg = false, secondOperandNeg = false;

				// take the absolute value of the inputs
				//try
				//{
				if ((operand.data[lastPos] & 0x80000000) != 0) // operand negative
				{
					operandNeg = true;
					operand = -operand;
				}
				if ((secondOperand.data[lastPos] & 0x80000000) != 0) // secondOperand negative
				{
					secondOperandNeg = true;
					secondOperand = -secondOperand;
				}
				//}
				//catch (ApplicationException)
				//{
				//}

				BigInteger result = new BigInteger();

				// multiply the absolute values
				for (int i = 0; i < operand.dataLength; i++)
				{
					if (operand.data[i] == 0)
						continue;

					ulong mcarry = 0;
					for (int j = 0, k = i; j < secondOperand.dataLength; j++, k++)
					{
						// k = i + j
						if (k > maxLength)
						{
							throw new ArithmeticException("Multiplication overflow.");
						}

						ulong val = (operand.data[i] * (ulong)secondOperand.data[j]) + result.data[k] + mcarry;

						result.data[k] = (uint)(val & 0xFFFFFFFF);
						mcarry = (val >> 32);
					}

					if (mcarry != 0)
						result.data[i + secondOperand.dataLength] = (uint)mcarry;
				}

				result.dataLength = operand.dataLength + secondOperand.dataLength;
				if (result.dataLength > maxLength)
					result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				// overflow check (result is -ve)
				if ((result.data[lastPos] & 0x80000000) != 0)
				{
					if (operandNeg != secondOperandNeg && result.data[lastPos] == 0x80000000) // different sign
					{
						// handle the special case where multiplication produces
						// a max negative number in 2's complement.

						if (result.dataLength == 1)
							return result;
						else
						{
							bool isMaxNeg = true;
							for (int i = 0; i < result.dataLength - 1 && isMaxNeg; i++)
							{
								if (result.data[i] != 0)
									isMaxNeg = false;
							}

							if (isMaxNeg)
								return result;
						}
					}

					throw (new ArithmeticException("Multiplication overflow."));
				}

				// if input has different signs, then result is -ve
				if (operandNeg != secondOperandNeg)
					return -result;

				return result;
			}
		}

		//***********************************************************************
		// Overloading of unary << operators
		//***********************************************************************

		/// <summary>
		/// Left shifts this by the specified value
		/// </summary>
		/// <param name="shiftValue">The shift value.</param>
		/// <returns>The result of the operator.</returns>
		public BigInteger LeftShift(int shiftValue)
		{
			return this << shiftValue;
		}

		/// <summary>
		/// Implements the operator &lt;&lt;.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="shiftValue">The shift value.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator <<(BigInteger operand, int shiftValue)
		{
			if ((object) operand == null)
				throw new ArgumentNullException("operand");
			BigInteger result = new BigInteger(operand);
			result.dataLength = shiftLeft(result.data, shiftValue);
			return result;
		}

		// least significant bits at lower part of buffer

		private static int shiftLeft(uint[] buffer, int shiftValue)
		{
			unchecked
			{
				int shiftAmount = 32;
				int bufLen = buffer.Length;

				while (bufLen > 1 && buffer[bufLen - 1] == 0)
					bufLen--;

				for (int count = shiftValue; count > 0;)
				{
					if (count < shiftAmount)
						shiftAmount = count;

					//Console.WriteLine("shiftAmount = {0}", shiftAmount);

					ulong carry = 0;
					for (int i = 0; i < bufLen; i++)
					{
						ulong val = ((ulong)buffer[i]) << shiftAmount;
						val |= carry;

						buffer[i] = (uint)(val & 0xFFFFFFFF);
						carry = val >> 32;
					}

					if (carry != 0)
					{
						if (bufLen + 1 <= buffer.Length)
						{
							buffer[bufLen] = (uint)carry;
							bufLen++;
						}
					}
					count -= shiftAmount;
				}
				return bufLen;
			}
		}

		//***********************************************************************
		// Overloading of unary >> operators
		//***********************************************************************

		/// <summary>
		/// Right shifts this by the specified value
		/// </summary>
		/// <param name="shiftValue">The shift value.</param>
		/// <returns>The result of the operator.</returns>
		public BigInteger RightShift(int shiftValue)
		{
			return this >> shiftValue;
		}

		/// <summary>
		/// Implements the operator &gt;&gt;.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="shiftValue">The shift value.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator >>(BigInteger operand, int shiftValue)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				BigInteger result = new BigInteger(operand);
				result.dataLength = shiftRight(result.data, shiftValue);

				if ((operand.data[maxLength - 1] & 0x80000000) != 0) // negative
				{
					for (int i = maxLength - 1; i >= result.dataLength; i--)
						result.data[i] = 0xFFFFFFFF;

					uint mask = 0x80000000;
					for (int i = 0; i < 32; i++)
					{
						if ((result.data[result.dataLength - 1] & mask) != 0)
							break;

						result.data[result.dataLength - 1] |= mask;
						mask >>= 1;
					}
					result.dataLength = maxLength;
				}

				return result;
			}
		}

		private static int shiftRight(uint[] buffer, int shiftVal)
		{
			unchecked
			{
				int shiftAmount = 32;
				int invShift = 0;
				int bufLen = buffer.Length;

				while (bufLen > 1 && buffer[bufLen - 1] == 0)
					bufLen--;

				//Console.WriteLine("bufLen = " + bufLen + " buffer.Length = " + buffer.Length);

				for (int count = shiftVal; count > 0;)
				{
					if (count < shiftAmount)
					{
						shiftAmount = count;
						invShift = 32 - shiftAmount;
					}

					//Console.WriteLine("shiftAmount = {0}", shiftAmount);

					ulong carry = 0;
					for (int i = bufLen - 1; i >= 0; i--)
					{
						ulong val = ((ulong)buffer[i]) >> shiftAmount;
						val |= carry;

						carry = ((ulong)buffer[i]) << invShift;
						buffer[i] = (uint)(val);
					}

					count -= shiftAmount;
				}

				while (bufLen > 1 && buffer[bufLen - 1] == 0)
					bufLen--;

				return bufLen;
			}
		}

		//***********************************************************************
		// Overloading of the NOT operator (1's complement)
		//***********************************************************************

		/// <summary>
		/// Returns NOT this (1's complement).
		/// </summary>
		/// <returns></returns>
		public BigInteger OnesComplement()
		{
			return ~this;
		}

		/// <summary>
		/// Implements the operator ~.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator ~(BigInteger operand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				BigInteger result = new BigInteger(operand);

				for (int i = 0; i < maxLength; i++)
					result.data[i] = ~(operand.data[i]);

				result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				return result;
			}
		}

		//***********************************************************************
		// Overloading of the NEGATE operator (2's complement)
		//***********************************************************************

		/// <summary>
		/// Negates this instance.
		/// </summary>
		/// <returns></returns>
		public BigInteger Negate()
		{
			return -this;
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator -(BigInteger operand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				// handle neg of zero separately since it'll cause an overflow
				// if we proceed.

				if (operand.dataLength == 1 && operand.data[0] == 0)
					return (new BigInteger());

				BigInteger result = new BigInteger(operand);

				// 1's complement
				for (int i = 0; i < maxLength; i++)
					result.data[i] = ~(operand.data[i]);

				// add one to result of 1's complement
				long carry = 1;
				int index = 0;

				while (carry != 0 && index < maxLength)
				{
					long val = result.data[index];
					val++;

					result.data[index] = (uint)(val & 0xFFFFFFFF);
					carry = val >> 32;

					index++;
				}

				if ((operand.data[maxLength - 1] & 0x80000000) == (result.data[maxLength - 1] & 0x80000000))
					throw (new ArithmeticException("Overflow in negation."));

				result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;
				return result;
			}
		}

		//***********************************************************************
		// Overloading of equality operator
		//***********************************************************************

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(BigInteger operand, BigInteger secondOperand)
		{
            if ((object)operand == null)
                return ((object)secondOperand == null);
			return operand.Equals(secondOperand);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(BigInteger operand, BigInteger secondOperand)
		{
			if ((object) operand == null)
                return ((object)secondOperand != null);
            return !(operand.Equals(secondOperand));
		}

		/// <summary>
		/// Returns <c>true</c> if "this" is equal to object o, <c>false</c> otherwise.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			return Equals(obj as BigInteger);
		}

		/// <summary>
		/// Returns <c>true</c> if "this" is equal to <see cref="BigInteger"/> o, <c>false</c> otherwise.
		/// </summary>
		/// <param name="obj">The <see cref="BigInteger"/> to compare to.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public bool Equals(BigInteger obj)
		{
			if (ReferenceEquals(obj, null))
			{
				return false;
			}

			if (dataLength != obj.dataLength)
			{
				return false;
			}

			for (int i = 0; i < dataLength; i++)
			{
				if (data[i] != obj.data[i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		//***********************************************************************
		// Overloading of inequality operator
		//***********************************************************************

		/// <summary>
		/// Compares this to the specified operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>Returns <c>true</c> if this > operand, <c>false</c> otherwise.</returns>
		public bool Compare(BigInteger operand)
		{
			return this > operand;
		}

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");
				int pos = maxLength - 1;

				// operand is negative, secondOperand is positive
				if ((operand.data[pos] & 0x80000000) != 0 && (secondOperand.data[pos] & 0x80000000) == 0)
					return false;

					// operand is positive, secondOperand is negative
				else if ((operand.data[pos] & 0x80000000) == 0 && (secondOperand.data[pos] & 0x80000000) != 0)
					return true;

				// same sign
				int len = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;
				pos = len - 1;
				while (pos >= 0 && operand.data[pos] == secondOperand.data[pos])
					pos--;

				if (pos >= 0)
				{
					if (operand.data[pos] > secondOperand.data[pos])
						return true;
					return false;
				}
				return false;
			}
		}

		/*
 *		/// <summary>
		/// Compares this to the specified operand
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns><c>true</c> if this is less than operand, <c>false</c> otherwise.</returns>
		public bool LessThan(BigInteger operand)
		{
			return this < operand;
		}
 */

		/// <summary>
		/// Implements the operator &lt;.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				int pos = maxLength - 1;

				// operand is negative, secondOperand is positive
				if ((operand.data[pos] & 0x80000000) != 0 && (secondOperand.data[pos] & 0x80000000) == 0)
					return true;

					// operand is positive, secondOperand is negative
				else if ((operand.data[pos] & 0x80000000) == 0 && (secondOperand.data[pos] & 0x80000000) != 0)
					return false;

				// same sign
				int len = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;
				pos = len - 1;
				while (pos >= 0 && operand.data[pos] == secondOperand.data[pos])
					pos--;

				if (pos >= 0)
				{
					if (operand.data[pos] < secondOperand.data[pos])
						return true;
					return false;
				}
				return false;
			}
		}

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >=(BigInteger operand, BigInteger secondOperand)
		{
			return (operand == secondOperand || operand > secondOperand);
		}

		/// <summary>
		/// Implements the operator &lt;=.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <=(BigInteger operand, BigInteger secondOperand)
		{
			return (operand == secondOperand || operand < secondOperand);
		}

		//***********************************************************************
		// Private function that supports the division of two numbers with
		// a divisor that has more than 1 digit.
		//
		// Algorithm taken from [1]
		//***********************************************************************

		private static void multiByteDivide(BigInteger operand, BigInteger secondOperand,
											BigInteger outQuotient, BigInteger outRemainder)
		{
			unchecked
			{
				uint[] result = new uint[maxLength];

				int remainderLen = operand.dataLength + 1;
				uint[] remainder = new uint[remainderLen];

				uint mask = 0x80000000;
				uint val = secondOperand.data[secondOperand.dataLength - 1];
				int shift = 0, resultPos = 0;

				while (mask != 0 && (val & mask) == 0)
				{
					shift++;
					mask >>= 1;
				}

				//Console.WriteLine("shift = {0}", shift);
				//Console.WriteLine("Before operand Len = {0}, secondOperand Len = {1}", operand.dataLength, secondOperand.dataLength);

				for (int i = 0; i < operand.dataLength; i++)
					remainder[i] = operand.data[i];
				shiftLeft(remainder, shift);
				secondOperand = secondOperand << shift;

				/*
						Console.WriteLine("operand Len = {0}, secondOperand Len = {1}", operand.dataLength, secondOperand.dataLength);
						Console.WriteLine("dividend = " + operand + "\ndivisor = " + secondOperand);
						for(int q = remainderLen - 1; q >= 0; q--)
								Console.Write("{0:x2}", remainder[q]);
						Console.WriteLine();
						*/

				int j = remainderLen - secondOperand.dataLength;
				int pos = remainderLen - 1;

				ulong firstDivisorByte = secondOperand.data[secondOperand.dataLength - 1];
				ulong secondDivisorByte = secondOperand.data[secondOperand.dataLength - 2];

				int divisorLen = secondOperand.dataLength + 1;
				uint[] dividendPart = new uint[divisorLen];

				while (j > 0)
				{
					ulong dividend = ((ulong)remainder[pos] << 32) + remainder[pos - 1];
					//Console.WriteLine("dividend = {0}", dividend);

					ulong q_hat = dividend / firstDivisorByte;
					ulong r_hat = dividend % firstDivisorByte;

					//Console.WriteLine("q_hat = {0:X}, r_hat = {1:X}", q_hat, r_hat);

					bool done = false;
					while (!done)
					{
						done = true;

						if (q_hat == 0x100000000 ||
							(q_hat * secondDivisorByte) > ((r_hat << 32) + remainder[pos - 2]))
						{
							q_hat--;
							r_hat += firstDivisorByte;

							if (r_hat < 0x100000000)
								done = false;
						}
					}

					for (int h = 0; h < divisorLen; h++)
						dividendPart[h] = remainder[pos - h];

					BigInteger kk = new BigInteger(dividendPart);
					BigInteger ss = secondOperand * (long)q_hat;

					//Console.WriteLine("ss before = " + ss);
					while (ss > kk)
					{
						q_hat--;
						ss -= secondOperand;
						//Console.WriteLine(ss);
					}
					BigInteger yy = kk - ss;

					//Console.WriteLine("ss = " + ss);
					//Console.WriteLine("kk = " + kk);
					//Console.WriteLine("yy = " + yy);

					for (int h = 0; h < divisorLen; h++)
						remainder[pos - h] = yy.data[secondOperand.dataLength - h];

					/*
								Console.WriteLine("dividend = ");
								for(int q = remainderLen - 1; q >= 0; q--)
										Console.Write("{0:x2}", remainder[q]);
								Console.WriteLine("\n************ q_hat = {0:X}\n", q_hat);
								*/

					result[resultPos++] = (uint)q_hat;

					pos--;
					j--;
				}

				outQuotient.dataLength = resultPos;
				int y = 0;
				for (int x = outQuotient.dataLength - 1; x >= 0; x--, y++)
					outQuotient.data[y] = result[x];
				for (; y < maxLength; y++)
					outQuotient.data[y] = 0;

				while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0)
					outQuotient.dataLength--;

				if (outQuotient.dataLength == 0)
					outQuotient.dataLength = 1;

				outRemainder.dataLength = shiftRight(remainder, shift);

				for (y = 0; y < outRemainder.dataLength; y++)
					outRemainder.data[y] = remainder[y];
				for (; y < maxLength; y++)
					outRemainder.data[y] = 0;
			}
		}

		//***********************************************************************
		// Private function that supports the division of two numbers with
		// a divisor that has only 1 digit.
		//***********************************************************************

		private static void singleByteDivide(BigInteger operand, BigInteger secondOperand,
											 BigInteger outQuotient, BigInteger outRemainder)
		{
			unchecked
			{
				uint[] result = new uint[maxLength];
				int resultPos = 0;

				// copy dividend to reminder
				for (int i = 0; i < maxLength; i++)
					outRemainder.data[i] = operand.data[i];
				outRemainder.dataLength = operand.dataLength;

				while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0)
					outRemainder.dataLength--;

				ulong divisor = secondOperand.data[0];
				int pos = outRemainder.dataLength - 1;
				ulong dividend = outRemainder.data[pos];

				//Console.WriteLine("divisor = " + divisor + " dividend = " + dividend);
				//Console.WriteLine("divisor = " + secondOperand + "\ndividend = " + operand);

				if (dividend >= divisor)
				{
					ulong quotient = dividend / divisor;
					result[resultPos++] = (uint)quotient;

					outRemainder.data[pos] = (uint)(dividend % divisor);
				}
				pos--;

				while (pos >= 0)
				{
					//Console.WriteLine(pos);

					dividend = ((ulong)outRemainder.data[pos + 1] << 32) + outRemainder.data[pos];
					ulong quotient = dividend / divisor;
					result[resultPos++] = (uint)quotient;

					outRemainder.data[pos + 1] = 0;
					outRemainder.data[pos--] = (uint)(dividend % divisor);
					//Console.WriteLine(">>>> " + operand);
				}

				outQuotient.dataLength = resultPos;
				int j = 0;
				for (int i = outQuotient.dataLength - 1; i >= 0; i--, j++)
					outQuotient.data[j] = result[i];
				for (; j < maxLength; j++)
					outQuotient.data[j] = 0;

				while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0)
					outQuotient.dataLength--;

				if (outQuotient.dataLength == 0)
					outQuotient.dataLength = 1;

				while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0)
					outRemainder.dataLength--;
			}
		}

		//***********************************************************************
		// Overloading of division operator
		//***********************************************************************

		/// <summary>
		/// Divides this by the specified operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public BigInteger Divide(BigInteger operand)
		{
			return this/operand;
		}

		/// <summary>
		/// Implements the operator /.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator /(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				BigInteger quotient = new BigInteger();
				BigInteger remainder = new BigInteger();

				int lastPos = maxLength - 1;
				bool divisorNeg = false, dividendNeg = false;

				if ((operand.data[lastPos] & 0x80000000) != 0) // operand negative
				{
					operand = -operand;
					dividendNeg = true;
				}
				if ((secondOperand.data[lastPos] & 0x80000000) != 0) // secondOperand negative
				{
					secondOperand = -secondOperand;
					divisorNeg = true;
				}

				if (operand < secondOperand)
					return quotient;

				else
				{
					if (secondOperand.dataLength == 1)
						singleByteDivide(operand, secondOperand, quotient, remainder);
					else
						multiByteDivide(operand, secondOperand, quotient, remainder);

					if (dividendNeg != divisorNeg)
						return -quotient;

					return quotient;
				}
			}
		}

		//***********************************************************************
		// Overloading of modulus operator
		//***********************************************************************

		/// <summary>
		/// Returns the modulus of the operands.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		[Obsolete("Use the Mod method instead.")]
		public static BigInteger Modulus(BigInteger operand, BigInteger secondOperand)
		{
			return operand%secondOperand;
		}

		/// <summary>
		/// Returns the modulus of the operands.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger Mod(BigInteger operand, BigInteger secondOperand)
		{
			return operand % secondOperand;
		}

		/// <summary>
		/// Implements the operator %.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator %(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				BigInteger quotient = new BigInteger();
				BigInteger remainder = new BigInteger(operand);

				int lastPos = maxLength - 1;
				bool dividendNeg = false;

				if ((operand.data[lastPos] & 0x80000000) != 0) // operand negative
				{
					operand = -operand;
					dividendNeg = true;
				}
				if ((secondOperand.data[lastPos] & 0x80000000) != 0) // secondOperand negative
					secondOperand = -secondOperand;

				if (operand < secondOperand)
					return remainder;

				else
				{
					if (secondOperand.dataLength == 1)
						singleByteDivide(operand, secondOperand, quotient, remainder);
					else
						multiByteDivide(operand, secondOperand, quotient, remainder);

					if (dividendNeg)
						return -remainder;

					return remainder;
				}
			}
		}

		//***********************************************************************
		// Overloading of bitwise AND operator
		//***********************************************************************

		/// <summary>
		/// Returns the bitwise and of this and the operator.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public BigInteger BitwiseAnd(BigInteger operand)
		{
			return this & operand;
		}

		/// <summary>
		/// Implements the operator &amp;.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator &(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				BigInteger result = new BigInteger();

				int len = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;

				for (int i = 0; i < len; i++)
				{
					uint sum = operand.data[i] & secondOperand.data[i];
					result.data[i] = sum;
				}

				result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				return result;
			}
		}

		//***********************************************************************
		// Overloading of bitwise OR operator
		//***********************************************************************

		/// <summary>
		/// ORs this and the specified operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public BigInteger BitwiseOr(BigInteger operand)
		{
			return this | operand;
		}

		/// <summary>
		/// Implements the operator |.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator |(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				BigInteger result = new BigInteger();

				int len = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;

				for (int i = 0; i < len; i++)
				{
					uint sum = operand.data[i] | secondOperand.data[i];
					result.data[i] = sum;
				}

				result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				return result;
			}
		}

		//***********************************************************************
		// Overloading of bitwise XOR operator
		//***********************************************************************

		/// <summary>
		/// Implements the operator Xor.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger Xor(BigInteger operand, BigInteger secondOperand)
		{
			return operand ^ secondOperand;
		}

		/// <summary>
		/// Implements the operator ^.
		/// </summary>
		/// <param name="operand">The first operand.</param>
		/// <param name="secondOperand">The second operand.</param>
		/// <returns>The result of the operator.</returns>
		public static BigInteger operator ^(BigInteger operand, BigInteger secondOperand)
		{
			unchecked
			{
				if ((object)operand == null)
					throw new ArgumentNullException("operand");
				if ((object)secondOperand == null)
					throw new ArgumentNullException("secondOperand");

				BigInteger result = new BigInteger();

				int len = (operand.dataLength > secondOperand.dataLength) ? operand.dataLength : secondOperand.dataLength;

				for (int i = 0; i < len; i++)
				{
					uint sum = operand.data[i] ^ secondOperand.data[i];
					result.data[i] = sum;
				}

				result.dataLength = maxLength;

				while (result.dataLength > 1 && result.data[result.dataLength - 1] == 0)
					result.dataLength--;

				return result;
			}
		}

		//***********************************************************************
		// Returns max(this, bi)
		//***********************************************************************

		/// <summary>
		/// Returns max(this, bi)
		/// </summary>
		/// <param name="bi">The BigInteger to compare to.</param>
		/// <returns></returns>
		public BigInteger Max(BigInteger bi)
		{
			if (this > bi)
				return (new BigInteger(this));
			else
				return (new BigInteger(bi));
		}

		//***********************************************************************
		// Returns min(this, bi)
		//***********************************************************************

		/// <summary>
		/// Returns min(this, bi)
		/// </summary>
		/// <param name="bi">The BigInteger to compare to.</param>
		/// <returns></returns>
		public BigInteger Min(BigInteger bi)
		{
			if (this < bi)
				return (new BigInteger(this));
			else
				return (new BigInteger(bi));
		}

		//***********************************************************************
		// Returns the absolute value
		//***********************************************************************

		/// <summary>
		/// Returns the absolute value.
		/// </summary>
		/// <returns></returns>
		public BigInteger Abs()
		{
			unchecked
			{
				if ((data[maxLength - 1] & 0x80000000) != 0)
					return (-this);
				else
					return (new BigInteger(this));
			}
		}

		//***********************************************************************
		// Returns a string representing the BigInteger in base 10.
		//***********************************************************************

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:Fullsix.Security.Cryptography.BigInteger"></see> in base 10.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:Fullsix.Security.Cryptography.BigInteger"></see>.
		/// </returns>
		public override string ToString()
		{
			return ToString(10);
		}

		//***********************************************************************
		// Returns a string representing the BigInteger in sign-and-magnitude
		// format in the specified radix.
		//
		// Example
		// -------
		// If the value of BigInteger is -255 in base 10, then
		// ToString(16) returns "-FF"
		//
		//***********************************************************************

		/// <summary>
		/// Returns a string representing the BigInteger in sign-and-magnitude
		/// format in the specified radix.
		/// </summary>
		/// <param name="radix">The radix.</param>
		/// <returns></returns>
		public string ToString(int radix)
		{
			unchecked
			{
				if (radix < 2 || radix > 36)
					throw (new ArgumentException("Radix must be >= 2 and <= 36"));

				string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				StringBuilder result = new StringBuilder();

				BigInteger a = this;

				bool negative = false;
				if ((a.data[maxLength - 1] & 0x80000000) != 0)
				{
					negative = true;
					try
					{
						a = -a;
					}
					catch (ApplicationException)
					{
					}
				}

				BigInteger quotient = new BigInteger();
				BigInteger remainder = new BigInteger();
				BigInteger biRadix = new BigInteger(radix);

				if (a.dataLength == 1 && a.data[0] == 0)
					return "0";
				else
				{
					while (a.dataLength > 1 || (a.dataLength == 1 && a.data[0] != 0))
					{
						singleByteDivide(a, biRadix, quotient, remainder);

						if (remainder.data[0] < 10)
							result.Insert(0, remainder.data[0]);
						else
							result.Insert(0, charSet[(int)remainder.data[0] - 10]);

						a = quotient;
					}
					if (negative)
						result.Insert(0, "-");
				}

				return result.ToString();
			}
		}

		//***********************************************************************
		// Returns a hex string showing the contains of the BigInteger
		//
		// Examples
		// -------
		// 1) If the value of BigInteger is 255 in base 10, then
		//    ToHexString() returns "FF"
		//
		// 2) If the value of BigInteger is -255 in base 10, then
		//    ToHexString() returns ".....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF01",
		//    which is the 2's complement representation of -255.
		//
		//***********************************************************************

		/// <summary>
		/// Returns a hex string showing the contains of the BigInteger.
		/// </summary>
		/// <returns></returns>
		public string ToHexString()
		{
			unchecked
			{
				string result = data[dataLength - 1].ToString("X", CultureInfo.InvariantCulture);
				StringBuilder sb = new StringBuilder(result);
				for (int i = dataLength - 2; i >= 0; i--)
					sb.Append(data[i].ToString("X8", CultureInfo.InvariantCulture));

				return sb.ToString();
			}
		}

		//***********************************************************************
		// Modulo Exponentiation
		//***********************************************************************

		/// <summary>
		/// Modulo Exponentiation
		/// </summary>
		/// <param name="exponent">The exponent.</param>
		/// <param name="modulus">The modulus.</param>
		/// <returns></returns>
		public BigInteger ModPow(BigInteger exponent, BigInteger modulus)
		{
			unchecked
			{
				if (exponent == null)
					throw new ArgumentNullException("exponent");
				if (modulus == null)
					throw new ArgumentNullException("modulus");
				if ((exponent.data[maxLength - 1] & 0x80000000) != 0)
					throw (new ArithmeticException("Positive exponents only."));

				BigInteger resultNum = 1;
				BigInteger tempNum;
				bool thisNegative = false;

				if ((data[maxLength - 1] & 0x80000000) != 0) // negative this
				{
					tempNum = -this % modulus;
					thisNegative = true;
				}
				else
					tempNum = this % modulus; // ensures (tempNum * tempNum) < b^(2k)

				if ((modulus.data[maxLength - 1] & 0x80000000) != 0) // negative n
					modulus = -modulus;

				// calculate constant = b^(2k) / m
				BigInteger constant = new BigInteger();

				int i = modulus.dataLength << 1;
				constant.data[i] = 0x00000001;
				constant.dataLength = i + 1;

				constant = constant / modulus;
				int totalBits = exponent.BitCount();
				int count = 0;

				// perform squaring and multiply exponentiation
				for (int pos = 0; pos < exponent.dataLength; pos++)
				{
					uint mask = 0x01;
					//Console.WriteLine("pos = " + pos);

					for (int index = 0; index < 32; index++)
					{
						if ((exponent.data[pos] & mask) != 0)
							resultNum = BarrettReduction(resultNum * tempNum, modulus, constant);

						mask <<= 1;

						tempNum = BarrettReduction(tempNum * tempNum, modulus, constant);

						if (tempNum.dataLength == 1 && tempNum.data[0] == 1)
						{
							if (thisNegative && (exponent.data[0] & 0x1) != 0) //odd exp
								return -resultNum;
							return resultNum;
						}
						count++;
						if (count == totalBits)
							break;
					}
				}

				if (thisNegative && (exponent.data[0] & 0x1) != 0) //odd exp
					return -resultNum;

				return resultNum;
			}
		}

		//***********************************************************************
		// Fast calculation of modular reduction using Barrett's reduction.
		// Requires x < b^(2k), where b is the base.  In this case, base is
		// 2^32 (uint).
		//
		// Reference [4]
		//***********************************************************************

		private static BigInteger BarrettReduction(BigInteger x, BigInteger n, BigInteger constant)
		{
			unchecked
			{
				int k = n.dataLength,
					kPlusOne = k + 1,
					kMinusOne = k - 1;

				BigInteger q1 = new BigInteger();

				// q1 = x / b^(k-1)
				for (int i = kMinusOne, j = 0; i < x.dataLength; i++, j++)
					q1.data[j] = x.data[i];
				q1.dataLength = x.dataLength - kMinusOne;
				if (q1.dataLength <= 0)
					q1.dataLength = 1;

				BigInteger q2 = q1 * constant;
				BigInteger q3 = new BigInteger();

				// q3 = q2 / b^(k+1)
				for (int i = kPlusOne, j = 0; i < q2.dataLength; i++, j++)
					q3.data[j] = q2.data[i];
				q3.dataLength = q2.dataLength - kPlusOne;
				if (q3.dataLength <= 0)
					q3.dataLength = 1;

				// r1 = x mod b^(k+1)
				// i.e. keep the lowest (k+1) words
				BigInteger r1 = new BigInteger();
				int lengthToCopy = (x.dataLength > kPlusOne) ? kPlusOne : x.dataLength;
				for (int i = 0; i < lengthToCopy; i++)
					r1.data[i] = x.data[i];
				r1.dataLength = lengthToCopy;

				// r2 = (q3 * n) mod b^(k+1)
				// partial multiplication of q3 and n

				BigInteger r2 = new BigInteger();
				for (int i = 0; i < q3.dataLength; i++)
				{
					if (q3.data[i] == 0)
						continue;

					ulong mcarry = 0;
					int t = i;
					for (int j = 0; j < n.dataLength && t < kPlusOne; j++, t++)
					{
						// t = i + j
						ulong val = (q3.data[i] * (ulong)n.data[j]) + r2.data[t] + mcarry;

						r2.data[t] = (uint)(val & 0xFFFFFFFF);
						mcarry = (val >> 32);
					}

					if (t < kPlusOne)
						r2.data[t] = (uint)mcarry;
				}
				r2.dataLength = kPlusOne;
				while (r2.dataLength > 1 && r2.data[r2.dataLength - 1] == 0)
					r2.dataLength--;

				r1 -= r2;
				if ((r1.data[maxLength - 1] & 0x80000000) != 0) // negative
				{
					BigInteger val = new BigInteger();
					val.data[kPlusOne] = 0x00000001;
					val.dataLength = kPlusOne + 1;
					r1 += val;
				}

				while (r1 >= n)
					r1 -= n;

				return r1;
			}
		}

		//***********************************************************************
		// Returns gcd(this, bi)
		//***********************************************************************

		/// <summary>
		/// Returns the greatest common denominator of (this, bi)
		/// </summary>
		/// <param name="bi">bi.</param>
		/// <returns></returns>
		public BigInteger Gcd(BigInteger bi)
		{
			unchecked
			{
				if ((object)bi == null)
					throw new ArgumentNullException("bi");

				BigInteger x;
				BigInteger y;

				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					x = -this;
				else
					x = this;

				if ((bi.data[maxLength - 1] & 0x80000000) != 0) // negative
					y = -bi;
				else
					y = bi;

				BigInteger g = y;

				while (x.dataLength > 1 || (x.dataLength == 1 && x.data[0] != 0))
				{
					g = x;
					x = y % x;
					y = g;
				}

				return g;
			}
		}

		//***********************************************************************
		// Populates "this" with the specified amount of random bits
		//***********************************************************************

		/// <summary>
		/// Populates "this" with the specified amount of random bits
		/// </summary>
		/// <param name="bits">The number of bits.</param>
		/// <param name="rand">The random generator.</param>
		public void GenerateRandomBits(int bits, Random rand)
		{
			unchecked
			{
				if (rand == null)
					throw new ArgumentNullException("rand");
				int dwords = bits >> 5;
				int remBits = bits & 0x1F;

				if (remBits != 0)
					dwords++;

				if (dwords > maxLength)
					throw (new ArithmeticException("Number of required bits > maxLength."));

				for (int i = 0; i < dwords; i++)
					data[i] = (uint)(rand.NextDouble() * 0x100000000);

				for (int i = dwords; i < maxLength; i++)
					data[i] = 0;

				if (remBits != 0)
				{
					uint mask = (uint)(0x01 << (remBits - 1));
					data[dwords - 1] |= mask;

					mask = 0xFFFFFFFF >> (32 - remBits);
					data[dwords - 1] &= mask;
				}
				else
					data[dwords - 1] |= 0x80000000;

				dataLength = dwords;

				if (dataLength == 0)
					dataLength = 1;
			}
		}

		//***********************************************************************
		// Returns the position of the most significant bit in the BigInteger.
		//
		// Eg.  The result is 0, if the value of BigInteger is 0...0000 0000
		//      The result is 1, if the value of BigInteger is 0...0000 0001
		//      The result is 2, if the value of BigInteger is 0...0000 0010
		//      The result is 2, if the value of BigInteger is 0...0000 0011
		//
		//***********************************************************************

		/// <summary>
		/// Returns the position of the most significant bit in the BigInteger.
		/// </summary>
		/// <returns></returns>
		public int BitCount()
		{
			unchecked
			{
				while (dataLength > 1 && data[dataLength - 1] == 0)
					dataLength--;

				uint value = data[dataLength - 1];
				uint mask = 0x80000000;
				int bits = 32;

				while (bits > 0 && (value & mask) == 0)
				{
					bits--;
					mask >>= 1;
				}
				bits += ((dataLength - 1) << 5);

				return bits;
			}
		}

		//***********************************************************************
		// Probabilistic prime test based on Fermat's little theorem
		//
		// for any a < p (p does not divide a) if
		//      a^(p-1) mod p != 1 then p is not prime.
		//
		// Otherwise, p is probably prime (pseudoprime to the chosen base).
		//
		// Returns
		// -------
		// True if "this" is a pseudoprime to randomly chosen
		// bases.  The number of chosen bases is given by the "confidence"
		// parameter.
		//
		// False if "this" is definitely NOT prime.
		//
		// Note - this method is fast but fails for Carmichael numbers except
		// when the randomly chosen base is a factor of the number.
		//
		//***********************************************************************

		/// <summary>
		/// Probabilistic prime test based on Fermat's little theorem
		///
		/// for any a &lt; p (p does not divide a) if
		///      a^(p-1) mod p != 1 then p is not prime.
		///
		/// Otherwise, p is probably prime (pseudoprime to the chosen base).
		/// </summary>
		/// <param name="confidence">The confidence.</param>
		/// <returns>
		/// True if "this" is a pseudoprime to randomly chosen
		/// bases.  The number of chosen bases is given by the "confidence"
		/// parameter.
		///
		/// False if "this" is definitely NOT prime.
		/// </returns>
		public bool FermatLittleTest(int confidence)
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				if (thisVal.dataLength == 1)
				{
					// test small numbers
					if (thisVal.data[0] == 0 || thisVal.data[0] == 1)
						return false;
					else if (thisVal.data[0] == 2 || thisVal.data[0] == 3)
						return true;
				}

				if ((thisVal.data[0] & 0x1) == 0) // even numbers
					return false;

				int bits = thisVal.BitCount();
				BigInteger a = new BigInteger();
				BigInteger p_sub1 = thisVal - (new BigInteger(1));
				Random rand = new Random();

				for (int round = 0; round < confidence; round++)
				{
					bool done = false;

					while (!done) // generate a < n
					{
						int testBits = 0;

						// make sure "a" has at least 2 bits
						while (testBits < 2)
							testBits = (int)(rand.NextDouble() * bits);

						a.GenerateRandomBits(testBits, rand);

						int byteLen = a.dataLength;

						// make sure "a" is not 0
						if (byteLen > 1 || (byteLen == 1 && a.data[0] != 1))
							done = true;
					}

					// check whether a factor exists (fix for version 1.03)
					BigInteger gcdTest = a.Gcd(thisVal);
					if (gcdTest.dataLength == 1 && gcdTest.data[0] != 1)
						return false;

					// calculate a^(p-1) mod p
					BigInteger expResult = a.ModPow(p_sub1, thisVal);

					int resultLen = expResult.dataLength;

					// is NOT prime is a^(p-1) mod p != 1

					if (resultLen > 1 || (resultLen == 1 && expResult.data[0] != 1))
					{
						//Console.WriteLine("a = " + a.ToString());
						return false;
					}
				}

				return true;
			}
		}

		//***********************************************************************
		// Probabilistic prime test based on Rabin-Miller's
		//
		// for any p > 0 with p - 1 = 2^s * t
		//
		// p is probably prime (strong pseudoprime) if for any a < p,
		// 1) a^t mod p = 1 or
		// 2) a^((2^j)*t) mod p = p-1 for some 0 <= j <= s-1
		//
		// Otherwise, p is composite.
		//
		// Returns
		// -------
		// True if "this" is a strong pseudoprime to randomly chosen
		// bases.  The number of chosen bases is given by the "confidence"
		// parameter.
		//
		// False if "this" is definitely NOT prime.
		//
		//***********************************************************************

		/// <summary>
		/// Probabilistic prime test based on Rabin-Miller's
		///
		/// for any p &gt; 0 with p - 1 = 2^s * t
		///
		/// p is probably prime (strong pseudoprime) if for any a &lt; p,
		/// 1) a^t mod p = 1 or
		/// 2) a^((2^j)*t) mod p = p-1 for some 0 &lt;= j &lt;= s-1
		///
		/// Otherwise, p is composite.
		/// </summary>
		/// <param name="confidence">The confidence.</param>
		/// <returns>
		/// True if "this" is a strong pseudoprime to randomly chosen
		/// bases.  The number of chosen bases is given by the "confidence"
		/// parameter.
		///
		/// False if "this" is definitely NOT prime.
		/// </returns>
		public bool RabinMillerTest(int confidence)
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				if (thisVal.dataLength == 1)
				{
					// test small numbers
					if (thisVal.data[0] == 0 || thisVal.data[0] == 1)
						return false;
					else if (thisVal.data[0] == 2 || thisVal.data[0] == 3)
						return true;
				}

				if ((thisVal.data[0] & 0x1) == 0) // even numbers
					return false;

				// calculate values of s and t
				BigInteger p_sub1 = thisVal - (new BigInteger(1));
				int s = 0;

				for (int index = 0; index < p_sub1.dataLength; index++)
				{
					uint mask = 0x01;

					for (int i = 0; i < 32; i++)
					{
						if ((p_sub1.data[index] & mask) != 0)
						{
							index = p_sub1.dataLength; // to break the outer loop
							break;
						}
						mask <<= 1;
						s++;
					}
				}

				BigInteger t = p_sub1 >> s;

				int bits = thisVal.BitCount();
				BigInteger a = new BigInteger();
				Random rand = new Random();

				for (int round = 0; round < confidence; round++)
				{
					bool done = false;

					while (!done) // generate a < n
					{
						int testBits = 0;

						// make sure "a" has at least 2 bits
						while (testBits < 2)
							testBits = (int)(rand.NextDouble() * bits);

						a.GenerateRandomBits(testBits, rand);

						int byteLen = a.dataLength;

						// make sure "a" is not 0
						if (byteLen > 1 || (byteLen == 1 && a.data[0] != 1))
							done = true;
					}

					// check whether a factor exists (fix for version 1.03)
					BigInteger gcdTest = a.Gcd(thisVal);
					if (gcdTest.dataLength == 1 && gcdTest.data[0] != 1)
						return false;

					BigInteger b = a.ModPow(t, thisVal);

					/*
								Console.WriteLine("a = " + a.ToString(10));
								Console.WriteLine("b = " + b.ToString(10));
								Console.WriteLine("t = " + t.ToString(10));
								Console.WriteLine("s = " + s);
								*/

					bool result = false;

					if (b.dataLength == 1 && b.data[0] == 1) // a^t mod p = 1
						result = true;

					for (int j = 0; result == false && j < s; j++)
					{
						if (b == p_sub1) // a^((2^j)*t) mod p = p-1 for some 0 <= j <= s-1
						{
							result = true;
							break;
						}

						b = (b * b) % thisVal;
					}

					if (result == false)
						return false;
				}
				return true;
			}
		}

		//***********************************************************************
		// Probabilistic prime test based on Solovay-Strassen (Euler Criterion)
		//
		// p is probably prime if for any a < p (a is not multiple of p),
		// a^((p-1)/2) mod p = J(a, p)
		//
		// where J is the Jacobi symbol.
		//
		// Otherwise, p is composite.
		//
		// Returns
		// -------
		// True if "this" is a Euler pseudoprime to randomly chosen
		// bases.  The number of chosen bases is given by the "confidence"
		// parameter.
		//
		// False if "this" is definitely NOT prime.
		//
		//***********************************************************************

		/// <summary>
		/// Probabilistic prime test based on Solovay-Strassen (Euler Criterion)
		///
		/// p is probably prime if for any a &lt; p (a is not multiple of p),
		/// a^((p-1)/2) mod p = J(a, p)
		///
		/// where J is the Jacobi symbol.
		///
		/// Otherwise, p is composite.
		/// </summary>
		/// <param name="confidence">The confidence.</param>
		/// <returns>
		/// True if "this" is a Euler pseudoprime to randomly chosen
		/// bases.  The number of chosen bases is given by the "confidence"
		/// parameter.
		///
		/// False if "this" is definitely NOT prime.
		/// </returns>
		public bool SolovayStrassenTest(int confidence)
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				if (thisVal.dataLength == 1)
				{
					// test small numbers
					if (thisVal.data[0] == 0 || thisVal.data[0] == 1)
						return false;
					else if (thisVal.data[0] == 2 || thisVal.data[0] == 3)
						return true;
				}

				if ((thisVal.data[0] & 0x1) == 0) // even numbers
					return false;

				int bits = thisVal.BitCount();
				BigInteger a = new BigInteger();
				BigInteger p_sub1 = thisVal - 1;
				BigInteger p_sub1_shift = p_sub1 >> 1;

				Random rand = new Random();

				for (int round = 0; round < confidence; round++)
				{
					bool done = false;

					while (!done) // generate a < n
					{
						int testBits = 0;

						// make sure "a" has at least 2 bits
						while (testBits < 2)
							testBits = (int)(rand.NextDouble() * bits);

						a.GenerateRandomBits(testBits, rand);

						int byteLen = a.dataLength;

						// make sure "a" is not 0
						if (byteLen > 1 || (byteLen == 1 && a.data[0] != 1))
							done = true;
					}

					// check whether a factor exists (fix for version 1.03)
					BigInteger gcdTest = a.Gcd(thisVal);
					if (gcdTest.dataLength == 1 && gcdTest.data[0] != 1)
						return false;

					// calculate a^((p-1)/2) mod p

					BigInteger expResult = a.ModPow(p_sub1_shift, thisVal);
					if (expResult == p_sub1)
						expResult = -1;

					// calculate Jacobi symbol
					BigInteger jacob = Jacobi(a, thisVal);

					//Console.WriteLine("a = " + a.ToString(10) + " b = " + thisVal.ToString(10));
					//Console.WriteLine("expResult = " + expResult.ToString(10) + " Jacob = " + jacob.ToString(10));

					// if they are different then it is not prime
					if (expResult != jacob)
						return false;
				}

				return true;
			}
		}

		//***********************************************************************
		// Implementation of the Lucas Strong Pseudo Prime test.
		//
		// Let n be an odd number with gcd(n,D) = 1, and n - J(D, n) = 2^s * d
		// with d odd and s >= 0.
		//
		// If Ud mod n = 0 or V2^r*d mod n = 0 for some 0 <= r < s, then n
		// is a strong Lucas pseudoprime with parameters (P, Q).  We select
		// P and Q based on Selfridge.
		//
		// Returns True if number is a strong Lucus pseudo prime.
		// Otherwise, returns False indicating that number is composite.
		//***********************************************************************

		/// <summary>
		/// Implementation of the Lucas Strong Pseudo Prime test.
		/// </summary>
		/// <returns>
		/// Returns True if number is a strong Lucus pseudo prime.
		/// Otherwise, returns False indicating that number is composite.
		/// </returns>
		public bool LucasStrongTest()
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				if (thisVal.dataLength == 1)
				{
					// test small numbers
					if (thisVal.data[0] == 0 || thisVal.data[0] == 1)
						return false;
					else if (thisVal.data[0] == 2 || thisVal.data[0] == 3)
						return true;
				}

				if ((thisVal.data[0] & 0x1) == 0) // even numbers
					return false;

				return LucasStrongTestHelper(thisVal);
			}
		}

		private static bool LucasStrongTestHelper(BigInteger thisVal)
		{
			unchecked
			{
				// Do the test (selects D based on Selfridge)
				// Let D be the first element of the sequence
				// 5, -7, 9, -11, 13, ... for which J(D,n) = -1
				// Let P = 1, Q = (1-D) / 4

				long D = 5, sign = -1, dCount = 0;
				bool done = false;

				while (!done)
				{
					int Jresult = Jacobi(D, thisVal);

					if (Jresult == -1)
						done = true; // J(D, this) = 1
					else
					{
						if (Jresult == 0 && Math.Abs(D) < thisVal) // divisor found
							return false;

						if (dCount == 20)
						{
							// check for square
							BigInteger root = thisVal.Sqrt();
							if (root * root == thisVal)
								return false;
						}

						//Console.WriteLine(D);
						D = (Math.Abs(D) + 2) * sign;
						sign = -sign;
					}
					dCount++;
				}

				long Q = (1 - D) >> 2;

				/*
						Console.WriteLine("D = " + D);
						Console.WriteLine("Q = " + Q);
						Console.WriteLine("(n,D) = " + thisVal.gcd(D));
						Console.WriteLine("(n,Q) = " + thisVal.gcd(Q));
						Console.WriteLine("J(D|n) = " + BigInteger.Jacobi(D, thisVal));
						*/

				BigInteger p_add1 = thisVal + 1;
				int s = 0;

				for (int index = 0; index < p_add1.dataLength; index++)
				{
					uint mask = 0x01;

					for (int i = 0; i < 32; i++)
					{
						if ((p_add1.data[index] & mask) != 0)
						{
							index = p_add1.dataLength; // to break the outer loop
							break;
						}
						mask <<= 1;
						s++;
					}
				}

				BigInteger t = p_add1 >> s;

				// calculate constant = b^(2k) / m
				// for Barrett Reduction
				BigInteger constant = new BigInteger();

				int nLen = thisVal.dataLength << 1;
				constant.data[nLen] = 0x00000001;
				constant.dataLength = nLen + 1;

				constant = constant / thisVal;

				BigInteger[] lucas = LucasSequenceHelper(1, Q, t, thisVal, constant, 0);
				bool isPrime = false;

				if ((lucas[0].dataLength == 1 && lucas[0].data[0] == 0) ||
					(lucas[1].dataLength == 1 && lucas[1].data[0] == 0))
				{
					// u(t) = 0 or V(t) = 0
					isPrime = true;
				}

				for (int i = 1; i < s; i++)
				{
					if (!isPrime)
					{
						// doubling of index
						lucas[1] = BarrettReduction(lucas[1] * lucas[1], thisVal, constant);
						lucas[1] = (lucas[1] - (lucas[2] << 1)) % thisVal;

						//lucas[1] = ((lucas[1] * lucas[1]) - (lucas[2] << 1)) % thisVal;

						if ((lucas[1].dataLength == 1 && lucas[1].data[0] == 0))
							isPrime = true;
					}

					lucas[2] = BarrettReduction(lucas[2] * lucas[2], thisVal, constant); //Q^k
				}

				if (isPrime) // additional checks for composite numbers
				{
					// If n is prime and gcd(n, Q) == 1, then
					// Q^((n+1)/2) = Q * Q^((n-1)/2) is congruent to (Q * J(Q, n)) mod n

					BigInteger g = thisVal.Gcd(Q);
					if (g.dataLength == 1 && g.data[0] == 1) // gcd(this, Q) == 1
					{
						if ((lucas[2].data[maxLength - 1] & 0x80000000) != 0)
							lucas[2] += thisVal;

						BigInteger temp = (Q * Jacobi(Q, thisVal)) % thisVal;
						if ((temp.data[maxLength - 1] & 0x80000000) != 0)
							temp += thisVal;

						if (lucas[2] != temp)
							isPrime = false;
					}
				}

				return isPrime;
			}
		}

		//***********************************************************************
		// Determines whether a number is probably prime, using the Rabin-Miller's
		// test.  Before applying the test, the number is tested for divisibility
		// by primes < 2000
		//
		// Returns true if number is probably prime.
		//***********************************************************************

		/// <summary>
		/// Determines within the specified confidence if "this" is a probable prime.
		/// </summary>
		/// <param name="confidence">The confidence.</param>
		/// <returns>
		/// 	<c>true</c> if the specified confidence is a probable prime; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProbablePrime(int confidence)
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				// test for divisibility by primes < 2000
				for (int p = 0; p < primesBelow2000.Length; p++)
				{
					BigInteger divisor = primesBelow2000[p];

					if (divisor >= thisVal)
						break;

					BigInteger resultNum = thisVal % divisor;
					if (resultNum.Int32Value() == 0)
					{
						/*
						Console.WriteLine("Not prime!  Divisible by {0}\n",
														primesBelow2000[p]);
										*/
						return false;
					}
				}

				if (thisVal.RabinMillerTest(confidence))
					return true;
				else
				{
					//Console.WriteLine("Not prime!  Failed primality test\n");
					return false;
				}
			}
		}

		//***********************************************************************
		// Determines whether this BigInteger is probably prime using a
		// combination of base 2 strong pseudoprime test and Lucas strong
		// pseudoprime test.
		//
		// The sequence of the primality test is as follows,
		//
		// 1) Trial divisions are carried out using prime numbers below 2000.
		//    if any of the primes divides this BigInteger, then it is not prime.
		//
		// 2) Perform base 2 strong pseudoprime test.  If this BigInteger is a
		//    base 2 strong pseudoprime, proceed on to the next step.
		//
		// 3) Perform strong Lucas pseudoprime test.
		//
		// Returns True if this BigInteger is both a base 2 strong pseudoprime
		// and a strong Lucas pseudoprime.
		//
		// For a detailed discussion of this primality test, see [6].
		//
		//***********************************************************************

		/// <summary>
		/// Determines  if "this" is a probable prime.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the specified confidence is a probable prime; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProbablePrime()
		{
			unchecked
			{
				BigInteger thisVal;
				if ((data[maxLength - 1] & 0x80000000) != 0) // negative
					thisVal = -this;
				else
					thisVal = this;

				if (thisVal.dataLength == 1)
				{
					// test small numbers
					if (thisVal.data[0] == 0 || thisVal.data[0] == 1)
						return false;
					else if (thisVal.data[0] == 2 || thisVal.data[0] == 3)
						return true;
				}

				if ((thisVal.data[0] & 0x1) == 0) // even numbers
					return false;

				// test for divisibility by primes < 2000
				for (int p = 0; p < primesBelow2000.Length; p++)
				{
					BigInteger divisor = primesBelow2000[p];

					if (divisor >= thisVal)
						break;

					BigInteger resultNum = thisVal % divisor;
					if (resultNum.Int32Value() == 0)
					{
						//Console.WriteLine("Not prime!  Divisible by {0}\n",
						//                  primesBelow2000[p]);

						return false;
					}
				}

				// Perform BASE 2 Rabin-Miller Test

				// calculate values of s and t
				BigInteger p_sub1 = thisVal - (new BigInteger(1));
				int s = 0;

				for (int index = 0; index < p_sub1.dataLength; index++)
				{
					uint mask = 0x01;

					for (int i = 0; i < 32; i++)
					{
						if ((p_sub1.data[index] & mask) != 0)
						{
							index = p_sub1.dataLength; // to break the outer loop
							break;
						}
						mask <<= 1;
						s++;
					}
				}

				BigInteger t = p_sub1 >> s;

				BigInteger a = 2;

				// b = a^t mod p
				BigInteger b = a.ModPow(t, thisVal);
				bool result = false;

				if (b.dataLength == 1 && b.data[0] == 1) // a^t mod p = 1
					result = true;

				for (int j = 0; result == false && j < s; j++)
				{
					if (b == p_sub1) // a^((2^j)*t) mod p = p-1 for some 0 <= j <= s-1
					{
						result = true;
						break;
					}

					b = (b * b) % thisVal;
				}

				// if number is strong pseudoprime to base 2, then do a strong lucas test
				if (result)
					result = LucasStrongTestHelper(thisVal);

				return result;
			}
		}

		//***********************************************************************
		// Returns the lowest 4 bytes of the BigInteger as an int.
		//***********************************************************************

		/// <summary>
		/// Returns the lowest 4 bytes of the BigInteger as an int.
		/// </summary>
		/// <returns></returns>
		[Obsolete("Use the Int32Value method instead. This method is kept for backwards compatibility.")]
		public int IntValue()
		{
			return Int32Value();
		}

		/// <summary>
		/// Returns the lowest 4 bytes of the BigInteger as an <see cref="int"/>.
		/// </summary>
		public int Int32Value()
		{
			return (int) data[0];
		}

		//***********************************************************************
		// Returns the lowest 8 bytes of the BigInteger as a long.
		//***********************************************************************

		/// <summary>
		/// Returns the lowest 8 bytes of the BigInteger as a long.
		/// </summary>
		[Obsolete("Use the Int64Value method instead. This method is kept for backwards compatibility.")]
		public long LongValue()
		{
			return Int64Value();
		}

		/// <summary>
		/// Returns the lowest 8 bytes of the BigInteger as a long.
		/// </summary>
		public long Int64Value()
		{
			unchecked
			{
				long val = data[0];
				try
				{
					// exception if maxLength = 1
					val |= (long)data[1] << 32;
				}
				catch (ApplicationException)
				{
					if ((data[0] & 0x80000000) != 0) // negative
						val = (int)data[0];
				}

				return val;
			}
		}

		//***********************************************************************
		// Computes the Jacobi Symbol for a and b.
		// Algorithm adapted from [3] and [4] with some optimizations
		//***********************************************************************

		/// <summary>
		/// Computes the Jacobi Symbol for a and b.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">b.</param>
		/// <returns></returns>
		public static int Jacobi(BigInteger a, BigInteger b)
		{
			unchecked
			{
				if ((object)a == null)
					throw new ArgumentNullException("a");
				if ((object)b == null)
					throw new ArgumentNullException("b");
				// Jacobi defined only for odd integers
				if ((b.data[0] & 0x1) == 0)
					throw (new ArgumentException("Jacobi defined only for odd integers."));

				if (a >= b)
					a %= b;
				if (a.dataLength == 1 && a.data[0] == 0)
					return 0; // a == 0
				if (a.dataLength == 1 && a.data[0] == 1)
					return 1; // a == 1

				if (a < 0)
				{
					if ((((b - 1).data[0]) & 0x2) == 0) //if( (((b-1) >> 1).data[0] & 0x1) == 0)
						return Jacobi(-a, b);
					else
						return -Jacobi(-a, b);
				}

				int e = 0;
				for (int index = 0; index < a.dataLength; index++)
				{
					uint mask = 0x01;

					for (int i = 0; i < 32; i++)
					{
						if ((a.data[index] & mask) != 0)
						{
							index = a.dataLength; // to break the outer loop
							break;
						}
						mask <<= 1;
						e++;
					}
				}

				BigInteger a1 = a >> e;

				int s = 1;
				if ((e & 0x1) != 0 && ((b.data[0] & 0x7) == 3 || (b.data[0] & 0x7) == 5))
					s = -1;

				if ((b.data[0] & 0x3) == 3 && (a1.data[0] & 0x3) == 3)
					s = -s;

				if (a1.dataLength == 1 && a1.data[0] == 1)
					return s;
				else
					return (s * Jacobi(b % a1, a1));
			}
		}

		//***********************************************************************
		// Generates a positive BigInteger that is probably prime.
		//***********************************************************************

		/// <summary>
		/// Generates a positive BigInteger that is probably prime.
		/// </summary>
		/// <param name="bits">The bits.</param>
		/// <param name="confidence">The confidence.</param>
		/// <param name="rand">The random number generator.</param>
		/// <returns></returns>
		public static BigInteger GeneratePseudoPrime(int bits, int confidence, Random rand)
		{
			unchecked
			{
				BigInteger result = new BigInteger();
				bool done = false;

				while (!done)
				{
					result.GenerateRandomBits(bits, rand);
					result.data[0] |= 0x01; // make it odd

					// prime test
					done = result.IsProbablePrime(confidence);
				}
				return result;
			}
		}

		//***********************************************************************
		// Generates a random number with the specified number of bits such
		// that gcd(number, this) = 1
		//***********************************************************************

		/// <summary>
		/// Generates a random number with the specified number of bits such
		/// that gcd(number, this) = 1
		/// </summary>
		/// <param name="bits">The bits.</param>
		/// <param name="rand">The rand.</param>
		/// <returns></returns>
		public BigInteger GenerateCoprime(int bits, Random rand)
		{
			unchecked
			{
				bool done = false;
				BigInteger result = new BigInteger();

				while (!done)
				{
					result.GenerateRandomBits(bits, rand);
					//Console.WriteLine(result.ToString(16));

					// gcd test
					BigInteger g = result.Gcd(this);
					if (g.dataLength == 1 && g.data[0] == 1)
						done = true;
				}

				return result;
			}
		}

		//***********************************************************************
		// Returns the modulo inverse of this.  Throws ArithmeticException if
		// the inverse does not exist.  (i.e. gcd(this, modulus) != 1)
		//***********************************************************************

		/// <summary>
		/// Returns the modulo inverse of this.  Throws ArithmeticException if
		/// the inverse does not exist.  (i.e. gcd(this, modulus) != 1)
		/// </summary>
		/// <param name="modulus">The modulus.</param>
		/// <returns></returns>
		public BigInteger ModInverse(BigInteger modulus)
		{
			unchecked
			{
				BigInteger[] p = {0, 1};
				BigInteger[] q = new BigInteger[2]; // quotients
				BigInteger[] r = {0, 0}; // remainders

				int step = 0;

				BigInteger a = modulus;
				BigInteger b = this;

				while (b.dataLength > 1 || (b.dataLength == 1 && b.data[0] != 0))
				{
					BigInteger quotient = new BigInteger();
					BigInteger remainder = new BigInteger();

					if (step > 1)
					{
						BigInteger pval = (p[0] - (p[1] * q[0])) % modulus;
						p[0] = p[1];
						p[1] = pval;
					}

					if (b.dataLength == 1)
						singleByteDivide(a, b, quotient, remainder);
					else
						multiByteDivide(a, b, quotient, remainder);

					/*
								Console.WriteLine(quotient.dataLength);
								Console.WriteLine("{0} = {1}({2}) + {3}  p = {4}", a.ToString(10),
												b.ToString(10), quotient.ToString(10), remainder.ToString(10),
												p[1].ToString(10));
								*/

					q[0] = q[1];
					r[0] = r[1];
					q[1] = quotient;
					r[1] = remainder;

					a = b;
					b = remainder;

					step++;
				}

				if (r[0].dataLength > 1 || (r[0].dataLength == 1 && r[0].data[0] != 1))
					throw (new ArithmeticException("No inverse!"));

				BigInteger result = ((p[0] - (p[1] * q[0])) % modulus);

				if ((result.data[maxLength - 1] & 0x80000000) != 0)
					result += modulus; // get the least positive modulus

				return result;
			}
		}

		//***********************************************************************
		// Returns the value of the BigInteger as a byte array.  The lowest
		// index contains the MSB.
		//***********************************************************************

		/// <summary>
		/// Returns the value of the BigInteger as a byte array.  The lowest
		/// index contains the MSB.
		/// </summary>
		/// <returns></returns>
		public byte[] ToByteArray()
		{
			unchecked
			{
				int numBits = BitCount();

				int numBytes = numBits >> 3;
				if ((numBits & 0x7) != 0)
					numBytes++;

				byte[] result = new byte[numBytes];

				//Console.WriteLine(result.Length);

				int pos = 0;
				uint tempVal, val = data[dataLength - 1];

				bool found = false;
				if ((tempVal = (val >> 24 & 0xFF)) != 0)
				{
					found = true;
					result[pos++] = (byte)tempVal;
				}
				if ((tempVal = (val >> 16 & 0xFF)) != 0 | found)
				{
					found = true;
					result[pos++] = (byte)tempVal;
				}
				if ((tempVal = (val >> 8 & 0xFF)) != 0 | found)
				{
					found = true;
					result[pos++] = (byte)tempVal;
				}
				if ((tempVal = (val & 0xFF)) != 0 | found)
					result[pos++] = (byte)tempVal;

				for (int i = dataLength - 2; i >= 0; i--, pos += 4)
				{
					val = data[i];
					result[pos + 3] = (byte)(val & 0xFF);
					val >>= 8;
					result[pos + 2] = (byte)(val & 0xFF);
					val >>= 8;
					result[pos + 1] = (byte)(val & 0xFF);
					val >>= 8;
					result[pos] = (byte)(val & 0xFF);
				}

				return result;
			}
		}

		//***********************************************************************
		// Sets the value of the specified bit to 1
		// The Least Significant Bit position is 0.
		//***********************************************************************

		/// <summary>
		/// Sets the value of the specified bit to 1
		/// The Least Significant Bit position is 0.
		/// </summary>
		/// <param name="bitNumber">The bit num.</param>
		[CLSCompliant(false)]
		public void SetBit(uint bitNumber)
		{
			unchecked
			{
				uint bytePos = bitNumber >> 5; // divide by 32
				byte bitPos = (byte)(bitNumber & 0x1F); // get the lowest 5 bits

				uint mask = (uint)1 << bitPos;
				data[bytePos] |= mask;

				if (bytePos >= dataLength)
					dataLength = (int)bytePos + 1;
			}
		}

		//***********************************************************************
		// Sets the value of the specified bit to 0
		// The Least Significant Bit position is 0.
		//***********************************************************************

		/// <summary>
		/// Sets the value of the specified bit to 0
		/// The Least Significant Bit position is 0.
		/// </summary>
		/// <param name="bitNumber">The bit num.</param>
		[CLSCompliant(false)]
		public void UnsetBit(uint bitNumber)
		{
			unchecked
			{
				uint bytePos = bitNumber >> 5;

				if (bytePos < dataLength)
				{
					byte bitPos = (byte)(bitNumber & 0x1F);

					uint mask = (uint)1 << bitPos;
					uint mask2 = 0xFFFFFFFF ^ mask;

					data[bytePos] &= mask2;

					if (dataLength > 1 && data[dataLength - 1] == 0)
						dataLength--;
				}
			}
		}

		//***********************************************************************
		// Returns a value that is equivalent to the integer square root
		// of the BigInteger.
		//
		// The integer square root of "this" is defined as the largest integer n
		// such that (n * n) <= this
		//
		//***********************************************************************

		/// <summary>
		/// Returns a value that is equivalent to the integer square root
		/// of the BigInteger.
		/// </summary>
		/// <returns></returns>
		public BigInteger Sqrt()
		{
			unchecked
			{
				uint numBits = (uint)BitCount();

				if ((numBits & 0x1) != 0) // odd number of bits
					numBits = (numBits >> 1) + 1;
				else
					numBits = (numBits >> 1);

				uint bytePos = numBits >> 5;
				byte bitPos = (byte)(numBits & 0x1F);

				uint mask;

				BigInteger result = new BigInteger();
				if (bitPos == 0)
					mask = 0x80000000;
				else
				{
					mask = (uint)1 << bitPos;
					bytePos++;
				}
				result.dataLength = (int)bytePos;

				for (int i = (int)bytePos - 1; i >= 0; i--)
				{
					while (mask != 0)
					{
						// guess
						result.data[i] ^= mask;

						// undo the guess if its square is larger than this
						if ((result * result) > this)
							result.data[i] ^= mask;

						mask >>= 1;
					}
					mask = 0x80000000;
				}
				return result;
			}
		}

		//***********************************************************************
		// Returns the k_th number in the Lucas Sequence reduced modulo n.
		//
		// Uses index doubling to speed up the process.  For example, to calculate V(k),
		// we maintain two numbers in the sequence V(n) and V(n+1).
		//
		// To obtain V(2n), we use the identity
		//      V(2n) = (V(n) * V(n)) - (2 * Q^n)
		// To obtain V(2n+1), we first write it as
		//      V(2n+1) = V((n+1) + n)
		// and use the identity
		//      V(m+n) = V(m) * V(n) - Q * V(m-n)
		// Hence,
		//      V((n+1) + n) = V(n+1) * V(n) - Q^n * V((n+1) - n)
		//                   = V(n+1) * V(n) - Q^n * V(1)
		//                   = V(n+1) * V(n) - Q^n * P
		//
		// We use k in its binary expansion and perform index doubling for each
		// bit position.  For each bit position that is set, we perform an
		// index doubling followed by an index addition.  This means that for V(n),
		// we need to update it to V(2n+1).  For V(n+1), we need to update it to
		// V((2n+1)+1) = V(2*(n+1))
		//
		// This function returns
		// [0] = U(k)
		// [1] = V(k)
		// [2] = Q^n
		//
		// Where U(0) = 0 % n, U(1) = 1 % n
		//       V(0) = 2 % n, V(1) = P % n
		//***********************************************************************

		/// <summary>
		/// Returns the k_th number in the Lucas Sequence reduced modulo n.
		/// </summary>
		/// <param name="P">P.</param>
		/// <param name="Q">Q.</param>
		/// <param name="k">k.</param>
		/// <param name="n">n.</param>
		/// <returns></returns>
		public static BigInteger[] LucasSequence(BigInteger P, BigInteger Q,
												 BigInteger k, BigInteger n)
		{
			unchecked
			{
				if ((object)P == null)
					throw new ArgumentNullException("P");
				if ((object)Q == null)
					throw new ArgumentNullException("Q");
				if ((object)k == null)
					throw new ArgumentNullException("k");
				if ((object)n == null)
					throw new ArgumentNullException("n");
				if (k.dataLength == 1 && k.data[0] == 0)
				{
					BigInteger[] result = new BigInteger[3];

					result[0] = 0;
					result[1] = 2 % n;
					result[2] = 1 % n;
					return result;
				}

				// calculate constant = b^(2k) / m
				// for Barrett Reduction
				BigInteger constant = new BigInteger();

				int nLen = n.dataLength << 1;
				constant.data[nLen] = 0x00000001;
				constant.dataLength = nLen + 1;

				constant = constant / n;

				// calculate values of s and t
				int s = 0;

				for (int index = 0; index < k.dataLength; index++)
				{
					uint mask = 0x01;

					for (int i = 0; i < 32; i++)
					{
						if ((k.data[index] & mask) != 0)
						{
							index = k.dataLength; // to break the outer loop
							break;
						}
						mask <<= 1;
						s++;
					}
				}

				BigInteger t = k >> s;

				//Console.WriteLine("s = " + s + " t = " + t);
				return LucasSequenceHelper(P, Q, t, n, constant, s);
			}
		}

		//***********************************************************************
		// Performs the calculation of the kth term in the Lucas Sequence.
		// For details of the algorithm, see reference [9].
		//
		// k must be odd.  i.e LSB == 1
		//***********************************************************************

		private static BigInteger[] LucasSequenceHelper(BigInteger P, BigInteger Q,
														BigInteger k, BigInteger n,
														BigInteger constant, int s)
		{
			unchecked
			{
				BigInteger[] result = new BigInteger[3];

				if ((k.data[0] & 0x00000001) == 0)
					throw (new ArgumentException("Argument k must be odd."));

				int numbits = k.BitCount();
				uint mask = (uint)0x1 << ((numbits & 0x1F) - 1);

				// v = v0, v1 = v1, u1 = u1, Q_k = Q^0

				BigInteger v = 2 % n,
					Q_k = 1 % n,
					v1 = P % n,
					u1 = Q_k;
				bool flag = true;

				for (int i = k.dataLength - 1; i >= 0; i--) // iterate on the binary expansion of k
				{
					//Console.WriteLine("round");
					while (mask != 0)
					{
						if (i == 0 && mask == 0x00000001) // last bit
							break;

						if ((k.data[i] & mask) != 0) // bit is set
						{
							// index doubling with addition

							u1 = (u1 * v1) % n;

							v = ((v * v1) - (P * Q_k)) % n;
							v1 = BarrettReduction(v1 * v1, n, constant);
							v1 = (v1 - ((Q_k * Q) << 1)) % n;

							if (flag)
								flag = false;
							else
								Q_k = BarrettReduction(Q_k * Q_k, n, constant);

							Q_k = (Q_k * Q) % n;
						}
						else
						{
							// index doubling
							u1 = ((u1 * v) - Q_k) % n;

							v1 = ((v * v1) - (P * Q_k)) % n;
							v = BarrettReduction(v * v, n, constant);
							v = (v - (Q_k << 1)) % n;

							if (flag)
							{
								Q_k = Q % n;
								flag = false;
							}
							else
								Q_k = BarrettReduction(Q_k * Q_k, n, constant);
						}

						mask >>= 1;
					}
					mask = 0x80000000;
				}

				// at this point u1 = u(n+1) and v = v(n)
				// since the last bit always 1, we need to transform u1 to u(2n+1) and v to v(2n+1)

				u1 = ((u1 * v) - Q_k) % n;
				v = ((v * v1) - (P * Q_k)) % n;
				if (flag)
					flag = false;
				else
					Q_k = BarrettReduction(Q_k * Q_k, n, constant);

				Q_k = (Q_k * Q) % n;

				for (int i = 0; i < s; i++)
				{
					// index doubling
					u1 = (u1 * v) % n;
					v = ((v * v) - (Q_k << 1)) % n;

					if (flag)
					{
						Q_k = Q % n;
						flag = false;
					}
					else
						Q_k = BarrettReduction(Q_k * Q_k, n, constant);
				}

				result[0] = u1;
				result[1] = v;
				result[2] = Q_k;

				return result;
			}
		}

#if biginteger_test
	//***********************************************************************
	// Tests the correct implementation of the /, %, * and + operators
	//***********************************************************************

		internal static void MulDivTest(int rounds)
		{
			Random rand = new Random();
			byte[] val = new byte[64];
			byte[] val2 = new byte[64];

			for (int count = 0; count < rounds; count++)
			{
				// generate 2 numbers of random length
				int t1 = 0;
				while (t1 == 0)
					t1 = (int)(rand.NextDouble() * 65);

				int t2 = 0;
				while (t2 == 0)
					t2 = (int)(rand.NextDouble() * 65);

				bool done = false;
				while (!done)
				{
					for (int i = 0; i < 64; i++)
					{
						if (i < t1)
							val[i] = (byte)(rand.NextDouble() * 256);
						else
							val[i] = 0;

						if (val[i] != 0)
							done = true;
					}
				}

				done = false;
				while (!done)
				{
					for (int i = 0; i < 64; i++)
					{
						if (i < t2)
							val2[i] = (byte)(rand.NextDouble() * 256);
						else
							val2[i] = 0;

						if (val2[i] != 0)
							done = true;
					}
				}

				while (val[0] == 0)
					val[0] = (byte)(rand.NextDouble() * 256);
				while (val2[0] == 0)
					val2[0] = (byte)(rand.NextDouble() * 256);

				Console.WriteLine(count);
				BigInteger bn1 = new BigInteger(val, t1);
				BigInteger bn2 = new BigInteger(val2, t2);

				// Determine the quotient and remainder by dividing
				// the first number by the second.

				BigInteger bn3 = bn1 / bn2;
				BigInteger bn4 = bn1 % bn2;

				// Recalculate the number
				BigInteger bn5 = (bn3 * bn2) + bn4;

				// Make sure they're the same
				if (bn5 != bn1)
				{
					Console.WriteLine("Error at " + count);
					Console.WriteLine(bn1 + "\n");
					Console.WriteLine(bn2 + "\n");
					Console.WriteLine(bn3 + "\n");
					Console.WriteLine(bn4 + "\n");
					Console.WriteLine(bn5 + "\n");
					return;
				}
			}
		}

		//***********************************************************************
		// Tests the correct implementation of the modulo exponential function
		// using RSA encryption and decryption (using pre-computed encryption and
		// decryption keys).
		//***********************************************************************

		internal static void RSATest(int rounds)
		{
			Random rand = new Random(1);
			byte[] val = new byte[64];

			// private and public key
			BigInteger bi_e =
				new BigInteger(
					"a932b948feed4fb2b692609bd22164fc9edb59fae7880cc1eaff7b3c9626b7e5b241c27a974833b2622ebe09beb451917663d47232488f23a117fc97720f1e7",
					16);
			BigInteger bi_d =
				new BigInteger(
					"4adf2f7a89da93248509347d2ae506d683dd3a16357e859a980c4f77a4e2f7a01fae289f13a851df6e9db5adaa60bfd2b162bbbe31f7c8f828261a6839311929d2cef4f864dde65e556ce43c89bbbf9f1ac5511315847ce9cc8dc92470a747b8792d6a83b0092d2e5ebaf852c85cacf34278efa99160f2f8aa7ee7214de07b7",
					16);
			BigInteger bi_n =
				new BigInteger(
					"e8e77781f36a7b3188d711c2190b560f205a52391b3479cdb99fa010745cbeba5f2adc08e1de6bf38398a0487c4a73610d94ec36f17f3f46ad75e17bc1adfec99839589f45f95ccc94cb2a5c500b477eb3323d8cfab0c8458c96f0147a45d27e45a4d11d54d77684f65d48f15fafcc1ba208e71e921b9bd9017c16a5231af7f",
					16);

			Console.WriteLine("e =\n" + bi_e.ToString(10));
			Console.WriteLine("\nd =\n" + bi_d.ToString(10));
			Console.WriteLine("\nn =\n" + bi_n.ToString(10) + "\n");

			for (int count = 0; count < rounds; count++)
			{
				// generate data of random length
				int t1 = 0;
				while (t1 == 0)
					t1 = (int)(rand.NextDouble() * 65);

				bool done = false;
				while (!done)
				{
					for (int i = 0; i < 64; i++)
					{
						if (i < t1)
							val[i] = (byte)(rand.NextDouble() * 256);
						else
							val[i] = 0;

						if (val[i] != 0)
							done = true;
					}
				}

				while (val[0] == 0)
					val[0] = (byte)(rand.NextDouble() * 256);

				Console.Write("Round = " + count);

				// encrypt and decrypt data
				BigInteger bi_data = new BigInteger(val, t1);
				BigInteger bi_encrypted = bi_data.ModPow(bi_e, bi_n);
				BigInteger bi_decrypted = bi_encrypted.ModPow(bi_d, bi_n);

				// compare
				if (bi_decrypted != bi_data)
				{
					Console.WriteLine("\nError at round " + count);
					Console.WriteLine(bi_data + "\n");
					return;
				}
				Console.WriteLine(" <PASSED>.");
			}
		}

		//***********************************************************************
		// Tests the correct implementation of the modulo exponential and
		// inverse modulo functions using RSA encryption and decryption.  The two
		// pseudoprimes p and q are fixed, but the two RSA keys are generated
		// for each round of testing.
		//***********************************************************************

		internal static void RSATest2(int rounds)
		{
			Random rand = new Random();
			byte[] val = new byte[64];

			byte[] pseudoPrime1 = {
			                      	0x85, 0x84, 0x64, 0xFD, 0x70, 0x6A,
			                      	0x9F, 0xF0, 0x94, 0x0C, 0x3E, 0x2C,
			                      	0x74, 0x34, 0x05, 0xC9, 0x55, 0xB3,
			                      	0x85, 0x32, 0x98, 0x71, 0xF9, 0x41,
			                      	0x21, 0x5F, 0x02, 0x9E, 0xEA, 0x56,
			                      	0x8D, 0x8C, 0x44, 0xCC, 0xEE, 0xEE,
			                      	0x3D, 0x2C, 0x9D, 0x2C, 0x12, 0x41,
			                      	0x1E, 0xF1, 0xC5, 0x32, 0xC3, 0xAA,
			                      	0x31, 0x4A, 0x52, 0xD8, 0xE8, 0xAF,
			                      	0x42, 0xF4, 0x72, 0xA1, 0x2A, 0x0D,
			                      	0x97, 0xB1, 0x31, 0xB3,
			                      };

			byte[] pseudoPrime2 = {
			                      	0x99, 0x98, 0xCA, 0xB8, 0x5E, 0xD7,
			                      	0xE5, 0xDC, 0x28, 0x5C, 0x6F, 0x0E,
			                      	0x15, 0x09, 0x59, 0x6E, 0x84, 0xF3,
			                      	0x81, 0xCD, 0xDE, 0x42, 0xDC, 0x93,
			                      	0xC2, 0x7A, 0x62, 0xAC, 0x6C, 0xAF,
			                      	0xDE, 0x74, 0xE3, 0xCB, 0x60, 0x20,
			                      	0x38, 0x9C, 0x21, 0xC3, 0xDC, 0xC8,
			                      	0xA2, 0x4D, 0xC6, 0x2A, 0x35, 0x7F,
			                      	0xF3, 0xA9, 0xE8, 0x1D, 0x7B, 0x2C,
			                      	0x78, 0xFA, 0xB8, 0x02, 0x55, 0x80,
			                      	0x9B, 0xC2, 0xA5, 0xCB,
			                      };

			BigInteger bi_p = new BigInteger(pseudoPrime1);
			BigInteger bi_q = new BigInteger(pseudoPrime2);
			BigInteger bi_pq = (bi_p - 1) * (bi_q - 1);
			BigInteger bi_n = bi_p * bi_q;

			for (int count = 0; count < rounds; count++)
			{
				// generate private and public key
				BigInteger bi_e = bi_pq.GenerateCoprime(512, rand);
				BigInteger bi_d = bi_e.ModInverse(bi_pq);

				Console.WriteLine("\ne =\n" + bi_e.ToString(10));
				Console.WriteLine("\nd =\n" + bi_d.ToString(10));
				Console.WriteLine("\nn =\n" + bi_n.ToString(10) + "\n");

				// generate data of random length
				int t1 = 0;
				while (t1 == 0)
					t1 = (int)(rand.NextDouble() * 65);

				bool done = false;
				while (!done)
				{
					for (int i = 0; i < 64; i++)
					{
						if (i < t1)
							val[i] = (byte)(rand.NextDouble() * 256);
						else
							val[i] = 0;

						if (val[i] != 0)
							done = true;
					}
				}

				while (val[0] == 0)
					val[0] = (byte)(rand.NextDouble() * 256);

				Console.Write("Round = " + count);

				// encrypt and decrypt data
				BigInteger bi_data = new BigInteger(val, t1);
				BigInteger bi_encrypted = bi_data.ModPow(bi_e, bi_n);
				BigInteger bi_decrypted = bi_encrypted.ModPow(bi_d, bi_n);

				// compare
				if (bi_decrypted != bi_data)
				{
					Console.WriteLine("\nError at round " + count);
					Console.WriteLine(bi_data + "\n");
					return;
				}
				Console.WriteLine(" <PASSED>.");
			}
		}

		//***********************************************************************
		// Tests the correct implementation of sqrt() method.
		//***********************************************************************

		internal static void SqrtTest(int rounds)
		{
			Random rand = new Random();
			for (int count = 0; count < rounds; count++)
			{
				// generate data of random length
				int t1 = 0;
				while (t1 == 0)
					t1 = (int)(rand.NextDouble() * 1024);

				Console.Write("Round = " + count);

				BigInteger a = new BigInteger();
				a.GenerateRandomBits(t1, rand);

				BigInteger b = a.Sqrt();
				BigInteger c = (b + 1) * (b + 1);

				// check that b is the largest integer such that b*b <= a
				if (c <= a)
				{
					Console.WriteLine("\nError at round " + count);
					Console.WriteLine(a + "\n");
					return;
				}
				Console.WriteLine(" <PASSED>.");
			}
		}

		internal static void Main(string[] args)
		{
			// Known problem -> these two pseudoprimes passes my implementation of
			// primality test but failed in JDK's isProbablePrime test.

			byte[] pseudoPrime1 = {
			                        0x00,
			                        0x85, 0x84, 0x64, 0xFD, 0x70, 0x6A,
			                        0x9F, 0xF0, 0x94, 0x0C, 0x3E, 0x2C,
			                        0x74, 0x34, 0x05, 0xC9, 0x55, 0xB3,
			                        0x85, 0x32, 0x98, 0x71, 0xF9, 0x41,
			                        0x21, 0x5F, 0x02, 0x9E, 0xEA, 0x56,
			                        0x8D, 0x8C, 0x44, 0xCC, 0xEE, 0xEE,
			                        0x3D, 0x2C, 0x9D, 0x2C, 0x12, 0x41,
			                        0x1E, 0xF1, 0xC5, 0x32, 0xC3, 0xAA,
			                        0x31, 0x4A, 0x52, 0xD8, 0xE8, 0xAF,
			                        0x42, 0xF4, 0x72, 0xA1, 0x2A, 0x0D,
			                        0x97, 0xB1, 0x31, 0xB3,
			                      };

			//byte[] pseudoPrime2 = {
			//                        0x00,
			//                        0x99, 0x98, 0xCA, 0xB8, 0x5E, 0xD7,
			//                        0xE5, 0xDC, 0x28, 0x5C, 0x6F, 0x0E,
			//                        0x15, 0x09, 0x59, 0x6E, 0x84, 0xF3,
			//                        0x81, 0xCD, 0xDE, 0x42, 0xDC, 0x93,
			//                        0xC2, 0x7A, 0x62, 0xAC, 0x6C, 0xAF,
			//                        0xDE, 0x74, 0xE3, 0xCB, 0x60, 0x20,
			//                        0x38, 0x9C, 0x21, 0xC3, 0xDC, 0xC8,
			//                        0xA2, 0x4D, 0xC6, 0x2A, 0x35, 0x7F,
			//                        0xF3, 0xA9, 0xE8, 0x1D, 0x7B, 0x2C,
			//                        0x78, 0xFA, 0xB8, 0x02, 0x55, 0x80,
			//                        0x9B, 0xC2, 0xA5, 0xCB,
			//                      };

			Console.WriteLine("List of primes < 2000\n---------------------");
			int limit = 100, count = 0;
			for (int i = 0; i < 2000; i++)
			{
				if (i >= limit)
				{
					Console.WriteLine();
					limit += 100;
				}

				BigInteger p = new BigInteger(-i);

				if (p.IsProbablePrime())
				{
					Console.Write(i + ", ");
					count++;
				}
			}
			Console.WriteLine("\nCount = " + count);

			BigInteger operand = new BigInteger(pseudoPrime1);
			Console.WriteLine("\n\nPrimality testing for\n" + operand + "\n");
			Console.WriteLine("SolovayStrassenTest(5) = " + operand.SolovayStrassenTest(5));
			Console.WriteLine("RabinMillerTest(5) = " + operand.RabinMillerTest(5));
			Console.WriteLine("FermatLittleTest(5) = " + operand.FermatLittleTest(5));
			Console.WriteLine("isProbablePrime() = " + operand.IsProbablePrime());

			Console.Write("\nGenerating 512-bits random pseudoprime. . .");
			Random rand = new Random();
			BigInteger prime = GeneratePseudoPrime(512, 5, rand);
			Console.WriteLine("\n" + prime);

			//int dwStart = System.Environment.TickCount;
			//BigInteger.MulDivTest(100000);
			//BigInteger.RSATest(10);
			//BigInteger.RSATest2(10);
			//Console.WriteLine(System.Environment.TickCount - dwStart);
		}
#endif
	}
}