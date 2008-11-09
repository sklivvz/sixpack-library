// MailSemanticValidationMode.cs 
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

namespace SixPack.Net.Mail
{
	/// <summary>
	/// Specifies the semantic validation mode for email
	/// </summary>
	public enum MailSemanticValidationMode
	{
		/// <summary>
		/// No validation is performed (default)
		/// </summary>
		None,

		/// <summary>
		/// Looks up the DNS records of the email host
		/// </summary>
		DnsLookup,

		/// <summary>
		/// Tries to contact the mail server.
		/// </summary>
		ServerLookup,

		/// <summary>
		/// Tries to lookup the user on the mail server. 
		/// Very strict but with false errors.
		/// </summary>
		UserLookup
	}
}
