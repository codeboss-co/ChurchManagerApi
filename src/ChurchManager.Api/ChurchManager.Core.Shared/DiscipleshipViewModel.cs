using System;

namespace ChurchManager.Core.Shared
{

    public record DiscipleshipStepViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
    }
}
