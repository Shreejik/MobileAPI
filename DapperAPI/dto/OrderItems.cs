namespace DapperAPI.dto
{
    public class OrderItems
    {
        public int OrderItemId { get; set; }
        public int? Orderid { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string productCode {  get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public string FactoryName { get; set; }
        public string ImgUrl { get; set; }

        public string CompanyLogo { get; set; }

        public string description { get; set; }

    }
}
