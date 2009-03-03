// IClassVisitor.cs 
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
	/// An interface that represents class visitors. 
	/// These classes can take an abstract class and 
	/// represent it in another form, like a source code file.
	/// </summary>
	public interface IClassVisitor
	{
		/// <summary>
		/// Visits the specified class.
		/// </summary>
		/// <param name="concreteClass">The class.</param>
		void Visit(AbstractClass concreteClass);

		/// <summary>
		/// Visits the specified constructor.
		/// </summary>
		/// <param name="constructor">The constructor.</param>
		void Visit(AbstractConstructor constructor);

		/// <summary>
		/// Visits the specified method.
		/// </summary>
		/// <param name="method">The method.</param>
		void Visit(AbstractMethod method);

		/// <summary>
		/// Visits the specified field.
		/// </summary>
		/// <param name="field">The field.</param>
		void Visit(AbstractField field);

		/// <summary>
		/// Visits the specified property.
		/// </summary>
		/// <param name="concreteProperty">The property.</param>
		void Visit(AbstractProperty concreteProperty);

		/// <summary>
		/// Visits the specified parameter.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		void Visit(AbstractParameter parameter);

		/// <summary>
		/// Visits the specified clause.
		/// </summary>
		/// <param name="clause">The clause.</param>
		void Visit(AbstractUsingClause clause);

		/// <summary>
		/// Visits the specified name space.
		/// </summary>
		/// <param name="concreteNamespace">The name space.</param>
		void Visit(AbstractNamespace concreteNamespace);

		/// <summary>
		/// Visits the specified getter.
		/// </summary>
		/// <param name="getter">The getter.</param>
		void Visit(AbstractGetter getter);

		/// <summary>
		/// Visits the specified setter.
		/// </summary>
		/// <param name="setter">The setter.</param>
		void Visit(AbstractSetter setter);
	}
}