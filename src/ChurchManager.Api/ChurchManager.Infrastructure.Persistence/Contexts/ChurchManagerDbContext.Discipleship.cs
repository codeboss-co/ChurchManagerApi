﻿using ChurchManager.Persistence.Models.Discipleship;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Contexts
{
    public partial class ChurchManagerDbContext
    {
        public DbSet<DiscipleshipProgram> DiscipleshipProgram { get; set; }
        public DbSet<DiscipleshipType> DiscipleshipType { get; set; }
        public DbSet<DiscipleshipStepDefinition> DiscipleshipStepDefinition { get; set; }
        public DbSet<DiscipleshipStep> DiscipleshipStep { get; set; }

    }
}
