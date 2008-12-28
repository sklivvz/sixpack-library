// DataCollection.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
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
using System.Data;

namespace SixPack.Data
{
	/// <summary>
	/// Collection of Data Transfer Objects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public abstract class DataCollection<T> : List<T>, IDataLoader where T : IDataObject, new()
	{
		#region IDataLoader Members

		/// <summary>
		/// Loads a DataCollection by parsing a DataTable
		/// It does not remove current items.
		/// </summary>
		/// <param name="dataTable">The data table.</param>
		public void Load(DataTable dataTable)
		{
			if (dataTable != null)
			{
				if (dataTable.Rows != null)
				{
					foreach (DataRow dr in dataTable.Rows)
					{
						T i = new T();
						i.Load(dr);
						Add(i);
					}
				}
			}
		}

		/// <summary>
		/// Loads a DataCollection by parsing a DataRow[]
		/// It does not remove current items.
		/// </summary>
		/// <param name="dataRowArray">The data row array.</param>
		public void Load(DataRow[] dataRowArray)
		{
			foreach (DataRow dr in dataRowArray)
			{
				T i = new T();
				i.Load(dr);
				Add(i);
			}
		}

		#endregion

		///// <summary>
		///// Serializes a DataCollection to an XmlWriter.
		///// </summary>
		///// <param name="xmlWriter">The XmlWriter to write to.</param>
		//public abstract void Serialize(XmlWriter xmlWriter);

		///// <summary>
		///// Deserializes an XmlReader to the current collection.
		///// It does not remove current items.
		///// </summary>
		///// <param name="xmlReader">The XmlReader to deserialize.</param>
		//public abstract void Deserialize(XmlReader xmlReader);
	}
}
