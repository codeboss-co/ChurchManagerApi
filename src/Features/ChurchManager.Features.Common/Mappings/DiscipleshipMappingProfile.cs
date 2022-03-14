using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Common.Mappings
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