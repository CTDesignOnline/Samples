using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Services.ZenDesk
{
    public class ZendeskService
    {
        private static IZendeskServiceImplementation _instance;

        public static IZendeskServiceImplementation Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (ZendeskServiceConfiguration.UseLiveProvider)
                    {
                        _instance = new RealZendeskServiceImplementation();
                    }
                    else
                    {
                        _instance = new MockZendeskServiceImplementation();
                     }
                }
                return _instance;
            }
        }
    }
}
