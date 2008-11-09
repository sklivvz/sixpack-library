// PhoneNumberType.cs 
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

namespace SixPack.Globalization.Portugal
{
	/// <summary>
	/// See http://www.anacom.pt/template12.jsp?categoryId=5344
	/// </summary>
	[Flags]
	public enum PhoneNumberTypes
	{
		/// <summary>
		/// Invalid phone number
		/// </summary>
		None = 0,

		/// <summary>
		/// International access prefix
		/// </summary>
		International = 1,

		/// <summary>
		/// Short Numbers
		/// </summary>
		ShortNumber = 2,

		/// <summary>
		/// Fixed Telephone Service
		/// </summary>
		FixedService = 4,

		/// <summary>
		/// Nomadic services
		/// </summary>
		NomadicService = 8,

		/// <summary>
		/// Audiotext, Data Network Access, etc.
		/// </summary>
		AudioText = 16,

		/// <summary>
		/// Private Voice Network and Universal Access Services, etc
		/// </summary>
		PrivateVoiceNetwork = 32,

		/// <summary>
		/// Free Services for the Caller, Virtual Call Card Services, Trunk Call Services, Personal Number
		/// </summary>
		FreeServices = 64,

		/// <summary>
		/// Mobile Communications Services
		/// </summary>
		MobileCommunicationServices = 128,

		/// <summary>
		/// Numbers that normal people have
		/// </summary>
		PersonalNumber = FixedService | MobileCommunicationServices,

		/// <summary>
		/// Numbers that companies have
		/// </summary>
		CompanyNumber = FixedService | MobileCommunicationServices | FreeServices | PrivateVoiceNetwork,
	}
}
