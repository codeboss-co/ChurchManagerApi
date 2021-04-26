using System.Linq;
using ChurchManager.Persistence.Models.Discipleship;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class DiscipleshipProgramsSpecification : Specification<DiscipleshipProgram>
    {
        public DiscipleshipProgramsSpecification(int? personId = null)
        {
            // Returns all the programs that the person is apart of
            if (personId is not null)
            {
                Criteria = x => x.DiscipleshipSteps.Any(x => x.PersonId == personId);
                Includes.Add(x => x.DiscipleshipSteps);
            }
        }
    }
}
