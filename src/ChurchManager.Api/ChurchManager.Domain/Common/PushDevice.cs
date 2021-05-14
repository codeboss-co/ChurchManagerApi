using ChurchManager.Domain.Features.People;
using Codeboss.Types;

namespace ChurchManager.Domain.Common
{
    public record PushDevice : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }

        public int PersonId { get; set; }

        #region Navigation

        public virtual Person Person { get; set; }

        #endregion
    }
}
