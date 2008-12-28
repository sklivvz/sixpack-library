// PublishMethodAttribute.cs 
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
using System.EnterpriseServices;

namespace SixPack.Web.Services
{
	/// <summary>
	/// Methods with this attribute will be published as webmethods by wserv.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public sealed class PublishMethodAttribute : Attribute
	{
		// Fields
		private readonly bool bufferResponse = true;
		private readonly int cacheDuration;
		private readonly bool enableSession;
		private readonly int transactionOption;
		private string description;
		private string messageName;

		// Methods
		/// <summary>
		/// Initializes a new instance of the <see cref="PublishMethodAttribute"/> class.
		/// </summary>
		public PublishMethodAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PublishMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">if set to <c>true</c> enable session.</param>
		public PublishMethodAttribute(bool enableSession)
			: this()
		{
			this.enableSession = enableSession;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PublishMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">if set to <c>true</c> enable session.</param>
		/// <param name="transactionOption">The transaction option.</param>
		public PublishMethodAttribute(bool enableSession, TransactionOption transactionOption)
			: this(enableSession)
		{
			this.transactionOption = (int) transactionOption;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PublishMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">if set to <c>true</c> enable session.</param>
		/// <param name="transactionOption">The transaction option.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		public PublishMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration)
			: this(enableSession, transactionOption)
		{
			this.cacheDuration = cacheDuration;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PublishMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">if set to <c>true</c> enable session.</param>
		/// <param name="transactionOption">The transaction option.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <param name="bufferResponse">if set to <c>true</c> buffer response.</param>
		public PublishMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration,
		                              bool bufferResponse)
			: this(enableSession, transactionOption, cacheDuration)
		{
			this.bufferResponse = bufferResponse;
		}

		// Properties
		/// <summary>
		/// Gets or sets a value indicating whether to buffer response.
		/// </summary>
		/// <value><c>true</c> if buffering response; otherwise, <c>false</c>.</value>
		public bool BufferResponse
		{
			get { return bufferResponse; }
		}

		/// <summary>
		/// Gets or sets the cache duration.
		/// </summary>
		/// <value>The cache duration.</value>
		public int CacheDuration
		{
			get { return cacheDuration; }
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get
			{
				if (description != null)
					return description;
				return string.Empty;
			}
			set { description = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to enable session.
		/// </summary>
		/// <value><c>true</c> if session enabled; otherwise, <c>false</c>.</value>
		public bool EnableSession
		{
			get { return enableSession; }
		}

		/// <summary>
		/// Gets or sets the name of the message.
		/// </summary>
		/// <value>The name of the message.</value>
		public string MessageName
		{
			get
			{
				if (messageName != null)
					return messageName;
				return string.Empty;
			}
			set { messageName = value; }
		}

		/// <summary>
		/// Gets or sets the transaction option.
		/// </summary>
		/// <value>The transaction option.</value>
		public TransactionOption TransactionOption
		{
			get { return (TransactionOption) transactionOption; }
		}
	}
}
