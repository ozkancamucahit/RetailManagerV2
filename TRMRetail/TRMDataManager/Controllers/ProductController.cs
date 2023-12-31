﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize(Roles = "CASHIER")]
	public sealed class ProductController : ApiController
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
