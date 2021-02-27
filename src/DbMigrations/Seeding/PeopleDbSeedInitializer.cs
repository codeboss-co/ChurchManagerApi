using System.Threading.Tasks;
using CodeBoss.AspNetCore.Startup;

namespace DbMigrations.Seeding
{
    /// <summary>
    /// Seeds the database with some dummy data
    /// </summary>
    public class PeopleDbSeedInitializer : IInitializer
    {
        public int OrderNumber { get; } = 1;
        
        public PeopleDbSeedInitializer()
        {
        
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
