using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAcess
{
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAcess _sql;

        public InventoryData(ISqlDataAcess sql)
        {
            _sql = sql;
        }
        public List<InventoryModel> GetInventory()
        {
            var output = _sql.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "RMData");
            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            _sql.SaveData("spInventory_Insert", item, "RMData");
        }
    }
}
