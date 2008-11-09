// SortedSetOfT.cs 
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

using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
    /// <summary>
    /// Collection that contains sorted unique items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortedSet<T> : SetBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortedSet&lt;T&gt;"/> class.
        /// </summary>
        public SortedSet()
            : base(new SortedDictionary<T, object>())
        {
            // Nothing to be done 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedSet&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public SortedSet(IComparer<T> comparer)
            : base(new SortedDictionary<T, object>(comparer))
        {
            // Nothing to be done 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedSet&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="elements">The elements to add to the set.</param>
        public SortedSet(IEnumerable<T> elements)
            : this()
        {
            AddRange(elements);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="elements">The elements to add to the set.</param>
        public SortedSet(params T[] elements)
            : this((IEnumerable<T>) elements)
        {
            // Nothing to be done 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        /// <param name="elements">The elements to add to the set.</param>
        public SortedSet(IComparer<T> comparer, IEnumerable<T> elements)
            : this(comparer)
        {
            AddRange(elements);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        /// <param name="elements">The elements to add to the set.</param>
        public SortedSet(IComparer<T> comparer, params T[] elements)
            : this(comparer, (IEnumerable<T>) elements)
        {
            // Nothing to be done 
        }
    }
}
