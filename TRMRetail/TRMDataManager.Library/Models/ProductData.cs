using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;

namespace TRMDataManager.Library.Models
{
	public sealed class ProductData
	{
		public IEnumerable<ProductModel> GetProducts()
		{
			IEnumerable<ProductModel> result;
			try
			{
				SQLDataAccess sql = new SQLDataAccess();

				result = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
			}
			catch
			{
				result = null;
			}

			return result ?? Enumerable.Empty<ProductModel>();
		}
	}
}
