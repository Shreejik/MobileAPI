namespace DapperAPI.dto
{
    public class Orders
    {
        public int OrderID { get; set; }

        public int? Buyer { get; set; }

        public int Orderstatus { get; set; }

        public DateTime Orderdate { get; set; }

        public DateTime Requireddate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int? SellerID { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        public double TotalAmount { get; set; }

        public double TotalFee { get; set; }

        public double discount { get; set; }
    }
}
