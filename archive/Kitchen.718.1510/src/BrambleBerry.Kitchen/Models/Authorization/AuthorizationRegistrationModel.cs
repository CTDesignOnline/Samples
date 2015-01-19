namespace BrambleBerry.Kitchen.Models.Authorization
{
    using System.ComponentModel.DataAnnotations;

    using BrambleBerry.Kitchen.Models.ViewModels;

    public class AuthorizationRegistrationModel : ViewModelBase
    {

        [Required, Display( Name = "First Name" )]
        public string Firstname { get; set; }

        [Required, Display(Name = "Last Name")]
        public string Surname { get; set; }


        //[Compare("EmailConfirmation", ErrorMessage = "Email addresses do not match")]
        [Required, EmailAddress, Display(Name = "Email Address"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required, EmailAddress, Display( Name = "Confirmation email address" )]
        //public string EmailConfirmation { get; set; }

        //[Compare("PasswordConfirmation", ErrorMessage = "Password do not match")]
        [Required, Display( Name = "Password" ), DataType( DataType.Password )]
        public string Password { get; set; }

        //[Required, Display( Name = "Confirm Password" ), DataType( DataType.Password )]
        //public string PasswordConfirmation { get; set; }

        [Display( Name = "Email Preferences" )]
        public bool OptedInToMarketed { get; set; }

        [ScaffoldColumn( false )]
        public string ReturnUrl { get; set; }
    }
}
