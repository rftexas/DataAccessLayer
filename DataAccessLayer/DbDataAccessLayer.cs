using Dapper;
using DataAccessLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DbDataAccessLayer<TConnection> : IDataAccessLayer<TConnection>
    {
        private readonly DataConnectionFactory _factory;

        public DbDataAccessLayer(DataConnectionFactory<TConnection> factory)
        {
            _factory = factory;
        }

        public async Task Execute(DataAccess.ICommand command)
        {
            if (!command.Validate()) throw new InvalidCommandException("Command is invalid");
            using var connection = _factory.GetConnection();

            await connection.ExecuteAsync(command.QueryText, command.Parameters, commandType: command.CommandType).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Query<T>(DataAccess.IQuery<T>.IWithoutTransform query)
        {
            if (!query.Validate()) throw new InvalidQueryException("Command is invalid");
            using var connection = _factory.GetConnection();

            return await connection.QueryAsync<T>(query.QueryText, query.Parameters, commandType: query.CommandType).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TOut>> Query<T, TOut>(DataAccess.IQuery<T>.IWithTransform<TOut> query)
        {
            if (!query.Validate()) throw new InvalidQueryException("Command is invalid");
            using var connection = _factory.GetConnection();
            var rawResult = await connection.QueryAsync<T>(query.QueryText, query.Parameters, commandType: query.CommandType).ConfigureAwait(false);

            return rawResult.Select(query.Transform);
        }
    }
}
