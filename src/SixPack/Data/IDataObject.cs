// IDataObject.cs 
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
	/// Inferface to be implemented by Data Objects.
	/// </summary>
	public interface IDataObject
	{
		/// <summary>
		/// Loads the contents of the object by parsing a DataRow.
		/// </summary>
		/// <param name="dr">A DataRow object containing the values to be loaded by the object</param>
		void Load(DataRow dr);

		///// <summary>
		///// Serializes a DataObject to an XmlWriter.
		///// </summary>
		///// <param name="xmlWriter">The XmlWriter to write to.</param>
		//void Serialize(XmlWriter xmlWriter);

		///// <summary>
		///// Deserializes XmlReader stream to the current instance.
		///// </summary>
		///// <param name="xmlReader">The XmlReader to deserialize.</param>
		//void Deserialize(XmlReader xmlReader);
	}
}
