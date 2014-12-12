using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrambleBerry.Kitchen.Extensions;
using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Authorization
{
    /// <summary>
    /// When a user has asked to reset their password this is the model for storing their new password reset data into
    /// </summary>
    public class AuthorizationResetPasswordFormModel : ViewModelBase
    {
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [Display( Name = "Confirm Password" )]
        [Compare( "Password", ErrorMessage = "Password do not match" )]
        public string ConfirmPassword { get; set; }

        public int MemberId { get; set; }
        public Guid ResetToken { get; set; }
        public MemberExtensions.ValidatePasswordResetTokenResult IsValid { get; set; }
    }
}
