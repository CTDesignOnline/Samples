using System.ComponentModel.DataAnnotations;

using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Authorization
{
    public class AuthorizationLoginModel : ViewModelBase
    {
        public enum ErrorState
        {
            None,
            UsernameOrPasswordIncorrect,
            AccountLocked
        }

        [Required, Display( Name = "Email" )]
        [EmailAddress]
        public string Username { get; set; }

        [Required, Display( Name = "Password" ), DataType( DataType.Password )]
        public string Password { get; set; }

        [Display( Name = "Remember me" )]
        public bool RememberMe { get; set; }

        [ScaffoldColumn( false )]
        public string ReturnUrl { get; set; }
    }
}
