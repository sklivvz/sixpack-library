// IFullListOfT.cs 
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

// IFullListOfT.cs created with MonoDevelop
// User: marco at 12:39 PM 10/30/2008
//

using System;
using System.Collections.Generic;

namespace SixPack.Collections.Generic
{
    /// <summary>
    /// Represents a collection of objects that can be individually accessed by index or by query.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IFullList<T> : IList<T>
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="IFullList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="IFullList&lt;T&gt;"/>. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        void AddRange(IEnumerable<T> collection);

        /// <summary>
        /// Sorts the elements in the entire <see cref="IFullList&lt;T&gt;"/> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison&lt;T&gt;"/> to use when comparing elements.</param>
        void Sort(Comparison<T> comparison);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified <see cref="Predicate&lt;T&gt;"/>, and returns the first occurrence within the entire <see cref="IFullList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate&lt;T&gt;"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified <see cref="Predicate&lt;T&gt;"/>, if found; otherwise, the default value for type T.</returns>
        T Find(Predicate<T> match);

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified <see cref="Predicate&lt;T&gt;"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate&lt;T&gt;"/> delegate that defines the conditions of the elements to search for.</param>
        /// <returns>A <see cref="IFullList&lt;T&gt;"/> containing all the elements that match the conditions defined by the specified <see cref="Predicate&lt;T&gt;"/>, if found; otherwise, an empty <see cref="IFullList&lt;T&gt;"/>.</returns>
        IFullList<T> FindAll(Predicate<T> match);

        /// <summary>
        /// Determines whether every element in the <see cref="IFullList&lt;T&gt;"/> matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate&lt;T&gt;"/> delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if every element in the <see cref="IFullList&lt;T&gt;"/> matches the conditions defined by the specified predicate; otherwise, false. If the list has no elements, the return value is true.</returns>
        bool TrueForAll(Predicate<T> match);

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IFullList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action&lt;T&gt;"/> delegate to perform on each element of the <see cref="IFullList&lt;T&gt;"/>.</param>
        void ForEach(Action<T> action);

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="IFullList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="index">The zero-based <see cref="IFullList&lt;T&gt;"/> index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the source <see cref="IFullList&lt;T&gt;"/>.</returns>
        IFullList<T> GetRange(int index, int count);
    }
}
