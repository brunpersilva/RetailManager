﻿using RMDataManager.Library.Internal.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAcess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            SqlDataAcess sql = new SqlDataAcess();
            var output = sql.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "RMData");
            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAcess sql = new SqlDataAcess();
            sql.SaveData("spInventory_Insert", item, "RMData");
        }
    }
}