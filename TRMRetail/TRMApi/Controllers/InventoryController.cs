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
		private readonly IConfiguration configuration;

		#region CTOR
		public InventoryController(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        #endregion

        // user in admin OR manager
        [Authorize(Roles ="MANAGER,ADMIN")]
		[HttpGet]
		public IEnumerable<InventoryModel> Get()
		{

			var inventoryData = new InventoryData(configuration);
			return inventoryData.GetInventory();

		}

		// User in both admin AND warehouse
		[Authorize(Roles ="ADMIN")]
		//[Authorize(Roles ="WarehouseWorker")]
		[HttpPost] 
		public void Post(InventoryModel item) 
		{  
			InventoryData data = new InventoryData(configuration);

			data.SaveInventoryRecord(item);
		}

	}
}
