namespace DapperAPI.dto
{
    public class OrderItemsReq
    {
        public int Productid { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
