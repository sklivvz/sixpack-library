// CachedMethodAttribute.cs 
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

namespace SixPack.ComponentModel
{
	/// <summary>
	/// Specifies that the result of the method will be cached for a set amount of time.
	/// </summary>
	/// <remarks>
	/// This attribute does not work on static methods.
	/// The <see cref="CachedAttribute"/> attribute must be applied to the class that contains this method.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class CachedMethodAttribute : Attribute
	{
		private readonly int cacheTime;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachedMethodAttribute"/> class.
		/// </summary>
		/// <param name="cacheTime">The cache time in seconds.</param>
		public CachedMethodAttribute(int cacheTime)
		{
			this.cacheTime = cacheTime;
		}

		/// <summary>
		/// Gets the cache time in seconds.
		/// </summary>
		/// <value>The cache time in seconds.</value>
		public int CacheTime
		{
			get { return cacheTime; }
		}
	}
}
