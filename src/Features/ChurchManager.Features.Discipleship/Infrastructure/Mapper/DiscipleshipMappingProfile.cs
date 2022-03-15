using AutoMapper;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Mapper;

namespace ChurchManager.Features.Discipleship.Infrastructure.Mapper
{
    public class DiscipleshipMappingProfile : Profile, IAutoMapperProfile
    {
        public DiscipleshipMappingProfile()
        {
            CreateMap<DiscipleshipProgram, GeneralViewModel>().ReverseMap();
            CreateMap<DiscipleshipStepDefinition, GeneralViewModel>().ReverseMap();
        }
        public int Order => 1;
    }
}