// SimpleParameter.cs 
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
	/// Class that represents a paramter
	/// </summary>
	public class SimpleParameter: AbstractParameter
	{
		private readonly string name;
		private readonly string prefix;
		private readonly Type variableType;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="prefix">The prefix.</param>
		public SimpleParameter(string name, Type type, string prefix)
		{
			this.name = name;
			this.prefix = prefix;
			variableType = type;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public override string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the type of the variable.
		/// </summary>
		/// <value>The type of the variable.</value>
		public override Type VariableType
		{
			get { return variableType; }
		}

		/// <summary>
		/// Gets the prefix.
		/// </summary>
		/// <value>The prefix.</value>
		public override string Prefix
		{
			get { return prefix; }
		}
	}
}