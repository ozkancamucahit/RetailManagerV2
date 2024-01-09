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
		private readonly IConfiguration configuration;

		#region CTOR
		public SaleController(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        #endregion

        [Authorize(Roles = "CASHIER")]
		[HttpPost]
		public async Task<IActionResult> Post(SaleModel saleModel)
		{
			var data = new SaleData(configuration);
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();

			data.SaveSale(saleModel, userId);
			return Ok(saleModel);
		}


		[Authorize(Roles = "ADMIN,MANAGER")]
		[Route("SalesReport")]
		public IEnumerable<SaleReportModel> GetSalesReport()
		{
			var saleData = new SaleData(configuration);
			return saleData.GetSaleReport();

		}
	}
}
