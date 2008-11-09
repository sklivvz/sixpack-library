// RsaSmallFullKey.cs 
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

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// Encapsulates a RSA-small full (private and public) key.
	/// </summary>
	[Serializable]
	public class RsaSmallFullKey
	{
		private readonly RsaSmallPrivateKey privateKey;
		private readonly RsaSmallPublicKey publicKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="RsaSmallFullKey"/> class.
		/// </summary>
		/// <param name="modulus">The modulus.</param>
		/// <param name="privateExponent">The private exponent.</param>
		/// <param name="publicExponent">The public exponent.</param>
		public RsaSmallFullKey(BigInteger modulus, BigInteger privateExponent, BigInteger publicExponent)
		{
			privateKey = new RsaSmallPrivateKey(modulus, privateExponent);
			publicKey = new RsaSmallPublicKey(modulus, publicExponent);
		}

		/// <summary>
		/// Gets the private key.
		/// </summary>
		/// <value>The private key.</value>
		public RsaSmallPrivateKey PrivateKey
		{
			get { return privateKey; }
		}

		/// <summary>
		/// Gets the public key.
		/// </summary>
		/// <value>The public key.</value>
		public RsaSmallPublicKey PublicKey
		{
			get { return publicKey; }
		}
	}
}
