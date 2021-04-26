using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public record GeneralViewModel : SelectItemViewModel
    {
        public string Description { get; set; }
    }

    public class GeneralViewModels : List<GeneralViewModel>
    {
        public GeneralViewModels(IEnumerable<GeneralViewModel> initial): base(initial)
        {
        }
    }
}
