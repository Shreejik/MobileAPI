namespace DapperAPI.dto
{
    public class OrderSearchResp 
    {
        public int OrderID { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusStr { get; set; }
        public DateTime Orderdate { get; set; }
        public double TotalAmount { get; set; }
        public double Percentage { get; set; }
        public double Oldamount { get; set; }
        public string InvoiceNo { get; set; }
    }
}
