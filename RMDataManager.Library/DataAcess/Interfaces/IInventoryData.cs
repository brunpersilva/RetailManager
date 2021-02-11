using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAcess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInventory();
        void SaveInventoryRecord(InventoryModel item);
    }
}