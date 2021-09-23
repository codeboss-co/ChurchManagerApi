using System;
using System.Collections.Generic;

namespace ChurchManager.Domain.Shared
{
    public record FollowUpViewModel
    {
        public int Id { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Type { get; set; }
        public PeopleAutocompleteViewModel AssignedPerson { get; set; }
        public PeopleAutocompleteViewModel Person { get; set; }
        public string Severity { get; set; }
        public string Note { get; set; }
        public bool? RequiresAdditionalFollowUp { get; set; }
    }

    public class FollowUpsViewModel : List<FollowUpViewModel>
    {
        public FollowUpsViewModel(IEnumerable<FollowUpViewModel> vms)
        {
            AddRange(vms);
        }
    }
}
