using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccessLayer
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

        public async Task<IEnumerable<T>> Query<T>(DataAccess.Query<T>.WithoutTransform query) where T: new()
        {
            using var connection = _factory.GetConnection();

            var cmd = CreateCommand(connection, query.Parameters, query.QueryText, query.CommandType);
            await connection.OpenAsync().ConfigureAwait(false);

            var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            var mappableProperties = ProcessQuerySchema<T>(await reader.GetSchemaTableAsync().ConfigureAwait(false));

            List<T> results = new();

            while (await reader.ReadAsync())
            {
                var result = new T();
                if (typeof(T).IsValueType)
                {
                    object objResult = new T();

                    foreach (var prop in mappableProperties)
                    {
                        prop.Value.SetValue(objResult, reader.GetValue(prop.Key));
                    }
                    result = (T)objResult;
                }
                else
                {
                    foreach (var prop in mappableProperties)
                    {
                        prop.Value.SetValue(result, reader.GetValue(prop.Key));
                    }
                }
                results.Add(result);
            }

            return results;
        }

        private static Dictionary<string, PropertyInfo> ProcessQuerySchema<T>(DataTable table)
        {
            var properties = typeof(T).GetProperties();
            var usableProperties = new Dictionary<string, PropertyInfo>();
            foreach(DataRow row in table.Rows)
            {
                var name = row.Field<string>(table.Columns[0]);
                var property = properties.FirstOrDefault(p => string.Equals(name, p.Name, StringComparison.OrdinalIgnoreCase));
                if (property == null) continue;

                usableProperties.Add(name, property);
            }
            return usableProperties;
        }

        public async Task<IEnumerable<TOut>> Query<T, TOut>(DataAccess.Query<T>.WithTransform<TOut> query) 
            where T : new() 
            where TOut:new()
        {
            using var connection = _factory.GetConnection();

            var cmd = CreateCommand(connection, query.Parameters, query.QueryText, query.CommandType);
            await connection.OpenAsync().ConfigureAwait(false);

            var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            var mappableProperties = ProcessQuerySchema<T>(await reader.GetSchemaTableAsync().ConfigureAwait(false));

            List<TOut> results = new();

            while (await reader.ReadAsync())
            {
                var result = new T();
                if (typeof(T).IsValueType)
                {
                    object objResult = new T();

                    foreach (var prop in mappableProperties)
                    {
                        prop.Value.SetValue(objResult, reader.GetValue(prop.Key));
                    }
                    result = (T)objResult;
                }
                else
                {
                    foreach (var prop in mappableProperties)
                    {
                        prop.Value.SetValue(result, reader.GetValue(prop.Key));
                    }
                }
                results.Add(query.Transform(result));
            }

            return results;
        }
    }
}
