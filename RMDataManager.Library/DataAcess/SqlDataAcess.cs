﻿using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RMDataManager.Library.DataAcess
{
    public class SqlDataAcess : IDisposable, ISqlDataAcess
    {
        private bool _isClosed = false;
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAcess> _logger;

        public SqlDataAcess(IConfiguration config, ILogger<SqlDataAcess> logger)
        {
            _config = config;
            _logger = logger;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
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
            _isClosed = false;
        }
        public void SaveDataInTransaction<T>(string storedProcedure, T paramater)
        {

            _connection.Execute(storedProcedure, paramater,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U paramater)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, paramater,
                   commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            _isClosed = true;
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            _isClosed = true;
        }

        public void Dispose()
        {
            if (!_isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Commit transaction failed in the dispose method.");
                }
            }

            _transaction = null;
            _connection = null;

        }
    }
}
