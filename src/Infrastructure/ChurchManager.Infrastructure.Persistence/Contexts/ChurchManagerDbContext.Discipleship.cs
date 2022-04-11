using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.Missions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<DiscipleshipProgram> DiscipleshipProgram { get; set; }
        public DbSet<DiscipleshipStepDefinition> DiscipleshipStepDefinition { get; set; }
        public DbSet<DiscipleshipStep> DiscipleshipStep { get; set; }



        public DbSet<Mission> Mission { get; set; }
    }
}
