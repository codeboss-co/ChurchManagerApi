using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.Groups;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ConveyPaging = Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : GenericRepositoryAsync<Group>, IGroupDbRepository
    {
        private readonly IDataShapeHelper<Group> _dataShaper;
    
        public GroupDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<GroupDomain>> AllPersonsGroups(int personId, RecordStatus recordStatus, CancellationToken ct = default)
        {
            var groups = await Queryable(new AllPersonsGroupsSpecification(personId, recordStatus))
                .Select( x => new GroupDomain(x))
                .ToListAsync(ct);

            return groups;
        }

        public async Task<ConveyPaging.PagedResult<GroupDomain>> BrowsePersonsGroups(int personId, string search, QueryParameter query, CancellationToken ct = default)
        {
            // Paging
            var pagedResult = await Queryable()
                .Specify(new BrowsePersonsGroupsSpecification(personId, search))
                //.FieldLimit(query)
                .PaginateAsync(query);
            
            // Shaping
            //var shapedData =  await _dataShaper.ShapeDataAsync(pagedResult.Items, query.Fields);
            
            return ConveyPaging.PagedResult<GroupDomain>.Create(
                pagedResult.Items.Select(x => new GroupDomain(x)),
                pagedResult.CurrentPage,
                pagedResult.ResultsPerPage, 
                pagedResult.TotalPages, 
                pagedResult.TotalResults);
        }

        public async Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, CancellationToken ct)
        {
            var groups = await Queryable(new GroupMembersSpecification(groupId, RecordStatus.Active))
                .SelectMany(x => x.Members)
                .Select(x => new GroupMemberViewModel
                {
                    PersonId = x.PersonId,
                    GroupMemberId = x.Id,
                    FirstName = x.Person.FullName.FirstName,
                    MiddleName = x.Person.FullName.MiddleName,
                    LastName = x.Person.FullName.LastName,
                    Gender = x.Person.Gender,
                    PhotoUrl = x.Person.PhotoUrl
                })
                .ToArrayAsync(ct);

            return groups;
        }

        public async Task<IEnumerable<GroupMemberRole>> GroupRolesForGroupAsync(int groupId, CancellationToken ct)
        {
            return await Queryable(new GroupRolesForGroupSpecification(groupId))
                .SelectMany(x => x.Members)
                .Select(x => x.GroupMemberRole)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int maxDepth, CancellationToken ct = default)
        {
            var query = Queryable()
                .AsNoTracking()
                .Where(x => x.ParentGroupId == null) // Exclude children\
                .Select(GroupProjection(maxDepth))
                ;

            return await query.ToListAsync(ct);
        }

        /// <summary>
        /// https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6
        /// </summary>
        private Expression<Func<Group, GroupViewModel>> GroupProjection(int maxDepth, int currentDepth = 0)
        {
            currentDepth++;

            Expression<Func<Group, GroupViewModel>> result = group => new GroupViewModel
            {
                Name = group.Name,
                Description = group.Description,
                Groups = currentDepth == maxDepth
                    ? new List<GroupViewModel>() // Reached maximum depth so stop
                    : group.Groups.AsQueryable()
                        .Select(GroupProjection(maxDepth, currentDepth))
                        .ToList()
            };

            return result;
        }

        private IQueryable<Group> FilterByColumn(IQueryable<Group> queryable, string search)
        {
            queryable.Include("GroupType");
            queryable.Include("Members.GroupMemberRole");
            queryable.Include("Members.Person");

            if (string.IsNullOrEmpty(search))
            {
                return queryable;
            }

            var predicate = PredicateBuilder.New<Group>();
            predicate = predicate.And(p => p.Name.Contains(search.Trim()));
            predicate = predicate.Or(p => p.Description.Contains(search.Trim()));

            return queryable.Where(predicate);
        }
    }
}
