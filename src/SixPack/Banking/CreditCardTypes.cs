// CreditCardTypes.cs 
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

namespace SixPack.Banking
{
	/// <summary>
	/// Types of Credit Cards
	/// </summary>
	[Flags]
	[Serializable]
	public enum CreditCardTypes
	{
		/// <summary>
		/// MasterCard
		/// </summary>
		MasterCard = 0x0001,

		/// <summary>
		/// VISA
		/// </summary>
		Visa = 0x0002,

		/// <summary>
		/// American Express
		/// </summary>
		AmericanExpress = 0x0004,

		/// <summary>
		/// Diners Club
		/// </summary>
		DinersClub = 0x0008,

		/// <summary>
		/// en Route
		/// </summary>
		EnRoute = 0x0010,

		/// <summary>
		/// Discover
		/// </summary>
		Discover = 0x0020,

		/// <summary>
		/// JCB
		/// </summary>
		Jcb = 0x0040,

		/// <summary>
		/// Unknown Credit Card
		/// </summary>
		Unknown = 0x0080,

		/// <summary>
		/// All Known Credit Card Types
		/// </summary>
		All = AmericanExpress | DinersClub | Discover | Discover | EnRoute | Jcb | MasterCard | Visa,

		/// <summary>
		/// Any credit card type
		/// </summary>
		Any = All | Unknown,
	}
}
