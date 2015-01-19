using System.ComponentModel.DataAnnotations;

using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Authorization
{
    public class AuthorizationForgotPasswordFormModel : ViewModelBase
    {
        [Required, Display( Name = "Email Address" )]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
