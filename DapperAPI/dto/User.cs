namespace DapperAPI.dto
{
    public class User
    {
        public int userid { get; set; }

        public string username { get; set; }

        public string password { get; set; }
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

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime UpdatedDt { get; set; }

    }
}
