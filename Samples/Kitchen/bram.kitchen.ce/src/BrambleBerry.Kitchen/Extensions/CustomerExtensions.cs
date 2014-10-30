using Merchello.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace BrambleBerry.Kitchen.Extensions
{
    public static class CustomerExtensions
    {
        public static IMember GetUmbracoMember(this ICustomer member, IMemberService memberService = null)
        {
            if (memberService == null)
            {
                memberService = ApplicationContext.Current.Services.MemberService;
            }
            
            return memberService.GetByUsername(member.LoginName);
        }
    }
}