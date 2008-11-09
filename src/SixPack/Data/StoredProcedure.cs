// StoredProcedure.cs 
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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using SixPack.Diagnostics;
using System.Collections.Generic;

namespace SixPack.Data
{
	/// <summary>
	/// This class is used to wrap and execute stored procedures.
	/// </summary>
	public class StoredProcedure : IDisposable
	{
		private readonly ConnectionInfo connectionInfo;
		private readonly CommandWrapper commandWrapper;
#if (DEBUG)
		private readonly HighResolutionTimer timer;
#endif

		#region Deadlock retries
		private static readonly int deadlockRetries = InitializeDeadlockRetries();

		private static int InitializeDeadlockRetries()
		{
			string dlr = ConfigurationManager.AppSettings["StoredProcedureDeadlockRetries"];

			if (string.IsNullOrEmpty(dlr))
			{
				Log.Instance.Add("Stored Procedure Deadlock Retries value not set. Assuming 0.", LogLevel.Warning);
				Log.Instance.Add("To suppress this warning, add this to your appSettings: <add key=\"StoredProcedureDeadlockRetries\" value=\"0\" />.", LogLevel.Warning);
			}
			else
			{
				try
				{
					return int.Parse(dlr, CultureInfo.InvariantCulture);
				}
				catch (FormatException fex)
				{
					Log.Instance.AddFormat("Invalid configuration value for StoredProcedureDeadlockRetries: '{0}'. Assuming 0.", dlr, LogLevel.Warning);
					Log.Instance.HandleException(fex, LogLevel.Warning);
				}
			}
			return 0;
		}
		#endregion

		#region Connection string caching
		#region ConnectionInfo
		/// <summary>
		/// Contains a connection string and a <see cref="DbProviderFactory"/>.
		/// </summary>
		private class ConnectionInfo
		{
			private readonly string connectionString;

			/// <summary>
			/// Gets the connection string.
			/// </summary>
			/// <value>The connection string.</value>
			public string ConnectionString
			{
				get
				{
					return connectionString;
				}
			}

			private readonly DbProviderFactory providerFactory;

