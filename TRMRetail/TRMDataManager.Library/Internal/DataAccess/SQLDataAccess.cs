using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
	internal sealed class SQLDataAccess
	{
		public string GetConnectionString(string name)
		{
			try
			{
				return ConfigurationManager.ConnectionStrings[name].ConnectionString ?? String.Empty;
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
	}
}
