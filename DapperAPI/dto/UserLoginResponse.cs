namespace DapperAPI.dto
{
    public class userLoginResponse
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string FirstName { get; set; }
        public string middleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string GSTNo { get; set; }
        public string RoleId { get; set; }
        public DateTime CreatedDt { get; set; }
        public string Token { get; set; }
        public string error { get; set; }
        public int sellerID { get; set; }
        public string CompanyName { get; set; }

        public string CompanyLogo { get; set; }

    }
}
