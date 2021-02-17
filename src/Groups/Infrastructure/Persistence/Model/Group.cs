
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class Group : IAggregateRoot<int>
    {
        public int Id { get; }
    }
}
