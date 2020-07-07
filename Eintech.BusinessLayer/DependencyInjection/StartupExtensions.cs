using Eintech.BusinessLayer.Contracts;
using Eintech.DataLayer.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eintech.BusinessLayer.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void RegisterAPIServices(this IServiceCollection services)
        {
            services.RegisterDataServices();

            services.AddScoped<ICustomerDataService, CustomerDataService>();
        }
    }
}
