using System;
using System.Linq;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;
using Convey.CQRS.Queries;
using GroupMemberViewModel = ChurchManager.Domain.Shared.GroupMemberViewModel;
using GroupTypeViewModel = ChurchManager.Domain.Shared.GroupTypeViewModel;
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
            CreateMap<GroupType, GroupTypeViewModel>();

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

            CreateMap<Schedule, ScheduleViewModel>()
                .ForMember(d => d.ScheduleText, opt => opt.MapFrom(src => src != null ? src.ToFriendlyScheduleText(true) : null))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src != null && src.GetICalEvent() != null ? src.GetICalEvent().DtStart.Date : (DateTime?)null))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src != null && src.GetICalEvent() != null ? src.GetICalEvent().DtEnd.Date : (DateTime?)null))
                .ForMember(dest => dest.MeetingTime, opt => opt.MapFrom(src => src != null && src.GetICalEvent() != null ? src.GetICalEvent().DtStart.Value.TimeOfDay.ToString(@"hh\:mm") : null))
                .ForMember(dest => dest.RecurrenceRule, opt => opt.MapFrom(src => src.WithInterval()))
                ;
        }
    }
}