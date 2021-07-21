using System;
using System.Collections.Generic;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.ViewModels
{
    public record DiscipleshipForPersonViewModel
    {
        public DiscipleshipProgramViewModel Program { get; set; }
        public IEnumerable<DiscipleshipStepViewModel> Steps { get; set; }
    }

    public record DiscipleshipStepViewModel
    {
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public StepDefinitionViewModel StepDefinition { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsComplete { get; set; }
        public PersonViewModelBasic Person { get; set; }
    }

    public record StepDefinitionViewModel : GeneralViewModel
    {
        public int Order { get; set; }
        public bool AllowMultiple { get; set; }
        public string IconCssClass { get; set; }
    }

    public record DiscipleshipProgramViewModel : GeneralViewModel
    {
        public int Order { get; set; }
        public string Category { get; set; }
        public string IconCssClass { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
