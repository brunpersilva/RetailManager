using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace RMDataManager.Library.Internal.DataAcess
{
    internal class SqlDataAcess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U paramater, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, paramater,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }
        public void SaveData<T>(string storedProcedure, T paramater, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, paramater,
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}
