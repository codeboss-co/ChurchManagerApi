using System;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.People.Queries;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class BrowsePeopleSpecification : Specification<Person>
    {
        public BrowsePeopleSpecification(PeopleAdvancedSearchQuery query)
        {
            // Includes
            Query.Include(person => person.Church);
            Query.Include(person => person.PhoneNumbers);

            // Search Term
            if (!query.SearchTerm.IsNullOrEmpty())
            {
                Query
                    .Where(person =>
                        // Name Search
                        EF.Functions.ILike(person.FullName.FirstName, $"%{query.SearchTerm}%") ||
                        EF.Functions.ILike(person.FullName.LastName, $"%{query.SearchTerm}%"));
            }

            // Connection Status
            if (query.ConnectionStatus.Any())
            {
                Expression<Func<Person, bool>> connectionStatusCriteria = person => false;
                foreach (var connectionStatus in query.ConnectionStatus)
                {
                    Expression<Func<Person, bool>> connectionStatusFilter = g => g.ConnectionStatus== connectionStatus;
                    connectionStatusCriteria = connectionStatusCriteria.Or(connectionStatusFilter);
                }

                Query.Where(connectionStatusCriteria);
            }

            // Age Classification
            if(query.AgeClassification.Any())
            {
                Expression<Func<Person, bool>> ageClassificationCriteria = person => false;
                foreach(var ageClassification in query.AgeClassification)
                {
                    Expression<Func<Person, bool>> ageClassificationFilter = g => g.AgeClassification == ageClassification;
                    ageClassificationCriteria = ageClassificationCriteria.Or(ageClassificationFilter);
                }

                Query.Where(ageClassificationCriteria);
            }

            // Age Classification
            if(query.Gender.Any())
            {
                Expression<Func<Person, bool>> genderCriteria = person => false;
                foreach(var gender in query.Gender)
                {
                    Expression<Func<Person, bool>> genderFilter = g => g.Gender == gender;
                    genderCriteria = genderCriteria.Or(genderFilter);
                }

                Query.Where(genderCriteria);
            }

            // Age Classification
            if(query.RecordStatus.Any())
            {
                Expression<Func<Person, bool>> recordStatusCriteria = person => false;
                foreach(var status in query.RecordStatus)
                {
                    Expression<Func<Person, bool>> recordStatusFilter = g => g.RecordStatus == status;
                    recordStatusCriteria = recordStatusCriteria.Or(recordStatusFilter);
                }

                Query.Where(recordStatusCriteria);
            }

            // Baptism
            if(query.Filters.Contains("baptised") || query.Filters.Contains("notBaptised"))
            {
                Expression<Func<Person, bool>> baptismCriteria = null;

                if(query.Filters.Contains("baptised"))
                {
                    baptismCriteria = PersonCriteria.IsBaptisedFilter;
                }
                if(query.Filters.Contains("notBaptised"))
                {
                    baptismCriteria = baptismCriteria is null 
                        ? PersonCriteria.NotBaptisedFilter 
                        : baptismCriteria.Or(PersonCriteria.NotBaptisedFilter);
                }

                if (baptismCriteria is not null)
                {
                    Query.Where(baptismCriteria);
                }
            }

            // Holy Spirit
            if(query.Filters.Contains("holySpirit") || query.Filters.Contains("noHolySpirit"))
            {
                Expression<Func<Person, bool>> holySpiritCriteria = null;

                if(query.Filters.Contains("holySpirit"))
                {
                    holySpiritCriteria = PersonCriteria.HasHolySpirit;
                }
                if(query.Filters.Contains("noHolySpirit"))
                {
                    holySpiritCriteria = holySpiritCriteria is null
                        ? PersonCriteria.NotBaptisedFilter
                        : holySpiritCriteria.Or(PersonCriteria.NoHolySpirit);
                }

                if(holySpiritCriteria is not null)
                {
                    Query.Where(holySpiritCriteria);
                }
            }

            // No Photo Filter
            if(query.Filters.Contains("noPhoto"))
            {
                Query.Where(x => x.PhotoUrl == null);
            }

            // Ordering
            if (!query.OrderBy.IsNullOrEmpty())
            {
                if(query.SortOrder == "ascending" || query.SortOrder.ToLower() == "ASC".ToLower())
                {
                    Query.OrderBy(query.OrderBy);
                }
                else
                {
                    Query.OrderByDescending(query.OrderBy);
                }
            }

            var skip = (query.Page - 1) * query.Results;
            // Paging
            Query
                .Skip(skip)
                .Take(query.Results);
        }
    }

    public static class PersonCriteria
    {
       public static Expression<Func<Person, bool>> IsBaptisedFilter = x =>
            x.BaptismStatus != null &&
            x.BaptismStatus.IsBaptised.HasValue &&
            x.BaptismStatus.IsBaptised.Value;

       public static Expression<Func<Person, bool>> NotBaptisedFilter = x =>
           x.BaptismStatus == null ||
           x.BaptismStatus.IsBaptised.HasValue == false ||
           x.BaptismStatus.IsBaptised.Value == false;

       public static Expression<Func<Person, bool>> HasHolySpirit = x =>
           x.ReceivedHolySpirit != null &&
           x.ReceivedHolySpirit.HasValue && 
           x.ReceivedHolySpirit.Value;

       public static Expression<Func<Person, bool>> NoHolySpirit = x =>
           x.ReceivedHolySpirit == null ||
           x.ReceivedHolySpirit.HasValue == false ||
           x.ReceivedHolySpirit == false;
    }
}
