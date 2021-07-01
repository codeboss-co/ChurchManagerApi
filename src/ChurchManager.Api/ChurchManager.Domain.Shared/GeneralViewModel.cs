using System.Collections.Generic;

namespace ChurchManager.Domain.Shared
{
    public record GeneralViewModel : SelectItemViewModel;

    public class GeneralViewModels : List<GeneralViewModel>
    {
        public GeneralViewModels(IEnumerable<GeneralViewModel> initial): base(initial)
        {
        }
    }
}
