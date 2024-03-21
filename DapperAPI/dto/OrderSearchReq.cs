namespace DapperAPI.dto
{
    public class OrderSearchReq :Paging
    {
        public int ID { get; set; }
        public string SellerID { get; set; }
    }
}
