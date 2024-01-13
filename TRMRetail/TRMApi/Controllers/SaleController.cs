using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public sealed class SaleController : ControllerBase
	{
		private readonly ISaleData data;

		#region CTOR
		public SaleController( ISaleData saleData)
        {
			this.data = saleData;
		}
        #endregion

        [Authorize(Roles = "Cashier")]
		[HttpPost]
		public async Task<IActionResult> Post(SaleModel saleModel)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

			data.SaveSale(saleModel, userId);
			return Ok(saleModel);
		}


		[Authorize(Roles = "Admin,Manager")]
		[Route("SalesReport")]
		[HttpGet]
		public IEnumerable<SaleReportModel> GetSalesReport()
		{
			return data.GetSaleReport();

		}
	}
}
