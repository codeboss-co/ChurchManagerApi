using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public record AutocompleteViewModel(int Id, string Label) { }

    public class AutocompleteResults : List<AutocompleteViewModel>
    {
        public AutocompleteResults(IEnumerable<AutocompleteViewModel> collection): base(collection)
        {
        }
    }
}
