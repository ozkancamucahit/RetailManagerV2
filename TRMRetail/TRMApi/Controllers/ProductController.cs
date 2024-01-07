using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "CASHIER")]

	public sealed class ProductController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<ProductModel> Get()
		{
			ProductData data = new ProductData();

			IEnumerable<ProductModel> result = data.GetProducts();

			return result ?? Enumerable.Empty<ProductModel>();
		}
	}
}
