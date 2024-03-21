namespace DapperAPI.dto
{
    public class OrdersReq
    {
        public DateTime Orderdate { get; set; }
        public int? Buyer { get; set; }

        public int? SellerID { get; set; }

        public double TotalAmount { get; set; }

        public List<OrderItemsReq>? Items { get; set; }

    }
}
