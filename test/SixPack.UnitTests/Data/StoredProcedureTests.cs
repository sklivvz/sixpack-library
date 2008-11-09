// StoredProcedureTests.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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
using SixPack.Data;
using MbUnit.Framework;
using System.Data.Common;
using System.Configuration;
using SixPack.Security.Cryptography;
using System.Collections.Generic;

namespace SixPack.UnitTests.Data
{
	[TestFixture]
	public class StoredProcedureTests
	{
		private static DbConnection GetConnection()
		{
			ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[0];
			DbProviderFactory factory = DbProviderFactories.GetFactory(css.ProviderName);
			DbConnection connection = factory.CreateConnection();
			connection.ConnectionString = css.ConnectionString;
			return connection;
		}

		[Test]
		public void TestGetDataSet()
		{
			const int startId = 2;
			const int endId = 3;

			StoredProcedure sp = new StoredProcedure("GetNames", "default");
			sp.Command.AddParameter("@StartId", startId, DbType.Int32, 4);
			sp.Command.AddParameter("@EndId", endId, DbType.Int32, 4);
			DataSet ds = sp.GetDataSet();

			DataTable table = ds.Tables[0];
			Assert.AreEqual(endId - startId + 1, table.Rows.Count, "The table does not contain the correct number of rows");
			for (int i = 0; i < table.Rows.Count; ++i)
			{
				Assert.AreEqual(i + startId, table.Rows[i][0], "The row is incorrect");
				Assert.AreEqual(string.Format("name{0}", i + startId), table.Rows[i][1], "The row is incorrect");
			}
		}

		[Test]
		public void TestTransaction()
		{
			using (DbConnection connection = GetConnection())
			{
				connection.Open();

				DbTransaction transaction = connection.BeginTransaction();

				const string newName = "changed";

				StoredProcedure changeName = new StoredProcedure("ChangeName");
				changeName.Command.AddParameter("@Id", 2, DbType.Int32, 4);
				changeName.Command.AddParameter("@NewName", newName, DbType.String, 50);
				changeName.Execute(transaction);

				StoredProcedure getName = new StoredProcedure("GetName");
				getName.Command.AddParameter("@Id", 2, DbType.Int32, 4);
				Assert.AreEqual(newName, getName.ExecuteScalar(transaction), "The name has not been changed");

				transaction.Rollback();

				Assert.AreEqual("name2", getName.ExecuteScalar(), "The name has not been reverted");

				connection.Close();
			}
		}

		[Test]
		public void TestExistingClosedConnection()
		{
			using (DbConnection connection = GetConnection())
			{
				const int startId = 2;
				const int endId = 3;

				Assert.AreEqual(ConnectionState.Closed, connection.State, "The connection should be closed");

				StoredProcedure sp = new StoredProcedure("GetNames", "default");
				sp.Command.AddParameter("@StartId", startId, DbType.Int32, 4);
				sp.Command.AddParameter("@EndId", endId, DbType.Int32, 4);
				DataSet ds = sp.GetDataSet(connection);

				Assert.AreEqual(ConnectionState.Closed, connection.State, "The connection should stay closed");

				DataTable table = ds.Tables[0];
				Assert.AreEqual(endId - startId + 1, table.Rows.Count, "The table does not contain the correct number of rows");
			}
		}

		[Test]
		public void TestExistingOpenConnection()
		{
			using (DbConnection connection = GetConnection())
			{
				const int startId = 2;
				const int endId = 3;

				connection.Open();
				Assert.AreEqual(ConnectionState.Open, connection.State, "The connection should be open");

				StoredProcedure sp = new StoredProcedure("GetNames", "default");
				sp.Command.AddParameter("@StartId", startId, DbType.Int32, 4);
				sp.Command.AddParameter("@EndId", endId, DbType.Int32, 4);
				DataSet ds = sp.GetDataSet(connection);

				Assert.AreEqual(ConnectionState.Open, connection.State, "The connection should stay open");
				connection.Close();

				DataTable table = ds.Tables[0];
				Assert.AreEqual(endId - startId + 1, table.Rows.Count, "The table does not contain the correct number of rows");
			}
		}

		[Test]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void TestInexistantConnectionString()
		{
			string connectionStringName = DataGenerator.RandomAsciiString(10);
			StoredProcedure sp = new StoredProcedure("GetNames", connectionStringName);
		}

		[Test]
		public void TestOutputParameter()
		{
			StoredProcedure sp = new StoredProcedure("OutputParameter", "default");
			sp.Command.AddParameter("@First", 2, DbType.Int32, 4);
			sp.Command.AddParameter("@Second", 3, DbType.Int32, 4);
			sp.Command.AddOutParameter("@Result", DbType.Int32, 4);
			sp.Execute();

			int result = (int)sp.GetOutputParameter("@Result");
			Assert.AreEqual(5, result);
		}

		[Test]
		public void TestOutputParameterGeneric()
		{
			StoredProcedure sp = new StoredProcedure("OutputParameter", "default");
			sp.Command.AddParameter("@First", 2, DbType.Int32, 4);
			sp.Command.AddParameter("@Second", 3, DbType.Int32, 4);
			sp.Command.AddOutParameter("@Result", DbType.Int32, 4);

			int result;
			sp.Execute(out result);

			Assert.AreEqual(5, result);
		}
	}
}
