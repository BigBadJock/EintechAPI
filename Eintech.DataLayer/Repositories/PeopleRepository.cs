using Core.Common;
using Eintech.DataLayer.Contracts;
using Eintech.DataLayer.Infrastructure;
using Eintech.DataModels;
using Microsoft.Extensions.Logging;

namespace Eintech.DataLayer.Repositories
{
    public class PeopleRepository: BaseRepository<Person>, IPeopleRepository
    {
        private ILogger<PeopleRepository> logger;

        public PeopleRepository(EintechDbContext dataContext, ILogger<PeopleRepository> logger): base(dataContext, logger)
        {
            this.logger = logger;
        }
    }
}
