using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using Convey.CQRS.Queries;
using GroupMemberViewModel = ChurchManager.Domain.Shared.GroupMemberViewModel;

namespace ChurchManager.Application.Mappings
{
    public class PeopleMappingProfile : Profile
    {
        public PeopleMappingProfile()
        {
            CreateMap<Person, PersonViewModel>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                ;

            CreateMap<Person, PersonViewModelBasic>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                ;

            CreateMap<Person, GroupMemberViewModel>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(d => d.MiddleName,
                    opt => opt.MapFrom(src => src.FullName.MiddleName))
                .ForMember(d => d.PhotoUrl,
                    opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(d => d.PersonId,
                    opt => opt.MapFrom(src => src.Id))
                ;


            CreateMap<PagedResult<Person>, PagedResult<PersonViewModel>>()
                .ForMember(d => d.Items,
                    opt => opt.MapFrom(src => src.Items));


            CreateMap<Person, PersonViewModel>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ConnectionStatus,
                    o => o.MapFrom(src =>
                        src.ConnectionStatus == null ? ConnectionStatus.Unknown.Value : src.ConnectionStatus.Value))
                .ForMember(d => d.Gender,
                    o => o.MapFrom(src => src.Gender == null ? Gender.Unknown.Value : src.Gender.Value))
                .ForMember(d => d.AgeClassification,
                    o => o.MapFrom(src =>
                        src.AgeClassification == null ? AgeClassification.Unknown.Value : src.AgeClassification.Value))
                .ForMember(d => d.CommunicationPreference,
                    o => o.MapFrom(src =>
                        src.CommunicationPreference == null
                            ? CommunicationType.None.Value
                            : src.CommunicationPreference.Value))
                // Gets the persons family members excluding them
                .ForMember(d => d.FamilyMembers, o => o.MapFrom(src
                    => src.Family == null
                        ? new List<PersonViewModelBasic>(0)
                        : src.Family.FamilyMembers
                            .Where(x => x.Id != src.Id)
                            .Select(x => new PersonViewModelBasic
                            {
                                PersonId = x.Id,
                                FullName = x.FullName,
                                AgeClassification = x.AgeClassification,
                                BirthDate = x.BirthDate,
                                Gender = x.Gender,
                                PhotoUrl = x.PhotoUrl
                            })
                            .ToList()))
                ;
        }
    }
}