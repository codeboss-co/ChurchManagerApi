using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Mapper;

namespace ChurchManager.Features.Churches.Infrastructure.Mapper
{
    public static class MappingExtensions
    {
        public static ChurchViewModel ToModel(this Church entity)
        {
            return entity.MapTo<Church, ChurchViewModel>();
        }
    }
}
