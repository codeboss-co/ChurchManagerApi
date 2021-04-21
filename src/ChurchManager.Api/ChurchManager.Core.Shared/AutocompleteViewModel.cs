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
    
    public record PeopleAutocompleteViewModel(int Id, string Label, string PhotoUrl, string ConnectionStatus) : AutocompleteViewModel(Id, Label) { }
    public class PeopleAutocompleteResults : List<PeopleAutocompleteViewModel>
    {
        public PeopleAutocompleteResults(IEnumerable<PeopleAutocompleteViewModel> collection) : base(collection)
        {
        }
    }
}
