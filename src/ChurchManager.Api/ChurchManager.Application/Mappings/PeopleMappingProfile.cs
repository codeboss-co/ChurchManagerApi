using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Model;

namespace ChurchManager.Application.Mappings
{
    public class PeopleMappingProfile : Profile
    {
        public PeopleMappingProfile()
        {
            CreateMap<PersonDomain, PersonViewModel>();

            CreateMap<FamilyMemberDomain, FamilyMembersViewModel>();
        }
    }
}