			/// <summary>
			/// Gets the provider factory.
			/// </summary>
			/// <value>The provider factory.</value>
			public DbProviderFactory ProviderFactory
			{
				get
				{
					return providerFactory;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ConnectionInfo"/> class.
			/// </summary>
			/// <param name="css">The CSS.</param>
			public ConnectionInfo(ConnectionStringSettings css)
			{
				connectionString = css.ConnectionString;
				providerFactory = DbProviderFactories.GetFactory(css.ProviderName);
#if DEBUG
				Log.Instance.AddFormat("Loaded providerFactory {0}", providerFactory.GetType());
#endif
			}
		}
		#endregion

		private static readonly ConnectionInfo defaultConnectionString = InitializeDefaultConnectionString();
		private static Dictionary<string, ConnectionInfo> cachedConnectionStrings;

		/// <summary>
		/// Initializes the default connection string.
		/// </summary>
		/// <returns></returns>
		private static ConnectionInfo InitializeDefaultConnectionString()
		{
			cachedConnectionStrings = new Dictionary<string, ConnectionInfo>();

			if (ConfigurationManager.ConnectionStrings.Count == 0)
			{
				Log.Instance.Add("No connection string has been defined", LogLevel.Warning);
				return null;
			}
			else
			{
				try
				{
					ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[0];
					ConnectionInfo cachedItem = new ConnectionInfo(css);
					cachedConnectionStrings.Add(css.Name, cachedItem);
					return cachedItem;
				}
				catch (Exception err)
				{
					Log.Instance.HandleException(err);
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the connection string with the specified name from the cache.
		/// </summary>
		/// <param name="name">The name of the connection string.</param>
		/// <returns></returns>
		private static ConnectionInfo GetConnectionString(string name)
		{
			ConnectionInfo item;
			if (!cachedConnectionStrings.TryGetValue(name, out item))
			{
				lock (cachedConnectionStrings)
				{
					if (!cachedConnectionStrings.TryGetValue(name, out item))
					{
						ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
						if(css == null)
						{
							throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "There is no connection string named '{0}' in the configuration file", name));
						}
						item = new ConnectionInfo(css);
						cachedConnectionStrings[name] = item;
					}
				}
			}
			return item;
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="StoredProcedure"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="connectionInfo">The cached connection string information.</param>
		private StoredProcedure(string name, ConnectionInfo connectionInfo)
		{
			this.connectionInfo = connectionInfo;

			DbCommand dbCommand = connectionInfo.ProviderFactory.CreateCommand();
			dbCommand.CommandText = name;
			dbCommand.CommandType = CommandType.StoredProcedure;
			commandWrapper = new CommandWrapper(dbCommand, connectionInfo.ProviderFactory);

#if (DEBUG)
			timer = new HighResolutionTimer();
			// Build the timer
			timer.Start();
			timer.Stop();
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StoredProcedure"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="connectionStringName">Name of the connection string.</param>
		public StoredProcedure(string name, string connectionStringName)
			: this(name, GetConnectionString(connectionStringName))
		{
			// Nothing to be done
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StoredProcedure"/> class using the first connection string as connection.
		/// </summary>
		/// <param name="name">The name.</param>
		public StoredProcedure(string name)
			: this(name, defaultConnectionString)
		{
			// Nothing to be done
		}

		/// <summary>
		/// Gets the command.
		/// </summary>
		/// <value>The command.</value>
		public CommandWrapper Command
		{
			get
			{
				return commandWrapper;
			}
		}

		/// <summary>
		/// Gets the value of the specified output parameter.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns></returns>
		public object GetOutputParameter(string name)
		{
			return commandWrapper.GetOutputParameter(name);
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

		#region Common execute code
		/// <summary>
		/// Creates the database connection.
		/// </summary>
		/// <returns></returns>
		private DbConnection createDbConnection()
		{
			DbConnection ret = connectionInfo.ProviderFactory.CreateConnection();
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Created DbConnection of type \"{0}\"", ret.GetType(), LogLevel.Debug);
#endif
			ret.ConnectionString = connectionInfo.ConnectionString;
			return ret;
		}

		/// <summary>
		/// Creates the db data adapter.
		/// </summary>
		/// <returns></returns>
		private DbDataAdapter createDbDataAdapter()
		{
			DbDataAdapter ret = connectionInfo.ProviderFactory.CreateDataAdapter();
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Created DbDataAdapter of type \"{0}\"", ret.GetType(), LogLevel.Debug);
#endif
			ret.SelectCommand = commandWrapper.command;
			return ret;
		}

		private delegate T ExecuteDelegate<T>();

		/// <summary>
		/// Executes the stoted procedure.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="transaction">The transaction to use.</param>
		/// <param name="executeMethod">The method that actually executes the stored procedure.</param>
		/// <returns></returns>
		private T Execute<T>(DbConnection connection, DbTransaction transaction, ExecuteDelegate<T> executeMethod)
		{
			int numAttempts = 0;
			do
			{
				try
				{
					++numAttempts;
#if (DEBUG)
					timer.Start();
#endif
					T result;
					if (connection != null)
					{
						result = ExecuteInternal(connection, executeMethod);
					}
					else if (transaction != null)
					{
						result = ExecuteInternal(transaction, executeMethod);
					}
					else
					{
						result = ExecuteInternal(executeMethod);
					}
#if (DEBUG)
					timer.Stop();
					Log.Instance.AddFormat(
						"[StoredProcedure] Elapsed time for \"{0}\": {1:0} microseconds",
						commandWrapper.command.CommandText,
						timer.Duration(1000),
						LogLevel.Debug
					);
#endif
					return result;
				}
				catch (SqlException ex)
				{
					if (ex.Number != 1205)
						throw;
				}
				/* 
				 * Add extra provider exception handling here (Oracle, MySql, etc..).
				 * 
				 * Eg:
				 * catch(OracleException oracleEx)
				 * {
				 *     // Handle deadlock
				 * }
				 * */
				Log.Instance.AddFormat(
					"[StoredProcedure] Deadlock executing {0} (Execute). Attempt {1} of {2}.",
					Command.command.CommandText,
					numAttempts,
					deadlockRetries,
					LogLevel.Warning
				);
			} while (numAttempts <= deadlockRetries);

			// This code is never reached
			throw new InvalidOperationException("This code should never be executed");
		}

		/// <summary>
		/// Executes the stored procedure.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="executeMethod">The method that actually executes the stored procedure.</param>
		/// <returns></returns>
		private T ExecuteInternal<T>(ExecuteDelegate<T> executeMethod)
		{
			using (DbConnection conn = createDbConnection())
			{
				commandWrapper.command.Connection = conn;
				commandWrapper.command.Connection.Open();
				commandWrapper.command.Transaction = null;
				T result = executeMethod();
				conn.Close();
				return result;
			}
		}

		/// <summary>
		/// Executes the stored procedure.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="transaction">The transaction to use.</param>
		/// <param name="executeMethod">The method that actually executes the stored procedure.</param>
		/// <returns></returns>
		private T ExecuteInternal<T>(DbTransaction transaction, ExecuteDelegate<T> executeMethod)
		{
			commandWrapper.command.Connection = transaction.Connection;
			commandWrapper.command.Transaction = transaction;
			return executeMethod();
		}

		/// <summary>
		/// Executes the stored procedure.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="executeMethod">The method that actually executes the stored procedure.</param>
		/// <returns></returns>
		private T ExecuteInternal<T>(DbConnection connection, ExecuteDelegate<T> executeMethod)
		{
			ConnectionState previousState = connection.State;
			commandWrapper.command.Connection = connection;
			try
			{
				switch (previousState)
				{
					case ConnectionState.Closed:
						connection.Open();
						break;

					case ConnectionState.Open:
						// Nothing to be done
						break;

					default:
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The connection is in an invalid state: {0}", previousState));
				}

				return executeMethod();
			}
			finally
			{
				if (previousState == ConnectionState.Closed)
				{
					connection.Close();
				}
			}
		}
		#endregion

		#region Execute
		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute()
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute() on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return (int)Execute<object>(null, null, InternalExecute);
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute(DbTransaction transaction)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return (int)Execute<object>(null, transaction, InternalExecute);
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="connection">The connection to be used.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute(DbConnection connection)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return (int)Execute<object>(connection, null, InternalExecute);
		}

		/// <summary>
		/// Executes this instance.
		/// </summary>
		private object InternalExecute()
		{
			return commandWrapper.command.ExecuteNonQuery();
		}
		#endregion

		#region Execute with 1 output parameter
		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TParameter">The type of the param.</typeparam>
		/// <param name="parameter">The output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TParameter>(out TParameter parameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(out) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(1);
			parameter = (TParameter)outParameters[0];
			
			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TParameter">The type of the param.</typeparam>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <param name="parameter">The output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TParameter>(DbTransaction transaction, out TParameter parameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, transaction, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(1);
			parameter = (TParameter)outParameters[0];

			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TParameter">The type of the param.</typeparam>
		/// <param name="connection">The connection to be used.</param>
		/// <param name="parameter">The output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TParameter>(DbConnection connection, out TParameter parameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(connection, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(1);
			parameter = (TParameter)outParameters[0];

			return result;
		}
		#endregion

		#region Execute with 2 output parameters
		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter>(out TFirstParameter firstParameter, out TSecondParameter secondParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(out) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(2);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];

			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter>(DbTransaction transaction, out TFirstParameter firstParameter, out TSecondParameter secondParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, transaction, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(2);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];

			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <param name="connection">The connection to be used.</param>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter>(DbConnection connection, out TFirstParameter firstParameter, out TSecondParameter secondParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(connection, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(2);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];

			return result;
		}
		#endregion

		#region Execute with 3 output parameters
		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <typeparam name="TThirdParameter">The type of the third param.</typeparam>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <param name="thirdParameter">The third output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter, TThirdParameter>(out TFirstParameter firstParameter, out TSecondParameter secondParameter, out TThirdParameter thirdParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(out) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(3);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];
			thirdParameter = (TThirdParameter)outParameters[2];

			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <typeparam name="TThirdParameter">The type of the third param.</typeparam>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <param name="thirdParameter">The third output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter, TThirdParameter>(DbTransaction transaction, out TFirstParameter firstParameter, out TSecondParameter secondParameter, out TThirdParameter thirdParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(null, transaction, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(3);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];
			thirdParameter = (TThirdParameter)outParameters[2];

			return result;
		}

		/// <summary>
		/// Executes this instance, by calling <see cref="InternalExecute"/>.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <typeparam name="TFirstParameter">The type of the first param.</typeparam>
		/// <typeparam name="TSecondParameter">The type of the second param.</typeparam>
		/// <typeparam name="TThirdParameter">The type of the third param.</typeparam>
		/// <param name="connection">The connection to be used.</param>
		/// <param name="firstParameter">The first output parameter of the stored procedure.</param>
		/// <param name="secondParameter">The second output parameter of the stored procedure.</param>
		/// <param name="thirdParameter">The third output parameter of the stored procedure.</param>
		/// <returns>Returns the number of rows affected.</returns>
		public int Execute<TFirstParameter, TSecondParameter, TThirdParameter>(DbConnection connection, out TFirstParameter firstParameter, out TSecondParameter secondParameter, out TThirdParameter thirdParameter)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] Execute(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			int result = (int)Execute<object>(connection, null, InternalExecute);

			object[] outParameters = commandWrapper.GetOutputParameters(3);
			firstParameter = (TFirstParameter)outParameters[0];
			secondParameter = (TSecondParameter)outParameters[1];
			thirdParameter = (TThirdParameter)outParameters[2];

			return result;
		}
		#endregion

		#region ExecuteScalar
		/// <summary>
		/// Executes this instance returning a scalar, by calling <see cref="InternalExecuteScalar" />.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <returns></returns>
		public object ExecuteScalar()
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] ExecuteScalar() on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<object>(null, null, InternalExecuteScalar);
		}

