using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class ProductData
	{
		private readonly IConfiguration configuration;

		public ProductData(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public IEnumerable<ProductModel> GetProducts()
		{
			IEnumerable<ProductModel> result;
			try
			{
				SQLDataAccess sql = new SQLDataAccess(configuration);

				result = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
			}
			catch
			{
				result = null;
			}

			return result ?? Enumerable.Empty<ProductModel>();
		}

		public ProductModel GetProductById(int id)
		{
			var sql = new SQLDataAccess(configuration);

			var result = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = id}, "TRMData").FirstOrDefault();
			return result;
		}
	}
}
