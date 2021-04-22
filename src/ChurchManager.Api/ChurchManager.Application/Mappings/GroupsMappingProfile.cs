using AutoMapper;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Model;
using ChurchManager.Persistence.Models.Groups;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Mappings
{
    public class GroupsMappingProfile : Profile
    {
        public GroupsMappingProfile()
        {
            CreateMap<GroupDomain, GroupSummaryViewModel>()
                .ForMember(d => d.MembersCount, 
                    opt => opt.MapFrom(src => src.Members.Count));

            CreateMap<PagedResult<GroupDomain>, PagedResult<GroupSummaryViewModel>>()
                .ForMember(d => d.Items,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<Group, SelectItemViewModel>().ReverseMap();


            // Attendance Records
            CreateMap<GroupAttendance, GroupAttendanceViewModel>()
                .ForMember(d => d.GroupName,
                    opt => opt.MapFrom(src => src.Group.Name));
        }
    }
}
