using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Library.DataAcess
{
    public class SaleData
    {
        private readonly IConfiguration _config;

        public SaleData(IConfiguration config)
        {
            _config = config;
        }

        public void SaveSales(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData(_config);
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

            using (SqlDataAcess sql = new SqlDataAcess(_config))
            {
                try
                {
                    sql.StartTransaction("RMData");
                    //Save the sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);
                    //Get the Id from the sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                    //Finish filling in the sale detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        //Save the sale detail model
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }
                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }
        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAcess sql = new SqlDataAcess(_config);
            var output = sql.LoadData<SaleReportModel, dynamic>("spSale_SaleReport",new { }, "RMData");
            return output;
        }
    }
}
