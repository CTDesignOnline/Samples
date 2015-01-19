using System.Web;

namespace BrambleBerry.Kitchen.Routes.Logic
{
    internal class NotFoundHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = 404;
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}