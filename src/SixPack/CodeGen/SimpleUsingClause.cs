// SimpleUsingCause.cs 
//
//  Copyright (C) 2009 Marco Cecconi
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
namespace SixPack.CodeGen
{
	/// <summary>
	/// A class that represents a using clause
	/// </summary>
	public class SimpleUsingClause: AbstractUsingClause
	{
		private readonly string namespaceName;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleUsingClause"/> class.
		/// </summary>
		/// <param name="namespaceName">Name of the namespace.</param>
		public SimpleUsingClause(string namespaceName)
		{
			this.namespaceName = namespaceName;
		}

		/// <summary>
		/// Gets the name of the name space.
		/// </summary>
		/// <value>The name of the name space.</value>
		public override string NamespaceName
		{
			get { return namespaceName; }
		}
	}
}