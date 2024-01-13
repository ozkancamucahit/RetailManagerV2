using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace TRMDataManager.Library.Internal.DataAccess
{
	public sealed class SQLDataAccess : IDisposable, ISQLDataAccess
	{

		#region FIELDS
		private IDbConnection _dbConnection;
		private IDbTransaction _dbTransaction;
		private readonly IConfiguration configuration;

		#endregion

		#region CTOR
		public SQLDataAccess(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		#endregion

		public string GetConnectionString(string name)
		{
			try
			{
				return configuration.GetConnectionString(name);
				//return ConfigurationManager.ConnectionStrings[name].ConnectionString ?? String.Empty;
			}
			catch
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="storedProcedure"></param>
		/// <param name="parameters"></param>
		/// <param name="connectionStringName"></param>
		/// <returns>IEnumerable<T> => Liste(t)</returns>
		public IEnumerable<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
		{
			string cnnString = GetConnectionString(connectionStringName);
			IEnumerable<T> result = null;

			try
			{
				using (IDbConnection cnn = new SqlConnection(cnnString))
				{
					var rows = cnn.Query<T>(
						storedProcedure,
						parameters,
						commandType: CommandType.StoredProcedure);
					result = rows;
				}
			}
			catch
			{
				result = null;
			}

			return result ?? Enumerable.Empty<T>();

		}

		public bool SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
		{
			string cnnString = GetConnectionString(connectionStringName);
			int RowsEffected;

			try
			{
				using (IDbConnection cnn = new SqlConnection(cnnString))
				{
					RowsEffected = cnn.Execute(
						storedProcedure,
						parameters,
						commandType: CommandType.StoredProcedure);
				}
			}
			catch
			{
				RowsEffected = 0;
			}

			return RowsEffected > 0;

		}


		public void StartTransaction(string connStringName)
		{
			string connString = GetConnectionString(connStringName);

			_dbConnection = new SqlConnection(connString);
			_dbConnection.Open();
			_dbTransaction = _dbConnection.BeginTransaction();
			isClosed = false;
		}

		public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
		{
			_dbConnection.Execute(
			storedProcedure,
			parameters,
			commandType: CommandType.StoredProcedure,
			transaction: _dbTransaction
			);

		}

		public IEnumerable<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
		{
			var rows = _dbConnection.Query<T>(
				storedProcedure,
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction
				);

			return rows;
		}

		private bool isClosed = false;

		public void CommitTransaction()
		{
			_dbTransaction?.Commit();
			_dbConnection?.Close();
			isClosed = true;
		}

		public void RollbackTransaction()
		{
			_dbTransaction?.Rollback();
			isClosed = true;
		}

		public void Dispose()
		{
			if (isClosed == false)
			{
				try
				{
					CommitTransaction();
				}
				catch
				{
					//TODO : Log exception logic
				}
			}

			_dbTransaction = null;
			_dbConnection = null;
		}
	}
}
