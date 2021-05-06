using AutoMapper;
using ChurchManager.Application.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Model;

namespace ChurchManager.Application.Mappings
{
    public class ChurchesMappingProfile : Profile
    {
        public ChurchesMappingProfile()
        {
            CreateMap<Church, ChurchViewModel>().ReverseMap();
            CreateMap<ChurchDomain, ChurchViewModel>();
        }
    }
}
