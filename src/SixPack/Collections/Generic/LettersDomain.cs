// LettersDomain.cs 
//
//  Copyright (C) 2012 Antoine Aubry
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

using System.Diagnostics;
using System.Globalization;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Represents the domain of lower case letters from the latin alphabet.
	/// </summary>
	public sealed class LettersDomain : ITrieDomain<char>
	{
		int ITrieDomain<char>.Size
		{
			get { return 26; }
		}

		int ITrieDomain<char>.MapFromDomain(char atom)
		{
			atom = char.ToLower(atom, CultureInfo.InvariantCulture);
			int index = atom - 'a';
			Debug.Assert(index >= 0 && index < ((ITrieDomain<char>)this).Size);
			return index;
		}

		char ITrieDomain<char>.MapToDomain(int value)
		{
			return (char)(value + 'a');
		}
	}
}
