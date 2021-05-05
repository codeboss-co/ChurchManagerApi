using AutoMapper;
using ChurchManager.Application.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Model.Churches;

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
