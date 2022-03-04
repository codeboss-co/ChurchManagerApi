using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChurchManager.Application.Tests.Common
{
    public class DbContextFactory
    {
        public static ChurchManagerDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ChurchManagerDbContext(options, new LocalTenantProvider(), null);

            context.Database.EnsureCreated();

            

            context.SaveChanges();

            return context;
        }

        public static void Destroy(ChurchManagerDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
