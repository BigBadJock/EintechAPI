using Core.Common;
using Eintech.BusinessLayer.Contracts;
using Eintech.DataLayer.Contracts;
using Eintech.DataModels;
using Microsoft.Extensions.Logging;

namespace Eintech.BusinessLayer
{
    public class CustomerDataService: BaseDataService<Customer>, ICustomerDataService
    {

        public CustomerDataService(ICustomerRepository repository, ILogger<Customer> logger): base(repository, logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
    }
}
