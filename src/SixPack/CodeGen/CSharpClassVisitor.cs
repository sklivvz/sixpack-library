// CSharpClassVisitor.cs 
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
using System.IO;

namespace SixPack.CodeGen
{
	/// <summary>
	/// A class visitor that renders a C# source code representation to a TextWriter
	/// </summary>
	public class CSharpClassVisitor: IClassVisitor
	{
		private readonly TextWriter writer;
		private int indentation;

		/// <summary>
		/// Initializes a new instance of the <see cref="CSharpClassVisitor"/> class.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public CSharpClassVisitor(TextWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			this.writer = writer;
		}

		#region IClassVisitor Members

		/// <summary>
		/// Visits the specified class.
		/// </summary>
		/// <param name="concreteClass">The class.</param>
		public void Visit(AbstractClass concreteClass)
		{
			if (concreteClass == null)
				throw new ArgumentNullException("concreteClass");
			foreach (AbstractUsingClause clause in concreteClass.UsingClauses)
			{
				clause.Accept(this);
			}
			writer.WriteLine();
			concreteClass.NamespaceDefinition.Accept(this);
			openBracket();
			indent();
			string postfix = string.IsNullOrEmpty(concreteClass.Postfix) ? string.Empty : " : " + concreteClass.Postfix;
			writer.WriteLine("{0} class {1}{2}", concreteClass.Prefix, concreteClass.Name, postfix);
			openBracket();
			foreach (AbstractField field in concreteClass.Fields)
			{
				field.Accept(this);
			}
			writer.WriteLine();
			foreach (AbstractProperty property in concreteClass.Properties)
			{
				property.Accept(this);
				writer.WriteLine();
			}
			foreach (AbstractConstructor constructor in concreteClass.Constructors)
			{
				constructor.Accept(this);
				writer.WriteLine();
			}
			foreach (AbstractMethod method in concreteClass.Methods)
			{
				method.Accept(this);
				writer.WriteLine();
			}
			closeBracket(); // class
			closeBracket(); // namespace
		}

		/// <summary>
		/// Visits the specified constructor.
		/// </summary>
		/// <param name="constructor">The constructor.</param>
		public void Visit(AbstractConstructor constructor)
		{
			if (constructor == null)
				throw new ArgumentNullException("constructor");
			indent();
			writer.Write("{0} {1} (", constructor.Prefix, constructor.NameAndReturn.Name);
			bool isFirst = true;
			foreach (AbstractParameter parameter in constructor.Parameters)
			{
				if (isFirst)
					isFirst = false;
				else
					writer.Write(", ");

				parameter.Accept(this);
			}
			string postfix = string.IsNullOrEmpty(constructor.Postfix) ? string.Empty : ": " + constructor.Postfix;
			writer.WriteLine("){0}", postfix);
			renderBody(constructor.Body);
		}

		/// <summary>
		/// Visits the specified method.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Visit(AbstractMethod method)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			indent();
			string returnName = method.NameAndReturn.VariableType == null ? "void" : method.NameAndReturn.VariableType.Name;
			writer.Write("{0} {1} {2} (", method.Prefix, returnName, method.NameAndReturn.Name);
			bool isFirst = true;
			foreach (AbstractParameter parameter in method.Parameters)
			{
				if (isFirst)
					isFirst = false;
				else
					writer.Write(", ");

				parameter.Accept(this);
			}
			writer.WriteLine(")");
			renderBody(method.Body);
		}

		/// <summary>
		/// Visits the specified field.
		/// </summary>
		/// <param name="field">The field.</param>
		public void Visit(AbstractField field)
		{
			if (field == null)
				throw new ArgumentNullException("field");

			indent();
			writer.WriteLine("{0} {1} {2};", field.Prefix, field.VariableType.Name, field.Name);
		}

		/// <summary>
		/// Visits the specified property.
		/// </summary>
		/// <param name="concreteProperty">The property.</param>
		public void Visit(AbstractProperty concreteProperty)
		{
			if (concreteProperty == null)
				throw new ArgumentNullException("concreteProperty");
			indent();
			writer.WriteLine("{0} {1} {2}", concreteProperty.Prefix, concreteProperty.VariableType.Name, concreteProperty.Name);
			openBracket();
			if (concreteProperty.Getter != null)
				concreteProperty.Getter.Accept(this);
			if (concreteProperty.Setter != null)
				concreteProperty.Setter.Accept(this);
			closeBracket();
		}

		/// <summary>
		/// Visits the specified parameter.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		public void Visit(AbstractParameter parameter)
		{
			if (parameter == null)
				throw new ArgumentNullException("parameter");
			if (string.IsNullOrEmpty(parameter.Prefix))
				writer.Write("{0} {1}", parameter.VariableType.Name, parameter.Name);
			else
				writer.Write("{0} {1} {2}", parameter.Prefix, parameter.VariableType.Name, parameter.Name);
		}

		/// <summary>
		/// Visits the specified clause.
		/// </summary>
		/// <param name="clause">The clause.</param>
		public void Visit(AbstractUsingClause clause)
		{
			if (clause == null)
				throw new ArgumentNullException("clause");
			indent();
			writer.WriteLine("using {0};", clause.NamespaceName);
		}

		/// <summary>
		/// Visits the specified name space.
		/// </summary>
		/// <param name="concreteNamespace">The name space.</param>
		public void Visit(AbstractNamespace concreteNamespace)
		{
			if (concreteNamespace == null)
				throw new ArgumentNullException("concreteNamespace");
			indent();
			writer.WriteLine("namespace {0}", concreteNamespace.Name);
		}

		/// <summary>
		/// Visits the specified getter.
		/// </summary>
		/// <param name="getter">The getter.</param>
		public void Visit(AbstractGetter getter)
		{
			if (getter == null)
				throw new ArgumentNullException("getter");
			indent();
			writer.WriteLine("get");
			renderBody(getter.Body);
		}

		/// <summary>
		/// Visits the specified setter.
		/// </summary>
		/// <param name="setter">The setter.</param>
		public void Visit(AbstractSetter setter)
		{
			if (setter == null)
				throw new ArgumentNullException("setter");
			indent();
			writer.WriteLine("set");
			renderBody(setter.Body);
		}

		#endregion

		private void renderBody(IEnumerable<string> body)
		{
			if (body == null)
				throw new ArgumentNullException("body");
			openBracket();
			foreach (string s in body)
			{
				indent();
				writer.WriteLine(s);
			}
			closeBracket();
		}

		private void openBracket()
		{
			indent();
			writer.WriteLine("{");
			indentation++;
		}

		private void closeBracket()
		{
			indentation--;
			indent();
			writer.WriteLine("}");
		}

		private void indent()
		{
			writer.Write(new string('\t', indentation));
		}
	}
}