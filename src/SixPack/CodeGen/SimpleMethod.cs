// SimpleMethod.cs 
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
using System.Collections.Generic;

namespace SixPack.CodeGen
{
	/// <summary>
	/// Class that represents a Method
	/// </summary>
	public class SimpleMethod: AbstractMethod
	{
		private readonly List<string> body = new List<string>();
		private readonly SimpleVariable nameAndReturn;
		private readonly List<AbstractParameter> parameters = new List<AbstractParameter>();
		private readonly string prefix;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleMethod"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="prefix">The prefix.</param>
		/// <param name="type">The type.</param>
		public SimpleMethod(string name, string prefix, Type type)
		{
			this.prefix = prefix;
			nameAndReturn = new SimpleVariable(name, type, null);
		}

		/// <summary>
		/// Gets the body.
		/// </summary>
		/// <value>The body.</value>
		public override ICollection<string> Body
		{
			get { return body; }
		}

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public override ICollection<AbstractParameter> Parameters
		{
			get { return parameters; }
		}

		/// <summary>
		/// Gets the name and return.
		/// </summary>
		/// <value>The name and return.</value>
		public override AbstractVariable NameAndReturn
		{
			get { return nameAndReturn; }
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