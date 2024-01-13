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
	public sealed class ProductData : IProductData
	{
		private readonly ISQLDataAccess sql;

		public ProductData( ISQLDataAccess sql)
		{
			this.sql = sql;
		}

		public IEnumerable<ProductModel> GetProducts()
		{
			IEnumerable<ProductModel> result;
			try
			{

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

			var result = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = id }, "TRMData").FirstOrDefault();
			return result;
		}
	}
}
