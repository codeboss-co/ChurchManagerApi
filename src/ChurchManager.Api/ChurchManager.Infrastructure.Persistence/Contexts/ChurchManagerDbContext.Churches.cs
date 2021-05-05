using ChurchManager.Domain.Model.Churches;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<Church> Church { get; set; }
        public DbSet<ChurchAttendanceType> ChurchAttendanceType { get; set; }
        public DbSet<ChurchAttendance> ChurchAttendance { get; set; }
    }
}
