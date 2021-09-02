namespace API.Helpers
{
    public class PaginationParams
    {
        private const int MaxPagesSize = 50;
        public int PageNumber { set; get; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPagesSize) ? MaxPagesSize : value;

        }
    }
}