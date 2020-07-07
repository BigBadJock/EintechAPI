using Eintech.DataLayer.Contracts;
using Eintech.DataLayer.Infrastructure;
using Eintech.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Eintech.DataLayer.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void RegisterDataServices(this IServiceCollection services)
        {
            services.AddScoped<EintechDbContext, EintechDbContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
