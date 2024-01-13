using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Cashier")]

	public sealed class ProductController : ControllerBase
	{
		private readonly IProductData data;

		public ProductController( IProductData data)
        {
			this.data = data;
		}

        [HttpGet]
		public IEnumerable<ProductModel> Get()
		{

			IEnumerable<ProductModel> result = data.GetProducts();

			return result ?? Enumerable.Empty<ProductModel>();
		}
	}
}
