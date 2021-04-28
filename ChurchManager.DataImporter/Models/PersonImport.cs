using System;
using ChurchManager.Persistence.Models.People;

namespace ChurchManager.DataImporter.Models
{
    public record PersonImport
    {
        public string FamilyName { get; set; }
        public FullName FullName { get; set; }
        public string ConnectionStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public BirthDate BirthDate { get; set; }
        public string Source { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public Baptism Baptism { get; set; }
        public FoundationSchool FoundationSchool { get; set; }
        public bool? ReceivedHolySpirit { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CommunicationPreference { get; set; }
        public string PhotoUrl { get; set; }
        public string Occupation { get; set; }
        public string ChurchName { get; set; }
        public string CellGroupName { get; set; }
        public string CellGroupRole { get; set; }
    }

    public record FoundationSchool
    {
        public bool? IsComplete { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
