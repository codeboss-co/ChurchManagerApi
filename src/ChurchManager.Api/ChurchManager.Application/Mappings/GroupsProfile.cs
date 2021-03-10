using AutoMapper;
using ChurchManager.Application.Features.Groups.Queries;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Domain.Model;

namespace ChurchManager.Application.Mappings
{
    public class GroupsProfile : Profile
    {
        public GroupsProfile()
        {
            CreateMap<GroupDomain, GroupSummaryViewModel>()
                .ForMember(d => d.MembersCount, 
                    opt => opt.MapFrom(src => src.Members.Count));
        }
    }
}
