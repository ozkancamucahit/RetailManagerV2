using System.Collections.Generic;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public interface ISaleData
	{
		IEnumerable<SaleReportModel> GetSaleReport();
		void SaveSale(SaleModel saleInfo, string cashierId);
	}
}