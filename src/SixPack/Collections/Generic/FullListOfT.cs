// FullListOfT.cs 
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

// SelectableOfT.cs created with MonoDevelop
// User: marco at 12:05 PM 10/30/2008
//

using System;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists. Implements <see cref="IFullList&lt;T&gt;"/>
    /// </summary>
    public class FullList<T> : List<T>, IFullList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullList&lt;T&gt;"/> class.
        /// </summary>
        public FullList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="collection"/> is null.</exception>
        public FullList(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public FullList(int capacity) : base(capacity)
        {
        }

        #region IFullList<T> Members

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="IFullList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="index">The zero-based <see cref="IFullList&lt;T&gt;"/> index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>
        /// A shallow copy of a range of elements in the source <see cref="IFullList&lt;T&gt;"/>.
        /// </returns>
        public new IFullList<T> GetRange(int index, int count)
        {
            return new FullList<T>(base.GetRange(index, count));
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate&lt;T&gt;"/> delegate that defines the conditions of the elements to search for.</param>
        /// <returns>
        /// A <see cref="IFullList&lt;T&gt;"/> containing all the elements that match the conditions defined by the specified <see cref="Predicate&lt;T&gt;"/>, if found; otherwise, an empty <see cref="IFullList&lt;T&gt;"/>.
        /// </returns>
        public new IFullList<T> FindAll(Predicate<T> match)
        {
            return new FullList<T>(base.FindAll(match));
        }

        #endregion
    }
}
