namespace DapperAPI.dto
{
    public class OrderResp 
    {
        public int OrderID { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusStr { get; set; }
        public DateTime Orderdate { get; set; }
        public double TotalAmount { get; set; }

        public List<OrderItems> Items { get; set; }
    }
   
}
