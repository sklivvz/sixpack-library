// RsaSmall.cs 
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

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// Rsa Cryptography for "small" numbers (RSA-small)
	/// </summary>
	[Serializable]
	public class RsaSmall
	{
		private readonly RsaSmallFullKey key;

		/// <summary>
		/// Initializes a new instance of the <see cref="RsaSmall"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		public RsaSmall(RsaSmallFullKey key)
		{
			this.key = key;
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public RsaSmallFullKey Key
		{
			get { return key; }
		}

		/// <summary>
		/// Encrypts the specified Biginteger.
		/// </summary>
		/// <param name="decryptedValue">The Biginteger to encrypt.</param>
		/// <returns></returns>
		public BigInteger Encrypt(BigInteger decryptedValue)
		{
			if (decryptedValue == null)
				throw new ArgumentNullException("decryptedValue");
			return decryptedValue.ModPow(key.PrivateKey.Exponent, key.PrivateKey.Modulus);
		}

		/// <summary>
		/// Decrypts the specified Biginteger.
		/// </summary>
		/// <param name="encryptedValue">The BigInteger to decrypt.</param>
		/// <returns></returns>
		public BigInteger Decrypt(BigInteger encryptedValue)
		{
			if (encryptedValue == null)
				throw new ArgumentNullException("encryptedValue");
			return encryptedValue.ModPow(key.PublicKey.Exponent, key.PublicKey.Modulus);
		}

		/// <summary>
		/// Generates the RSA-small key.
		/// </summary>
		/// <param name="bits">The bits.</param>
		public static RsaSmallFullKey GenerateKeys(int bits)
		{
			int pbits = bits/2;
			Random r = new Random();
			BigInteger p = BigInteger.GeneratePseudoPrime(pbits, 50000, r);
			while (p.BitCount() != pbits)
				p = BigInteger.GeneratePseudoPrime(pbits, 50000, r);
			int qbits = bits - pbits;
			BigInteger q = BigInteger.GeneratePseudoPrime(qbits, 50000, r);
			while (q.BitCount() != qbits)
				q = BigInteger.GeneratePseudoPrime(qbits, 50000, r);
			BigInteger n = p*q;
			BigInteger phi = (p - 1)*(q - 1);
			int phiBits = phi.BitCount();
			BigInteger e = phi.GenerateCoprime(phiBits, r);
			BigInteger d = e.ModInverse(phi);
			return new RsaSmallFullKey(n, e, d);
		}
	}
}
