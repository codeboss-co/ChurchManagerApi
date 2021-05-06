using AutoMapper;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;

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
