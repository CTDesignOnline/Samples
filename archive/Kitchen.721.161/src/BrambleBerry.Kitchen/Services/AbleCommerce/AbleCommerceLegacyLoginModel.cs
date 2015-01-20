using Umbraco.Core.Models.Membership;

namespace BrambleBerry.Kitchen.Services.AbleCommerce
{
    /// <summary>
    /// Represents if a user is recognised by the Legacy Able Commerce system.
    /// </summary>
    public class AbleCommerceLegacyLoginModel
    {
        public AbleCommerceLegacyLoginModel( string username, string password, int customerId, string firstname, string surname, bool usernameRecognised, bool passwordRecognised )
        {
            Username = username;
            Password = password;
            CustomerId = customerId;
            IsPasswordRecognised = passwordRecognised;
            IsUsernameRecognised = usernameRecognised;
        }

        /// <summary>
        /// The username passed to the old customer system
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The password passed to the old customer system
        /// </summary>
        public string Password { get; private set; }

        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        /// <summary>
        /// Does the supplied <see cref="Password" /> match that in the old customer system?
        /// </summary>
        public bool IsPasswordRecognised { get; private set; }

        /// <summary>
        /// Does the supplied <see cref="Username"/> been found in the old customer system?
        /// </summary>
        public bool IsUsernameRecognised { get; private set; }

        /// <summary>
        /// The original <see cref="CustomerId" /> in Able Commerce, WARNING: if an error occurs this will be returned as -1 so check before use
        /// </summary>
        public int CustomerId { get; private set; }

        /// <summary>
        /// Has the requested details been recognised by Able as a valid user in that system (ie an old customer from the old site)
        /// </summary>
        public bool IsValid
        {
            get { 
                return CustomerId != -1 && IsUsernameRecognised; // && IsPasswordRecognised Rusty does not care for the password we can just reset it if not recognised
            }
        }
    }
}
