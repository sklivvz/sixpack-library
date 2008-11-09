// WebServiceException.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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
using System.Runtime.Serialization;

namespace SixPack.Web.Services
{
	/// <summary>
	/// The abstract parent exception for all fullsix web service exceptions.
	/// </summary>
	[Serializable]
	[Obsolete("This type will be removed in a future version. Please use the SixPack.CodedException class in SixPack.dll")]
	public abstract class WebServiceException : CodedException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebServiceException"/> class.
		/// </summary>
		protected WebServiceException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebServiceException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		protected WebServiceException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebServiceException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		protected WebServiceException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebServiceException"/> class.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected WebServiceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
