using System.Collections.Generic;

namespace RMDataManager.Library.Internal.DataAcess
{
    public interface ISqlDataAcess
    {
        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U paramater, string connectionStringName);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U paramater);
        void RollbackTransaction();
        void SaveData<T>(string storedProcedure, T paramater, string connectionStringName);
        void SaveDataInTransaction<T>(string storedProcedure, T paramater);
        void StartTransaction(string connectionStringName);
    }
}