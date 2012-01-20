// ITrieDomain.cs 
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

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// Represents the domain of possible atoms to be used in a <see cref="Trie{TAtom, TValue}"/>.
	/// A domain is a mapping from atoms to zero-based integers.
	/// </summary>
	/// <typeparam name="TAtom">The type of the atom.</typeparam>
	public interface ITrieDomain<TAtom>
	{
		/// <summary>
		/// Gets the size of the domain (the number of elements of the domain).
		/// </summary>
		int Size { get; }

		/// <summary>
		/// Gets the index of the specified atom in the domanin.
		/// </summary>
		int MapFromDomain(TAtom atom);

		/// <summary>
		/// Gets the atome corresponding to the specified index.
		/// </summary>
		TAtom MapToDomain(int value);
	}
}
