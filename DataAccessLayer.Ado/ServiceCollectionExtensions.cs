using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Ado
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdoDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IDataAccessLayer, AdoDataAccessLayer>();
            return services;
        }
    }
}
