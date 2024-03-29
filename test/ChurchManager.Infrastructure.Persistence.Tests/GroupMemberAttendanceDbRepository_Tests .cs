using AutoMapper;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Repositories;
using ChurchManager.Infrastructure.Persistence.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace ChurchManager.Infrastructure.Persistence.Tests
{
    public class GroupMemberAttendanceDbRepository_Tests
    {
        private readonly DbContextOptions<ChurchManagerDbContext> _options =
            new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;")
                //.UseLoggerFactory(_loggerFactory) //Optional, this logs SQL generated by EF Core to the Console
                .Options;

        private static IMapper _mapper;
        private readonly ITestOutputHelper _output;

        public GroupMemberAttendanceDbRepository_Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Should_return_people_in_groups_statistics()
        {
            using (var dbContext = new ChurchManagerDbContext(_options, new LocalTenantProvider(), null))
            {
                var groupsDbRepository = new GroupDbRepository(dbContext, _mapper);

                var groups = await groupsDbRepository.Queryable()
                    .AsNoTracking()
                    .Include(x => x.GroupType)
                    .Where(x => x.GroupType.Name.Contains("Cell"))
                    .Select(x => x.Id)
                    .ToListAsync();

                var sut = new GroupMemberAttendanceDbRepository(dbContext);

                var statistics = await sut.PeopleStatisticsAsync(groups);

                Assert.True(statistics.firstTimersCount >= 0);
            }
        }

    }
}