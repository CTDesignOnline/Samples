using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrambleBerry.Kitchen.HttpHandlers
{
    /// <summary>
    /// for details please see http://blog.davidebbo.com/2011/02/register-your-http-modules-at-runtime.html
    /// </summary>
    public class HttpHandlerStarter
    {
        public static void Start()
        {
            // Register our module
            HttpApplication.RegisterModule(typeof(HttpHandlers.ProtectedMediaHandler));
        }
    }

}
