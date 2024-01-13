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
	public sealed class SaleData : ISaleData
	{
		private readonly IProductData product;
		private readonly ISQLDataAccess sql;
		#region CTOR
		public SaleData(IProductData product, ISQLDataAccess sql)
		{
			this.product = product;
			this.sql = sql;
		}
		#endregion

		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			var details = new List<SaleDetailDBModel>();
			decimal taxRate = ConfigHelper.GetTaxRate() / 100;

			foreach (var item in saleInfo.SaleDetails)
			{
				var detail = new SaleDetailDBModel
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity
				};

				var productInfo = product.GetProductById(detail.ProductId);

				if (productInfo == null)
				{

					throw new ArgumentNullException("ProductInfo", $"Product Info Not Found Id : {detail.ProductId}");
				}

				detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

				if (productInfo.IsTaxable)
				{
					detail.Tax = (detail.PurchasePrice * taxRate);
				}

				details.Add(detail);
			}

			#region CREATE SALE MODEL
			SaleDBModel sale = new SaleDBModel
			{
				SubTotal = details.Sum(x => x.PurchasePrice),
				Tax = details.Sum(x => x.Tax),
				CashierId = cashierId
			};
			sale.Total = sale.SubTotal + sale.Tax;
			#endregion

			#region SAVE THE SALE MODEL
			try
			{
				sql.StartTransaction("TRMData");
				sql.SaveDataInTransaction("dbo.sp_Sale_Insert", sale);

				sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

				foreach (var item in details)
				{
					item.SaleId = sale.Id;
					sql.SaveDataInTransaction("dbo.spSale_Detail_Insert", item);
				}

				sql.CommitTransaction();
			}
			catch
			{
				sql.RollbackTransaction();
				throw;
			}
			#endregion
		}

		public IEnumerable<SaleReportModel> GetSaleReport()
		{

			var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
			return output;
		}
	}
}
