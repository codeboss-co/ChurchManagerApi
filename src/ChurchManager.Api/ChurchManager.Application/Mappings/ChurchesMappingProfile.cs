using AutoMapper;
using ChurchManager.Application.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.Persistence.Models.Churches;

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
