using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class InventoryData : IInventoryData
	{
		private readonly IConfiguration configuration;
		private readonly ISQLDataAccess sql;

		#region CTOR
		public InventoryData(IConfiguration configuration, ISQLDataAccess sql)
		{
			this.configuration = configuration;
			this.sql = sql;
		}
		#endregion

		public IEnumerable<InventoryModel> GetInventory()
		{

			var output = sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "TRMData");
			return output;
		}

		public void SaveInventoryRecord(InventoryModel item)
		{

			sql.SaveData("[dbo].[spInventoryInsert]", item, "TRMData");

		}

	}
}
