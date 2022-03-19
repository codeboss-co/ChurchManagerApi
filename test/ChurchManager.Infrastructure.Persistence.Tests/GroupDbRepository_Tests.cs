using AutoMapper;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Features.Groups.Infrastructure.Mapper;
using ChurchManager.Features.Groups.Services;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Repositories;
using ChurchManager.Infrastructure.Persistence.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace ChurchManager.Infrastructure.Persistence.Tests
{
    public class GroupDbRepository_Tests
    {
        private readonly DbContextOptions<ChurchManagerDbContext> _options =
            new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;")
                //.UseLoggerFactory(_loggerFactory) //Optional, this logs SQL generated by EF Core to the Console
                .Options;

        private static IMapper _mapper;
        private readonly ITestOutputHelper _output;

        public GroupDbRepository_Tests(ITestOutputHelper output)
        {
            _output = output;

            if(_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new GroupsMappingProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }
        }

        [Fact]
        public async Task Should_construct_group_tree()
        {
            using (var dbContext = new ChurchManagerDbContext(_options, new LocalTenantProvider(), null))
            {
                var dbRepository = new GroupDbRepository(dbContext, _mapper);

                // This is bad
                var groups = await dbRepository.Queryable()
                    .AsNoTracking()
                    .Include(x => x.Groups)
                    .ToListAsync();

                BuildMenu(groups);

                // This is better
                var better = await dbRepository.GroupsWithChildrenAsync(10);
            }
        }

        [Fact]
        public async Task Should_return_group_roles_for_group_GroupService()
        {
            using (var dbContext = new ChurchManagerDbContext(_options, null, null))
            {
                var dbRepository = new GroupDbRepository(dbContext, _mapper);
                var service = new GroupsService(dbRepository);

                var roles = await service.GroupRolesForGroupAsync(1);

                Assert.NotEmpty(roles);
            }
        }


        private void BuildMenu(IEnumerable<Group> data, int? parentId = null)
        {
            var items = data.Where(d => d.ParentGroupId == parentId).OrderBy(i => i.Id).ToList();
            if (items.Any())
                foreach (var item in items)
                {
                    _output.WriteLine("\t" + item.Name);
                    BuildMenu(data, item.Id);
                }
        }
    }
}