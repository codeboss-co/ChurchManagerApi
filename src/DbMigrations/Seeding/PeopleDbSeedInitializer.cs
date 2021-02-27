using System.Threading.Tasks;
using CodeBoss.AspNetCore.Startup;
using DbMigrations.DbContext;

namespace DbMigrations.Seeding
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class PeopleDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 1;

        private readonly ChurchManagerDbContext _dbContext;

        public PeopleDbSeedInitializer(ChurchManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
