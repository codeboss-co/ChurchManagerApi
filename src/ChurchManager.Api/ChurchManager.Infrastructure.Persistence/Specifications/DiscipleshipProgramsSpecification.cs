using System.Linq;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class DiscipleshipProgramsSpecification : Specification<DiscipleshipStepDefinition>
    {
        public DiscipleshipProgramsSpecification(int? personId = null)
        {
            // Returns all the steps that the person is apart of: so we can get the program info also
            if (personId is not null)
            {
                Criteria = x => x.Steps.Any(x => x.PersonId == personId);

                Includes.Add(x => x.Steps);
                Includes.Add(x => x.DiscipleshipProgram);
            }
        }
    }
}
