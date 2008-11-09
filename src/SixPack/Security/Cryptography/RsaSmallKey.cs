// RsaSmallKey.cs 
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
	/// Abstract class that encapsulates a RSA-small key.
	/// </summary>
	[Serializable]
	public abstract class RsaSmallKey
	{
		private readonly BigInteger exponent;
		private readonly BigInteger modulus;

		/// <summary>
		/// Initializes a new instance of the <see cref="RsaSmallKey"/> class.
		/// </summary>
		/// <param name="modulus">The modulus.</param>
		/// <param name="exponent">The exponent.</param>
		protected RsaSmallKey(BigInteger modulus, BigInteger exponent)
		{
			this.exponent = exponent;
			this.modulus = modulus;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RsaSmallKey"/> class.
		/// </summary>
		protected RsaSmallKey()
		{
		}

		/// <summary>
		/// Gets the exponent.
		/// </summary>
		/// <value>The exponent.</value>
		public BigInteger Exponent
		{
			get { return exponent; }
		}

		/// <summary>
		/// Gets the modulus.
		/// </summary>
		/// <value>The modulus.</value>
		public BigInteger Modulus
		{
			get { return modulus; }
		}
	}
}
