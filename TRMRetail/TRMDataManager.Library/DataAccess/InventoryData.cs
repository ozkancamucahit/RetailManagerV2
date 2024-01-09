using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class InventoryData
	{
		private readonly IConfiguration configuration;

		#region CTOR
		public InventoryData(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        #endregion

        public IEnumerable<InventoryModel> GetInventory()
		{
			var sql = new SQLDataAccess(configuration);

			var output = sql.LoadData<InventoryModel, dynamic>("[dbo].[spInventory_GetAll]", new {}, "TRMData");
			return output;
		}

		public void SaveInventoryRecord(InventoryModel item)
		{
			var sql = new SQLDataAccess(configuration);

			sql.SaveData("[dbo].[spInventoryInsert]", item, "TRMData");

		}

	}
}
