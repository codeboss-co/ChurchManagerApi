using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Model;
using ChurchManager.Persistence.Models.People;

namespace ChurchManager.Application.Mappings
{
    public class PeopleMappingProfile : Profile
    {
        public PeopleMappingProfile()
        {
            CreateMap<PersonDomain, PersonViewModel>();

            CreateMap<FamilyMemberDomain, FamilyMembersViewModel>();

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
        }
    }
}
