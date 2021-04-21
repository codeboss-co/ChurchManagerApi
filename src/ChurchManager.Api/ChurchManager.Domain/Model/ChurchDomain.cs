using ChurchManager.Persistence.Models.Churches;

namespace ChurchManager.Domain.Model
{
    public class ChurchDomain
    {
        private readonly Church _entity;

        public int Id => _entity.Id;
        public int? LeaderPersonId => _entity.LeaderPersonId;
        public string Name => _entity.Name;
        public string Description => _entity.Description;
        public string ShortCode => _entity.ShortCode;
        public string PhoneNumber => _entity.PhoneNumber;
        public string Address => _entity.Address;

        public ChurchDomain(Church entity) => _entity = entity;
    }
}
