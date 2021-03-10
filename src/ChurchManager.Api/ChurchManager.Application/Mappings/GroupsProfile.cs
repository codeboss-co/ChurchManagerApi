using AutoMapper;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Domain.Model;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Mappings
{
    public class GroupsProfile : Profile
    {
        public GroupsProfile()
        {
            CreateMap<GroupDomain, GroupSummaryViewModel>()
                .ForMember(d => d.MembersCount, 
                    opt => opt.MapFrom(src => src.Members.Count));


            CreateMap<PagedResult<GroupDomain>, PagedResult<GroupSummaryViewModel>>()
                .ForMember(d => d.Items,
                    opt => opt.MapFrom(src => src.Items));
        }
    }
}
