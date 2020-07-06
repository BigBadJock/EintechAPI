using Core.Common;
using Eintech.BusinessLayer.Contracts;
using Eintech.DataLayer.Contracts;
using Eintech.DataModels;
using Microsoft.Extensions.Logging;

namespace Eintech.BusinessLayer
{
    public class PeopleDataService: BaseDataService<Person>, IPeopleDataService
    {

        public PeopleDataService(IPeopleRepository repository, ILogger<Person> logger): base(repository, logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
    }
}
