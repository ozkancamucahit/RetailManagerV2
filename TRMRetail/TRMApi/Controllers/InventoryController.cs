using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public sealed class InventoryController : ControllerBase
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
