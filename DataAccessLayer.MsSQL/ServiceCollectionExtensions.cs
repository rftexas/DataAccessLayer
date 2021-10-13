using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace DataAccessLayer.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDataAccessLayer<TConnection>(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new DataConnectionFactory<TConnection>(connectionString, s => new SqlConnection(s)));
            services.AddScoped<IDataAccessLayer<TConnection>, AdoDataAccessLayer<TConnection>>();

            return services;
        }
    }
}
