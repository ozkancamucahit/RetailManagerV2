using System.Collections.Generic;

namespace TRMDataManager.Library.Internal.DataAccess
{
	public interface ISQLDataAccess
	{
		void CommitTransaction();
		void Dispose();
		string GetConnectionString(string name);
		IEnumerable<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
		IEnumerable<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
		void RollbackTransaction();
		bool SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
		void SaveDataInTransaction<T>(string storedProcedure, T parameters);
		void StartTransaction(string connStringName);
	}
}