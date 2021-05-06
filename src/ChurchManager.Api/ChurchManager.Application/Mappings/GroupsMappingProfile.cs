using System.Collections.Generic;
using AutoMapper;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Shared;
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

            CreateMap<Group, GroupViewModel>();

            // Attendance Records
            CreateMap<GroupMemberAttendance, GroupMemberAttendanceViewModel>().ReverseMap();
            CreateMap<GroupMember, GroupMemberViewModel>()
                .ForMember(dest => dest.GroupMemberId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FullName.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Person.FullName.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.FullName.LastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Person.Gender))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Person.PhotoUrl))
                ;

            CreateMap<GroupAttendance, GroupAttendanceDetailViewModel>()
                .ForMember(d => d.GroupName,
                    opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.Attendees));
;
        }
    }
}
