// IDataLoader.cs 
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

using System.Data;

namespace SixPack.Data
{
	/// <summary>
	/// Collection of Data Transfer Objects
	/// </summary>
	public interface IDataLoader
	{
		/// <summary>
		/// Loads a DataCollection by parsing a DataTable
		/// It does not remove current items.
		/// </summary>
		/// <param name="dataTable">The data table.</param>
		void Load(DataTable dataTable);

		/// <summary>
		/// Loads a DataCollection by parsing a DataRow[]
		/// It does not remove current items.
		/// </summary>
		/// <param name="dataRowArray">The data row array.</param>
		void Load(DataRow[] dataRowArray);

		///// <summary>
		///// Serializes a DataCollection to an XmlWriter.
		///// </summary>
		///// <param name="xmlWriter">The XmlWriter to write to.</param>
		//void Serialize(XmlWriter xmlWriter);

		///// <summary>
		///// Deserializes an XmlReader to the current collection.
		///// It does not remove current items.
		///// </summary>
		///// <param name="xmlReader">The XmlReader to deserialize.</param>
		//void Deserialize(XmlReader xmlReader);
	}
}
