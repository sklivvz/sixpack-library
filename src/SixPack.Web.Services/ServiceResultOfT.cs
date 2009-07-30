// ServiceResultOfT.cs 
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

namespace SixPack.Web.Services
{
	/// <summary>
	/// Provides a wrapper result class for webservices (functions with return type T).
	/// </summary>
	/// <typeparam name="T">The return type to be handled.</typeparam>
	[Serializable]
	public sealed class ServiceResult<T>
	{
		private readonly ServiceResult innerResult = new ServiceResult();

		private T result;

		/// <summary>
		/// Initializes a new instance of the <see cref="SixPack.Web.Services.ServiceResult{T}"/> class.
		/// </summary>
		public ServiceResult()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SixPack.Web.Services.ServiceResult{T}"/> class.
		/// </summary>
		/// <param name="result">The result.</param>
		public ServiceResult(T result)
		{
			this.result = result;
		}

		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>The code.</value>
		public int Code
		{
			get
			{
				return innerResult.Code;
			}
			set
			{
				innerResult.Code = value;
			}
		}

		/// <summary>
		/// Gets or sets the status message.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get
			{
				return innerResult.Message;
			}
			set
			{
				innerResult.Message = value;
			}
		}

		/// <summary>
		/// Gets or sets the full exception in case one occurred.
		/// </summary>
		/// <value>The full exception.</value>
		public string FullException
		{
			get
			{
				return innerResult.FullException;
			}
			set
			{
				innerResult.FullException = value;
			}
		}

		/// <summary>
		/// Helper method to handle an exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		[Obsolete("This method is deprecated. You should use HandleException(Exception, bool) instead.")]
		public void HandleException(Exception exception)
		{
			innerResult.HandleException(exception);
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
			innerResult.HandleException(exception, includeException);
		}

		/// <summary>
		/// Gets or sets the result.
		/// </summary>
		/// <value>The result.</value>
		public T Result
		{
			get
			{
				return result;
			}
			set
			{
				result = value;
			}
		}
	}
}
