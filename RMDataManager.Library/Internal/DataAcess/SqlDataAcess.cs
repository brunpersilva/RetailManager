using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;

namespace RMDataManager.Library.Internal.DataAcess
{
    internal class SqlDataAcess : IDisposable
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
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }
        public void SaveDataInTransaction<T>(string storedProcedure, T paramater)
        {

            _connection.Execute(storedProcedure, paramater,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U paramater)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, paramater,
                   commandType: CommandType.StoredProcedure, transaction : _transaction).ToList();

            return rows;
        }
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            CommitTransaction();
        }
        //open connect/start transaction method
        //load using the transaction
        //save using the transaction
        //close connection/stop transaction methid
        //dispose
    }
}
