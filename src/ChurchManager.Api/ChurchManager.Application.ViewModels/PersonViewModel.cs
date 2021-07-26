using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.ViewModels
{
    public record PersonViewModel
    {
        public int PersonId { get; set; }
        public int? FamilyId { get; set; }
        public ChurchViewModel Church { get; set; }
        public string ConnectionStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public FullName FullName { get; set; }
        public BirthDate BirthDate { get; set; }
        public Baptism BaptismStatus { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public Email Email { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new Collection<PhoneNumber>();
        public string CommunicationPreference { get; set; }
        public string PhotoUrl { get; set; }
        public string Occupation { get; set; }
        public string Source { get; set; }
        public bool? ReceivedHolySpirit { get; set; } = false;
        public DiscipleshipStepViewModel FoundationSchool { get; set; }
        public ICollection<PersonViewModelBasic> FamilyMembers { get; set; } = new Collection<PersonViewModelBasic>();
        public string RecordStatus { get; set; }
    }
}
