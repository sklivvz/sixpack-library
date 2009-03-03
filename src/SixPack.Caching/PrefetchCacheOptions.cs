// PrefetchCacheStrategy.cs
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi <marco.cecconi@gmail.com>
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

namespace SixPack.Caching
{
	/// <summary>
	/// Specifies the prefetch behaviour of the cache.
	/// </summary>
	[Flags]
	public enum PrefetchCacheOptions
	{
		/// <summary>
		/// Simple behavior. Always returns an updated result at the cost of waiting.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Allows the cache to return empty results if a non empty result would involve waiting.
		/// </summary>
		AllowNoResult = 0x01,
		/// <summary>
		/// Allows the cache to return outdated results if an updated resould would involve waiting.
		/// </summary>
		AllowExpired = 0x02
	}
}