using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{

	[Authorize]
    public sealed class SaleController : ApiController
    {
		[Authorize(Roles = "CASHIER")]
		[HttpPost]
		public async Task<IHttpActionResult> Post(SaleModel saleModel)
		{
			var data = new SaleData();
			string userId = RequestContext.Principal.Identity.GetUserId();

			data.SaveSale(saleModel, userId);
			return  Ok(saleModel);
			//return Created<ProductData>();
		}


		[Authorize(Roles ="ADMIN,MANAGER")]
		[Route("SalesReport")]
		public IEnumerable<SaleReportModel> GetSalesReport()
		{
			RequestContext.Principal.IsInRole("ADMIN");

			var saleData = new SaleData();
			return saleData.GetSaleReport();

		}

	}
}
