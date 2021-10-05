using System;
using System.Data.Common;

namespace DataAccessLayer
{
    public class DataConnectionFactory
    {
        public DataConnectionFactory(string connString, Func<string, DbConnection> factory) => (_connectionString, _factory) = (connString, factory);

        public DbConnection GetConnection()
        {
            return _factory(_connectionString);
        }

        private readonly string _connectionString;
        private readonly Func<string, DbConnection> _factory;
    }
}
