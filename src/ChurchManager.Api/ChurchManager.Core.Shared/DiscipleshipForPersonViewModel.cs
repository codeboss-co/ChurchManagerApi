using System;
using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public record DiscipleshipForPersonViewModel
    {
        public GeneralViewModel Program { get; set; }
        public IEnumerable<DiscipleshipStepsViewModel> Steps { get; set; }
    }

    public record DiscipleshipStepsViewModel
    {
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public StepDefinitionViewModel StepDefinition { get; set; }
        public GeneralViewModel DiscipleshipType { get; set; }
    }

    public record StepDefinitionViewModel : GeneralViewModel
    {
        public int Order { get; set; }
    }
}
