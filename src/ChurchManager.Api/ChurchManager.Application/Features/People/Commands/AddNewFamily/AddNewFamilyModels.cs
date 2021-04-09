using System;

namespace ChurchManager.Application.Features.People.Commands.AddNewFamily
{
    public record BirthDate
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public record PersonBasicDetails
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AgeClassification { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public BirthDate BirthDate { get; set; }
        public bool ReceivedHolySpirit { get; set; }
    }

    public record FamilyMember
    {
        public int ChurchId { get; set; }
        public string ConnectionStatus { get; set; }
        public string Source { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public PersonBasicDetails Person { get; set; }
    }

    public record Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
    }
}
