using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;
using Convey.CQRS.Queries;
using GroupMemberViewModel = ChurchManager.Domain.Shared.GroupMemberViewModel;
using GroupViewModel = ChurchManager.Domain.Shared.GroupViewModel;

namespace ChurchManager.Application.Mappings
{
    public class GroupsMappingProfile : Profile
    {
        public GroupsMappingProfile()
        {
            CreateMap<Group, GroupSummaryViewModel>()
                .ForMember(d => d.GroupId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.GroupType, opt => opt.MapFrom(src => src.GroupType.Name))
                .ForMember(d => d.TakesAttendance, opt => opt.MapFrom(src => src.GroupType.TakesAttendance))
                .ForMember(d => d.MembersCount, opt => opt.MapFrom(src => src.Members.Count))
                ;

            CreateMap<PagedResult<Group>, PagedResult<GroupSummaryViewModel>>()
                .ForMember(d => d.Items,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<GroupType, SelectItemViewModel>().ReverseMap();
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
        }
    }
}
