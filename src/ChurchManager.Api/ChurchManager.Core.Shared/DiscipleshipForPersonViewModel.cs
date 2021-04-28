using System;
using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public record DiscipleshipForPersonViewModel
    {
        public DiscipleshipProgramViewModel Program { get; set; }
        public IEnumerable<DiscipleshipStepsViewModel> Steps { get; set; }
    }

    public record DiscipleshipStepsViewModel
    {
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public StepDefinitionViewModel StepDefinition { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsComplete { get; set; }
    }

    public record StepDefinitionViewModel : GeneralViewModel
    {
        public int Order { get; set; }
        public bool AllowMultiple { get; set; }
    }

    public record DiscipleshipProgramViewModel : GeneralViewModel
    {
        public int Order { get; set; }
        public string Category { get; set; }
    }
}
