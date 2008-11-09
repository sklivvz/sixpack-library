// CommandWrapper.cs 
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

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace SixPack.Data
{
	/// <summary>
	/// Fa�ade for SqlCommand
	/// </summary>
	public class CommandWrapper : IDisposable
	{
		private readonly DbProviderFactory providerFactory;
		internal DbCommand command;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandWrapper"/> class.
		/// </summary>
		/// <param name="dbCommand">The db command.</param>
		/// <param name="dbProviderFactory">The db provider factory.</param>
		public CommandWrapper(DbCommand dbCommand, DbProviderFactory dbProviderFactory)
		{
			command = dbCommand;
			providerFactory = dbProviderFactory;
		}

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Gets the value of the specified output parameter.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns></returns>
		internal object GetOutputParameter(string name)
		{
			DbParameter parameter = command.Parameters[name];
			if(parameter == null)
			{
				throw new ArgumentException("The specified parameter does not exist.", "name");
			}
			if(parameter.Direction != ParameterDirection.Output)
			{
				throw new ArgumentException("The specified parameter is not an output parameter.", "name");
			}
			return parameter.Value;
		}

		/// <summary>
		/// Gets the value of the output parameters.
		/// </summary>
		/// <returns></returns>
		internal object[] GetOutputParameters(int expectedCount)
		{
			List<object> outputParameters = new List<object>();
			foreach (DbParameter parameter in command.Parameters)
			{
				if(parameter.Direction == ParameterDirection.Output)
				{
					outputParameters.Add(parameter.Value);
				}
			}

			if (outputParameters.Count != expectedCount)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The number of output parameters ({0}) is not what has been requested ({1})", outputParameters.Count, expectedCount), "expectedCount");
			}

			return outputParameters.ToArray();
		}

		/// <summary>
		/// Adds an input parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="parType">Type of the par.</param>
		/// <param name="size">The size.</param>
		public void AddParameter(string name, object value, DbType parType, int size)
		{
			AddParameter(name, value, parType, size, ParameterDirection.Input);
		}

		/// <summary>
		/// Adds an output parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="parType">Type of the par.</param>
		/// <param name="size">The size.</param>
		public void AddOutParameter(string name, DbType parType, int size)
		{
			AddParameter(name, null, parType, size, ParameterDirection.Output);
		}

		/// <summary>
		/// Adds a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="parType">Type of the par.</param>
		/// <param name="size">The size.</param>
		/// <param name="direction">The direction.</param>
		public void AddParameter(string name, object value, DbType parType, int size, ParameterDirection direction)
		{
			DbParameter p = providerFactory.CreateParameter();
			p.ParameterName = name;

			////Automatic serialization support
			//if (parType==DbType.Object && value is IDataCollection)
			//{
			//    parType = DbType.String;
			//    StringWriter sw = new StringWriter();
			//    XmlTextWriter xtw = new XmlTextWriter(sw);
			//    ((IDataCollection)value).Serialize(xtw);
			//    value = sw.ToString();
			//}
			////End of automatic serialization support
			p.DbType = parType;
			if (
				parType == DbType.String ||
				parType == DbType.AnsiString ||
				parType == DbType.StringFixedLength ||
				parType == DbType.AnsiStringFixedLength
				)
				p.Size = size;

			if (value == null)
				p.Value = DBNull.Value;
			else if (parType == DbType.Guid && value is Guid && (Guid) value == Guid.Empty)
				p.Value = DBNull.Value;
			else
				p.Value = value;

			p.Direction = direction;

			command.Parameters.Add(p);
			//Console.WriteLine("{0}:\t{1}({2})", p.ParameterName, p.SqlDbType.ToString(), p.Size);
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="CommandWrapper"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="CommandWrapper"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("EXEC {0} ", command.CommandText);
			bool isFirst = true;
			foreach (DbParameter p in command.Parameters)
			{
				if (p.Value != DBNull.Value)
				{
					if (isFirst)
					{
						isFirst = false;
						sb.AppendFormat("{0}=", p.ParameterName);
					}
					else
						sb.AppendFormat(", {0}=", p.ParameterName);

					switch (p.DbType)
					{
						case DbType.Byte:
						case DbType.Currency:
						case DbType.Decimal:
						case DbType.Double:
						case DbType.Int16:
						case DbType.Int32:
						case DbType.Int64:
						case DbType.SByte:
						case DbType.Single:
						case DbType.UInt16:
						case DbType.UInt32:
						case DbType.UInt64:
							sb.AppendFormat("{0}", p.Value);
							break;
						case DbType.Boolean:
							sb.Append((bool) p.Value ? 1 : 0);
							break;
						case DbType.AnsiString:
						case DbType.DateTime:
						case DbType.Date:
						case DbType.Time:
						case DbType.Guid:
						case DbType.AnsiStringFixedLength:
							sb.AppendFormat("'{0}'", p.Value);
							break;
						case DbType.String:
						case DbType.StringFixedLength:
						case DbType.Xml:
							sb.AppendFormat("N'{0}'", p.Value);
							break;
						default:
							sb.AppendFormat("'{0}' /* unsupported */", p.Value);
							break;
					}
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
				command.Dispose();
			}
			// free native resources
		}
	}
}
