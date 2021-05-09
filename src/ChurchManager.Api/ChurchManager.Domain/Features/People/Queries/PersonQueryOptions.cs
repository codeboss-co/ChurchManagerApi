namespace ChurchManager.Domain.Features.People.Queries
{
    /// <summary>
    /// Options that can be configured when using some of the PersonService.Queryable(..) methods
    /// </summary>
    public class PersonQueryOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether deceased people should be included (default is false)
        /// </summary>
        public bool IncludeDeceased { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether pending people records should be included (default is true)
        /// </summary>
        public bool IncludePendingStatus { get; set; } = true;
    }
}
