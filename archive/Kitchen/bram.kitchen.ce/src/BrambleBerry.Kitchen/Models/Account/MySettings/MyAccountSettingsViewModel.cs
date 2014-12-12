using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Models.Account.MySettings
{
    public class MyAccountSettingsViewModel : BaseAccountViewModel
    {
        [Required, Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required, Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required, Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
