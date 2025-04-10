using Project.DAL.Entities.Order;

namespace Project.BLL.Specifications.OrderSpecs
{
    public class OrderSpecParams
    {

        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public string? Sort { get; set; }
        public OrderStatus? Status { get; set; }
        public OrderTracking? Tracking { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower().Trim(); }
        }

    }
}
