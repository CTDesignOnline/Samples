using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace BrambleBerry.Kitchen.Services.AbleCommerce
{
    class AbleCommerceLoginService : IAbleCommerceLegacyLogin
    {
        public AbleCommerceLegacyLoginModel HandleLogin(string username, string password)
        {
            // #TODO Do Able Commerce REST call here
                // Make the call to Able Commerce
                // Pass the response into ParseXmlResponse
                // return the Model

            // For now however just returned a canned response!
            var isUsernameRecognised = false;
            var isPasswordRecognised = false;
            return new AbleCommerceLegacyLoginModel( username, password, 1234, "Pete", "Duncanson", isUsernameRecognised, isPasswordRecognised );
        }

        /// <summary>
        /// Parses the passed in XML response from the Able Commerce Legacy Login service/api and returns a nice clean Model. It will throw an error if the XML is invalid!
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static AbleCommerceLegacyLoginModel ParseXmlResponse( string xml )
        {
            var doc = new XmlDocument();
            AbleCommerceLegacyLoginModel model = null;

            try
            {
                doc.LoadXml(xml);
                var root = doc.DocumentElement;
                var username = root.GetSafeValueAsStringByXPath("UserName");
                var password = root.GetSafeValueAsStringByXPath( "Password" );
                var firstname = root.GetSafeValueAsStringByXPath( "FirstName" );
                var surname = root.GetSafeValueAsStringByXPath( "Surname" );
                var customerId = root.GetSafeValueAsIntByXPath( "CustomerId", -1 );
                var isPasswordOk = root.GetSafeValueAsStringByXPath( "PasswordStatus", "false" ).ToLower() == "true";
                var isUsernameOk = root.GetSafeValueAsStringByXPath( "UserNameStatus", "false" ).ToLower() == "true";
                model = new AbleCommerceLegacyLoginModel(username, password, customerId, firstname, surname, isUsernameOk, isPasswordOk);
            }
            catch (Exception exc)
            {
                // #TODO would be nice to log this somewhere but the caller could do that
                throw new InvalidDataException( "Xml response from Able Commerce API was invalid or missing" );
            }

            return model;
        }
    }
}
