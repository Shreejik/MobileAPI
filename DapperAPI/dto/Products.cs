namespace DapperAPI.dto
{
    public class Products
    {
        public int product_id { get; set; }

        public string product_name { get; set; }

        public int brand_id { get; set; }

        public int category_id { get; set; }

        public short model_year { get; set; }

        public decimal Price { get; set; }

        public int factoryID { get; set; }

        public int? createdBy { get; set; }

        public int? updatedBy { get; set; }

        public DateTime? createdDt { get; set; }

        public DateTime? updatedDt { get; set; }

    }

}
