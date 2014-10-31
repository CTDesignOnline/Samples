using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrambleBerry.Kitchen.Services.AbleCommerce;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace BrambleBerry.Kitchen.Extensions
{
    public static class AbleCommerceLegacyLoginModelExtensions
    {
        /// <summary>
        /// Helper to convert an Able Commerece user into an Umbraco member, no Services required for ease of testing
        /// </summary>
        /// <param name="legacyAccount">The Able Commerce customer model</param>
        /// <param name="umbracoMember">A temp or dummy Umbraco member that we can flesh out</param>
        /// <returns>A valid Umbraco member (not persisted) or null</returns>
        public static IMember ConvertToUmbracoMember(AbleCommerceLegacyLoginModel legacyAccount, IMember umbracoMember )
        {
            IMember customer = null;

            if (legacyAccount != null)
            {
                if (legacyAccount.IsValid)
                {
                    var memberDisplayName = String.IsNullOrEmpty(legacyAccount.FirstName)
                        ? legacyAccount.Username
                        : legacyAccount.Surname + ", " + legacyAccount.FirstName;

                    umbracoMember.Name = memberDisplayName;
                    umbracoMember.Username = legacyAccount.Username;
                    umbracoMember.Email = legacyAccount.Username;

                    umbracoMember.SetValue( "ableCommerceId", legacyAccount.CustomerId );
                    umbracoMember.SetValue( "firstname", legacyAccount.FirstName );
                    umbracoMember.SetValue( "surname", legacyAccount.Surname );
                    umbracoMember.IsApproved = true;
                    customer = umbracoMember;
                }
            }

            return customer;
        }

        public static string GetPasswordToSaveUmbracoUserWith(AbleCommerceLegacyLoginModel legacyAccount)
        {
            // If password does not match assume user has forgot, import the user and set password to something temp, 
            // then we will ask the user to reset their password somehow?
            return legacyAccount.IsPasswordRecognised ? legacyAccount.Password : Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Converts an Able Commerce user into an Umbraco member if valid, NOTE it DOES NOT persist the user, you need to do that with a .Save()
        /// </summary>
        /// <param name="legacyAccount"></param>
        /// <param name="services"></param>
        /// <returns>A valid IMember or null if there was an issue</returns>
        public static IMember ConvertToUmbracoMember(this AbleCommerceLegacyLoginModel legacyAccount, ServiceContext services)
        {
            // Create a dummy umbraco member but don't persist it. Bit long winded but do for ease of testing
            IMember tempUmbracoMember = services.MemberService.CreateMember(legacyAccount.Username, legacyAccount.Username, legacyAccount.Username, SiteConstants.MemberTypes.CustomerAlias);
            IMember customer = ConvertToUmbracoMember( legacyAccount, tempUmbracoMember );

            if ( customer != null)
            {
                // Valid customer so lets save their password, note this still does not save the member to Umbraco! Remember to do an actual "Save" when calling this one
                services.MemberService.SavePassword(customer, GetPasswordToSaveUmbracoUserWith( legacyAccount ));
            }

            return customer;
        }
    }
}
