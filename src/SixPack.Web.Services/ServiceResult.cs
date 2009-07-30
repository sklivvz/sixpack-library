// ServiceResult.cs 
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
using SixPack.Diagnostics;

namespace SixPack.Web.Services
{
	/// <summary>
	/// Provides a wrapper result class for webservices (functions with void return type).
	/// </summary>
	[Serializable]
	public class ServiceResult
	{
		private int code;
		private string fullException;

		private string message = "OK";

		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>The code.</value>
		public int Code
		{
			get { return code; }
			set { code = value; }
		}

		/// <summary>
		/// Gets or sets the status message.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		/// <summary>
		/// Gets or sets the full exception in case one occurred.
		/// </summary>
		/// <value>The full exception.</value>
		public string FullException
		{
			get { return fullException; }
			set { fullException = value; }
		}

		/// <summary>
		/// Helper method to handle an exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		[Obsolete("This method is deprecated. You should use HandleException(Exception, bool) instead.")]
		public void HandleException(Exception exception)
		{
			HandleException(exception, true);
		}

		/// <summary>
		/// Helper method to handle an exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="includeException">if set to <c>true</c> include the full exception.</param>
		/// <remarks>
		/// Setting <paramref name="includeException"/> to true can introduce a security vulnerability
		/// because an attacker can use it to gather information about the system.
		/// </remarks>
		public void HandleException(Exception exception, bool includeException)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			Log.Instance.HandleException(exception);
			CodedException ce = exception as CodedException;
			if (ce != null)
			{
				code = ce.ErrorCode;
				message = ce.ErrorMessage;
			}
			else
			{
				code = -1;
				message = exception.Message;
			}
			if (includeException)
			{
				fullException = exception.ToString();
			}
		}
	}
}