		/// <summary>
		/// Executes this instance returning a scalar, by calling <see cref="InternalExecuteScalar" />.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <returns></returns>
		public object ExecuteScalar(DbTransaction transaction)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] ExecuteScalar(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<object>(null, transaction, InternalExecuteScalar);
		}

		/// <summary>
		/// Executes this instance returning a scalar, by calling <see cref="InternalExecuteScalar" />.
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="connection">The connection to be used.</param>
		/// <returns></returns>
		public object ExecuteScalar(DbConnection connection)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] ExecuteScalar(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<object>(connection, null, InternalExecuteScalar);
		}

		/// <summary>
		/// Executes this instance returning a scalar.
		/// </summary>
		/// <returns></returns>
		protected object InternalExecuteScalar()
		{
			return commandWrapper.command.ExecuteScalar();
		}
		#endregion

		#region GetDataSet
		/// <summary>
		/// Executes the stored procedure and returns the results as a DataSet, by calling <see cref="InternalGetDataSet" />..
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <returns></returns>
		public DataSet GetDataSet()
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] GetDataSet() on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<DataSet>(null, null, InternalGetDataSet);
		}

		/// <summary>
		/// Executes the stored procedure and returns the results as a DataSet, by calling <see cref="InternalGetDataSet" />..
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="transaction">The transaction on which the stored procedure should be executed.</param>
		/// <returns></returns>
		public DataSet GetDataSet(DbTransaction transaction)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] GetDataSet(DbTransaction) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<DataSet>(null, transaction, InternalGetDataSet);
		}

		/// <summary>
		/// Executes the stored procedure and returns the results as a DataSet, by calling <see cref="InternalGetDataSet" />..
		/// Handles deadlock retries for Sql Server connections.
		/// </summary>
		/// <param name="connection">The connection to be used.</param>
		/// <returns></returns>
		public DataSet GetDataSet(DbConnection connection)
		{
#if (DEBUG)
			Log.Instance.AddFormat("[StoredProcedure] GetDataSet(DbConnection) on \"{0}\"", commandWrapper, LogLevel.Debug);
#endif
			return Execute<DataSet>(connection, null, InternalGetDataSet);
		}

		/// <summary>
		/// Executes the stored procedure and returns the results as a DataSet.
		/// </summary>
		/// <returns></returns>
		protected DataSet InternalGetDataSet()
		{
			DataSet result = new DataSet();
			result.Locale = CultureInfo.InvariantCulture;

			DbDataAdapter da = createDbDataAdapter();
			da.Fill(result);

			return result;
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
				commandWrapper.Dispose();
			}
			// free native resources
		}
		#endregion
	}
}
