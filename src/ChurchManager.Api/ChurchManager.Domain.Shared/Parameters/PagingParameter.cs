namespace ChurchManager.Domain.Shared.Parameters
{
    public record PagingParameter
    {
        const int MaxPageSize = 200;

        public int Page
        {
            get => _page <= 0 ? 1 : _page;
            set => _page = _page <= 0 ? 1 : value;
        }

        private int _pageSize = 10;
        private int _page = 1;

        public int Results
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
