using Microsoft.IdentityModel.Tokens;

namespace DapperAPI.dto
{
    public class ProductSearchResp
    {
        public string ProductId { get; set; }
        public string ProductName  { get; set; }
        public string Productcode { get; set; }
        public string modelYear { get; set; }
        public string price { get; set; }
        public int factoryID { get; set; }
        public string factoryName { get; set; }
        public string imgUrl { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string weight { get; set; }
        public string CompanyLogo { get; set; }
        public string quantity { get; set; }
        public string description { get; set; }

        private List<string> _Colours;

        public List<string> Colours
        {
            get
            {
                if (!color.IsNullOrEmpty())
                {
                    return color.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else return new List<string>();
                
            }
            set { _Colours = value; }
        }
    }
}
