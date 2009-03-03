// DataTableClass.cs 
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
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;
using SixPack.Text;

namespace SixPack.CodeGen
{
	/// <summary>
	/// A class that creates a data transfer object representation from a datatable.
	/// </summary>
	// the class should remain sealed because it is using virtual members in the constructor!
	public sealed class DataTableClass: SimpleClass
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableClass"/> class.
		/// </summary>
		/// <param name="dataTable">The data table.</param>
		/// <param name="namespace">The @namespace.</param>
		/// <param name="prefix">The prefix.</param>
		public DataTableClass(DataTable dataTable, AbstractNamespace @namespace, string prefix)
		{
			if (string.IsNullOrEmpty(dataTable.TableName))
				throw new ArgumentNullException("dataTable", "The data table must have a TableName.");

			Name = TextUtilities.NormalizeForCode(dataTable.TableName, TextNormalizationType.Class);
			NamespaceDefinition = @namespace;
			Postfix = "ISerializable";
			Prefix = "[Serializable]\n\t" + prefix;
			UsingClauses.Add(new SimpleUsingClause("System"));
			UsingClauses.Add(new SimpleUsingClause("System.Data"));
			UsingClauses.Add(new SimpleUsingClause("System.Runtime.Serialization"));
			UsingClauses.Add(new SimpleUsingClause("System.Security.Permissions"));

			// ISerializable pattern
			SimpleConstructor serializationConstructor = new SimpleConstructor(Name, "protected", null);
			serializationConstructor.Parameters.Add(new SimpleParameter("information", typeof (SerializationInfo), null));
			serializationConstructor.Parameters.Add(new SimpleParameter("context", typeof (StreamingContext), null));

			SimpleMethod serializationMethod = new SimpleMethod("GetObjectData",
			                                                    "[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]\n\t\tpublic",
			                                                    null);
			serializationMethod.Parameters.Add(new SimpleParameter("info", typeof (SerializationInfo), null));
			serializationMethod.Parameters.Add(new SimpleParameter("context", typeof (StreamingContext), null));

			// datarow constructor
			SimpleConstructor datarowConstructor = new SimpleConstructor(Name, "public", null);
			datarowConstructor.Parameters.Add(new SimpleParameter("dataRow", typeof (DataRow), null));

			foreach (DataColumn column in dataTable.Columns)
			{
				string fieldName = TextUtilities.NormalizeForCode(column.ColumnName, TextNormalizationType.Field);
				string typeName = column.DataType.Name;

				Fields.Add(new SimpleField(fieldName, column.DataType, "private readonly"));

				SimpleGetter getter = new SimpleGetter();
				getter.Body.Add(string.Format(CultureInfo.InvariantCulture, "return {0};", fieldName));

				Properties.Add(new SimpleProperty(
				               	TextUtilities.NormalizeForCode(column.ColumnName, TextNormalizationType.Property), column.DataType,
				               	"public", getter, null));

				serializationConstructor.Body.Add(string.Format(CultureInfo.InvariantCulture,
				                                                "{0} = ({1}) information.GetValue(\"{0}\", typeof ({1}));",
				                                                fieldName, typeName));

				serializationMethod.Body.Add(string.Format(CultureInfo.InvariantCulture, "info.AddValue(\"{0}\", {1});", fieldName,
				                                           fieldName));

				datarowConstructor.Body.Add(string.Format(CultureInfo.InvariantCulture, "{0} = ({1})dataRow[\"{2}\"];", fieldName,
				                                          typeName, column.ColumnName));
			}

			Constructors.Add(serializationConstructor);
			Methods.Add(serializationMethod);

			Constructors.Add(datarowConstructor);
		}
	}
}