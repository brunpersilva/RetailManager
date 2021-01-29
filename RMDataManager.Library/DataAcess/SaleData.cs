using RMDataManager.Library.Internal.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Library.DataAcess
{
    public class SaleData
    {

        public void SaveSales(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel { ProductId = item.ProductId, Quantity = item.Quantity };

                var prodctInfo = products.GetProductById(detail.ProductId);
                if (prodctInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the Database.");
                }
                detail.PurchasePrice = (prodctInfo.RetailPrice * detail.Quantity);

                if (prodctInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            SqlDataAcess sql = new SqlDataAcess();
            sql.SaveData("dbo.spSale_Insert", sale, "RMData");
            sale.Id = sql.LoadData<int, dynamic>("dbo.spSaleLookup", new { sale.CashierId, sale.SaleDate }, "RMData").FirstOrDefault();


            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetail_Insert", item, "RMData");
            }



            //products.GetProductById(details.);

        }
    }
}
