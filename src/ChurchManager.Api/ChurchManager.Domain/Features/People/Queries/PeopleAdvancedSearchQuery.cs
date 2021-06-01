using System.Collections.Generic;
using ChurchManager.Domain.Parameters;

namespace ChurchManager.Domain.Features.People.Queries
{
    public record PeopleAdvancedSearchQuery : SearchTermQueryParameter
    {
        public IList<string> ConnectionStatus { get; set; } = new List<string>(0);
        public IList<string> AgeClassification { get; set; } = new List<string>(0);
        public IList<string> Gender { get; set; } = new List<string>(0);
    }
}
