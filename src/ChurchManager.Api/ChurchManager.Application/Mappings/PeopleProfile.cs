using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Model;

namespace ChurchManager.Application.Mappings
{
    public class PeopleProfile : Profile
    {
        public PeopleProfile()
        {
            CreateMap<PersonDomain, PersonViewModel>();
        }
    }
}
