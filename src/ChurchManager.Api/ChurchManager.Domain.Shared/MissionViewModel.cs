using System;

namespace ChurchManager.Domain.Shared
{
    public record MissionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // InReach, OutReach etc
        public string Category { get; set; } // ROSA, Healing Streams etc
        public string IconCssClass { get; set; } = "heroicons_solid:calendar";
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public PeopleAutocompleteViewModel Person { get; set; }
        public AutocompleteResult Church { get; set; }
        public AutocompleteResult Group { get; set; }

    }
}
