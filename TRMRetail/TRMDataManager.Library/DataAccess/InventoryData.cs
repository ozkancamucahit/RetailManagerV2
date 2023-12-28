using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class InventoryData
	{
		public IEnumerable<InventoryModel> GetInventory()
		{
			var sql = new SQLDataAccess();

			var output = sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new {}, "TRMData");
			return output;
		}

		public void SaveInventoryRecord(InventoryModel item)
		{
			var sql = new SQLDataAccess();

			sql.SaveData("[dbo].[spInventoryInsert]", item, "TRMData");

		}

	}
}
