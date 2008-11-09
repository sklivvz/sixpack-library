// IPAddressFormat.cs 
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
using System.Text.RegularExpressions;

namespace SixPack.Net
{
	/// <summary>
	/// Validates IP addresses
	/// </summary>
	public static class IPAddressFormat
	{
		/// <summary>
		/// Regex expression to validate an IP address
		/// </summary>
		private static readonly Regex ipAddressPattern = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled);

		/// <summary>
		/// Determines whether the specified ip address is valid.
		/// </summary>
		/// <param name="ipAddress">The ip address.</param>
		/// <returns>
		/// 	<c>true</c> if the specified ip address is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string ipAddress)
		{
			return ipAddressPattern.IsMatch(ipAddress);
		}
	}
}
