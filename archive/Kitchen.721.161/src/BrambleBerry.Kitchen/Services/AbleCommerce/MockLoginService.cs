using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Services.AbleCommerce
{
    class MockLoginService : IAbleCommerceLegacyLogin
    {
        public AbleCommerceLegacyLoginModel HandleLogin(string username, string password)
        {
            var isUsernameRecognised = true;
            var isPasswordRecognised = true;
            return new AbleCommerceLegacyLoginModel( "pete", "letmein", 1234, "Pete", "Duncanson", isUsernameRecognised, isPasswordRecognised );
        }
    }
}
