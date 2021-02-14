using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAcess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReport();
        decimal GetTaxRate();
        void SaveSales(SaleModel saleInfo, string cashierId);
    }
}