using Core.Common;
using Eintech.DataLayer.Contracts;
using Eintech.DataLayer.Infrastructure;
using Eintech.DataModels;
using Microsoft.Extensions.Logging;

namespace Eintech.DataLayer.Repositories
{
    public class CustomerRepository: BaseRepository<Customer>, ICustomerRepository
    {
        private ILogger<CustomerRepository> logger;

        public CustomerRepository(EintechDbContext dataContext, ILogger<CustomerRepository> logger): base(dataContext, logger)
        {
            this.logger = logger;
        }
    }
}
