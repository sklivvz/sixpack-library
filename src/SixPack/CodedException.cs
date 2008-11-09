// CodedException.cs 
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
using System.Globalization;
using System.Runtime.Serialization;
using SixPack.Diagnostics;

namespace SixPack
{
	/// <summary>
	/// The abstract parent exception for all exceptions which have a code and a message.
	/// </summary>
	[Serializable]
	public abstract class CodedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodedException"/> class.
		/// </summary>
		protected CodedException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		protected CodedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		protected CodedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodedException"/> class.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected CodedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Gets the error code.
		/// </summary>
		/// <value>The error code.</value>
		public abstract int ErrorCode { get; }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		/// <value>The error message.</value>
		public abstract string ErrorMessage { get; }

		/// <summary>
		/// Gets the error code from a resource string.
		/// </summary>
		/// <param name="resource">The resource string.</param>
		/// <returns></returns>
		protected static int GetErrorCodeFromResource(string resource)
		{
			if (string.IsNullOrEmpty(resource))
				throw new ArgumentNullException("resource");
			try
			{
				return
					int.Parse(resource.Split('|')[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			catch (ArgumentException e)
			{
				Log.Instance.HandleException(e, LogLevel.Critical);
				return -1;
			}
			catch (OverflowException e)
			{
				Log.Instance.HandleException(e, LogLevel.Critical);
				return -1;
			}
			catch (FormatException e)
			{
				Log.Instance.HandleException(e, LogLevel.Critical);
				return -1;
			}
		}

		/// <summary>
		/// Gets the error message from a resource string.
		/// </summary>
		/// <param name="resource">The resource string.</param>
		/// <returns></returns>
		protected static string GetErrorMessageFromResource(string resource)
		{
			if (string.IsNullOrEmpty(resource))
				throw new ArgumentNullException("resource");
			try
			{
				return resource.Split('|')[1];
			}
			catch (ArgumentException e)
			{
				Log.Instance.HandleException(e, LogLevel.Critical);
				return e.ToString();
			}
		}
	}
}
