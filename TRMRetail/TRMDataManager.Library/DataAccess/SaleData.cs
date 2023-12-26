using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public sealed class SaleData
	{
		

		public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            var details = new List<SaleDetailDBModel>();
            var product = new ProductData();
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
            using (var sql = new SQLDataAccess())
            {
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
            }
            #endregion
        }


        //public IEnumerable<ProductModel> GetProducts()
        //{
        //	IEnumerable<ProductModel> result;
        //	try
        //	{
        //		

        //		result = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
        //	}
        //	catch
        //	{
        //		result = null;
        //	}

        //	return result ?? Enumerable.Empty<ProductModel>();
        //}
    }
}
