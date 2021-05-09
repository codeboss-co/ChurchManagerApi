using AutoMapper;
using ChurchManager.Application.Abstractions;
using ChurchManager.Domain.Features.Discipleship;

namespace ChurchManager.Application.Mappings
{
    public class DiscipleshipMappingProfile : Profile
    {
        public DiscipleshipMappingProfile()
        {
            CreateMap<DiscipleshipProgram, GeneralViewModel>().ReverseMap();
            CreateMap<DiscipleshipStepDefinition, GeneralViewModel>().ReverseMap();
        }
    }
}
