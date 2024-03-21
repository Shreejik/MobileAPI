namespace DapperAPI.dto
{
    public class ProductSearchReq : Paging
    {
        public string ProdCriteria { get; set; }
        public int CompanyID { get; set; }
        public string DistributorCode { get; set; }
       

    }
}
