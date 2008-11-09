// ServiceResultOfT.cs 
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

namespace SixPack.Web.Services
{
	/// <summary>
	/// Provides a wrapper result class for webservices (functions with return type T).
	/// </summary>
	/// <typeparam name="T">The return type to be handled.</typeparam>
	[Serializable]
	public sealed class ServiceResult<T> : ServiceResult
	{
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
		/// Gets or sets the result.
		/// </summary>
		/// <value>The result.</value>
		public T Result
		{
			get { return result; }
			set { result = value; }
		}
	}
}
