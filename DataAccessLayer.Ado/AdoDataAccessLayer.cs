using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccessLayer.Ado
{
    public class AdoDataAccessLayer : IDataAccessLayer
    {
        private readonly DataConnectionFactory _factory;

        public AdoDataAccessLayer(DataConnectionFactory factory)
        {
            _factory = factory;
        }

        private static DbCommand CreateCommand(DbConnection connection, object parameters, string text, CommandType commandType)
        {
            var command = connection.CreateCommand();
            command.CommandText = text;
            command.CommandType = commandType;

            if (parameters == null) return command;

            var properties = parameters.GetType().GetProperties();
            foreach(var prop in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = prop.Name;
                parameter.Value = prop.GetValue(parameters);
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public async Task Execute(DataAccess.Command command)
        {
            using var connection = _factory.GetConnection();

            var cmd = CreateCommand(connection, command.Parameters, command.QueryText, command.CommandType);

            await connection.OpenAsync().ConfigureAwait(false);

            await cmd.ExecuteNonQueryAsync();
        }

        public Task<T> Query<T>(DataAccess.Query<T>.WithoutTransform query)
        {
            throw new NotImplementedException();
        }

        public Task<TOut> Query<T, TOut>(DataAccess.Query<T>.WithTransform<TOut> query)
        {
            throw new NotImplementedException();
        }
    }
}
