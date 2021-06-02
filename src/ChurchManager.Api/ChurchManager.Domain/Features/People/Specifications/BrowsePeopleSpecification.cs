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
                Expression<Func<Person, bool>> baptismCriteria = person => false;

                if(query.Filters.Contains("baptised"))
                {
                    Query.Where(x =>
                        x.BaptismStatus != null &&
                        x.BaptismStatus.IsBaptised.HasValue &&
                        x.BaptismStatus.IsBaptised.Value);
                }
                if(query.Filters.Contains("notBaptised"))
                {
                    baptismCriteria.Or(x =>
                        x.BaptismStatus == null ||
                        x.BaptismStatus.IsBaptised.HasValue == false ||
                        x.BaptismStatus.IsBaptised.Value == false);
                }

                //Query.Where(baptismCriteria); 
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
}
