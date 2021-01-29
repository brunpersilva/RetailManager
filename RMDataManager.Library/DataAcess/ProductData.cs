using RMDataManager.Library.Internal.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAcess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAcess sql = new SqlDataAcess();
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "RMData");

            return output;
        }
        public ProductModel GetProductById(int productId)
        {
            SqlDataAcess sql = new SqlDataAcess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "RMData").FirstOrDefault();

            return output;
        }
    }
}
