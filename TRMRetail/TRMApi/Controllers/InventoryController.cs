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
		private readonly IInventoryData inventoryData;

		#region CTOR
		public InventoryController( IInventoryData inventoryData)
        {
			this.inventoryData = inventoryData;
		}
        #endregion

        // user in admin OR manager
        [Authorize(Roles ="Manager,Admin")]
		[HttpGet]
		public IEnumerable<InventoryModel> Get()
		{
			return inventoryData.GetInventory();

		}

		// User in both admin AND warehouse
		[Authorize(Roles ="Admin")]
		//[Authorize(Roles ="WarehouseWorker")]
		[HttpPost] 
		public void Post(InventoryModel item) 
		{  
			inventoryData.SaveInventoryRecord(item);
		}

	}
}
