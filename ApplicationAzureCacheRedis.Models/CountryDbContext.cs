using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ApplicationAzureCacheRedis.Models
{
    public class CountryDbContext : DbContext
    {
        public CountryDbContext() : base("CountryDbContext")
        {
        }

        public DbSet<Country> Countries { get; set; }
    }

    public class CountryInitializer : CreateDatabaseIfNotExists<CountryDbContext>
    {
        protected override void Seed(CountryDbContext context)
        {
            CountryInitalizer.Initialize(context);
        }
    }

    public class CountryConfiguration : DbConfiguration
    {
        public CountryConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}
