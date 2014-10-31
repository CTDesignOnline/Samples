namespace AbleMods.Api.Models
{
    public class UAddress
    {
        public string NickName { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Countrycode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool Residence { get; set; }
        public bool IsBilling { get; set; }
    }
}