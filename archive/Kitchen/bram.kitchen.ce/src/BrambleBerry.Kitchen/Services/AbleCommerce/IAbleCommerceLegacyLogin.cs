using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Services.AbleCommerce
{
    public interface IAbleCommerceLegacyLogin
    {
        /// <summary>
        /// Check if the supplied details are recognised as a valid user login attempt
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AbleCommerceLegacyLoginModel HandleLogin(string username, string password);
    }
}
