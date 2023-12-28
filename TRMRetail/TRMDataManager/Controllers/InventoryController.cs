using System.Collections.Generic;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
	public sealed class InventoryController : ApiController
    {
		// user in admin OR manager
		[Authorize(Roles ="MANAGER,ADMIN")]
		[HttpGet]
		public IEnumerable<InventoryModel> Get()
		{

			var inventoryData = new InventoryData();
			return inventoryData.GetInventory();

		}

		// User in both admin AND warehouse
		[Authorize(Roles ="ADMIN")]
		//[Authorize(Roles ="WarehouseWorker")]
		[HttpPost] 
		public void Post(InventoryModel item) 
		{  
			InventoryData data = new InventoryData();

			data.SaveInventoryRecord(item);
		}

	}
}
