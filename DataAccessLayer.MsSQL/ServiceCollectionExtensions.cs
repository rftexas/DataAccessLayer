using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new DataConnectionFactory(connectionString, s => new SqlConnection(s)));

            return services;
        }
    }
}
