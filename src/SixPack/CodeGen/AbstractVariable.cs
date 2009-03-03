// AbstractVariable.cs 
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
using System;

namespace SixPack.CodeGen
{
	/// <summary>
	/// A class that represents a variable (field, property, parameter)
	/// </summary>
	public abstract class AbstractVariable
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public abstract string Name { get; }
		/// <summary>
		/// Gets the type of the variable.
		/// </summary>
		/// <value>The type of the variable.</value>
		public abstract Type VariableType { get; }
		/// <summary>
		/// Gets the prefix.
		/// </summary>
		/// <value>The prefix.</value>
		public abstract string Prefix { get; }
	}
}