using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Churches;

namespace ChurchManager.Features.Common.Mappings
{
    public class ChurchesMappingProfile : Profile
    {
        public ChurchesMappingProfile()
        {
            CreateMap<Church, ChurchViewModel>().ReverseMap();
        }
    }
}