namespace AbleMods.Api.Models
{
    public class UUserStatus
    {
        public bool UserNameStatus { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool PasswordStatus { get; set; }
        public int CustomerId { get; set; }
        public bool TaxExempt { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }

        public UUserStatus()
        {
            CustomerId = -1;
        }

        public UUserStatus(bool userNameStatus, string userName, string password, bool passwordStatus, int customerId, bool taxExempt, string firstName, string lastName)
        {
            UserNameStatus = userNameStatus;
            UserName = userName;
            Password = password;
            PasswordStatus = passwordStatus;
            CustomerId = customerId;
            TaxExempt = taxExempt;
            LastName = lastName;
            FirstName = firstName;
        }

    }
}