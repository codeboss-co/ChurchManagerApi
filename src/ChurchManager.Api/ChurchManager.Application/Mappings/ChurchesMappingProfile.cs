using AutoMapper;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Churches;

namespace ChurchManager.Application.Mappings
{
    public class ChurchesMappingProfile : Profile
    {
        public ChurchesMappingProfile()
        {
            CreateMap<Church, ChurchViewModel>().ReverseMap();
        }
    }
}
