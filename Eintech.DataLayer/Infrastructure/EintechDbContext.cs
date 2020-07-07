using Eintech.DataLayer.Contracts;
using Eintech.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Eintech.DataLayer.Infrastructure
{
    public class EintechDbContext: DbContext, IDbContext
    {
        public EintechDbContext(DbContextOptions options): base(options)
        {
        }

        public async virtual void Commit()
        {
            await base.SaveChangesAsync();
        }

        public DbSet<Customer> Customers { get; set; }

    }
}
