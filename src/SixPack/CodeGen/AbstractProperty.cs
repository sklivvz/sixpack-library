// AbstractProperty.cs 
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
	/// An abstract class that represents a property
	/// </summary>
	public abstract class AbstractProperty: AbstractVariable, IClassElement
	{
		/// <summary>
		/// Gets the getter.
		/// </summary>
		/// <value>The getter.</value>
		public abstract AbstractGetter Getter { get; }

		/// <summary>
		/// Gets the setter.
		/// </summary>
		/// <value>The setter.</value>
		public abstract AbstractSetter Setter { get; }

		#region IClassElement Members

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(IClassVisitor visitor)
		{
			if (visitor != null)
				visitor.Visit(this);
		}

		#endregion
	}
}