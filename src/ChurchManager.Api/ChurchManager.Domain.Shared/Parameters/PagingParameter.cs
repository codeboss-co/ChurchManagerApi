namespace ChurchManager.Domain.Shared.Parameters
{
    public record PagingParameter
    {
        const int MaxPageSize = 200;

        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        public int Results
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
