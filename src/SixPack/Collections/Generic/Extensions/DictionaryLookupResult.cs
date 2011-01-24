// DictionaryLookupResult.cs
//
//  Copyright (C) 2011 Antoine Aubry
//  Author: Antoine Aubry
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

namespace SixPack.Collections.Generic.Extensions
{
	/// <summary>
	/// Contains the result of a dictionary lookup.
	/// </summary>
	/// <typeparam name="T">The type of the result.</typeparam>
	public struct DictionaryLookupResult<T>
	{
		private readonly bool _found;

		/// <summary>
		/// Gets a value indicating whether the key was found.
		/// </summary>
		/// <value><c>true</c> if found; otherwise, <c>false</c>.</value>
		public bool Found
		{
			get
			{
				return _found;
			}
		}

		private readonly T _result;

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>The result.</value>
		public T Result
		{
			get
			{
				return _result;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DictionaryLookupResult&lt;T&gt;"/> struct.
		/// </summary>
		/// <param name="found">if set to <c>true</c> [found].</param>
		/// <param name="result">The result.</param>
		public DictionaryLookupResult(bool found, T result)
		{
			_found = found;
			_result = result;
		}
	}
}