using System;
using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Parameters;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class BrowsePeopleSpecification : Specification<Person>
    {
        public BrowsePeopleSpecification(SearchTermQueryParameter query)
        {
            Query
                .Where(person =>
                    // Name Search
                    EF.Functions.ILike(person.FullName.FirstName, $"%{query.SearchTerm}%") ||
                    EF.Functions.ILike(person.FullName.LastName, $"%{query.SearchTerm}%"))

                .Include(person => person.Church)
                .Include(person => person.PhoneNumbers);

            if (!query.OrderBy.IsNullOrEmpty())
            {
                if(query.SortOrder == "ascending" || query.SortOrder == "ASC")
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
