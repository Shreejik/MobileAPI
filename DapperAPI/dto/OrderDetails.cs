namespace DapperAPI.dto
{
    public class OrderDetails : OrderSearchResp
    {
        public IEnumerable<OrderItems> items { get; set; }

    }
}
