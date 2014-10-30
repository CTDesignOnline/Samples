using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace BrambleBerry.Kitchen.Extensions
{
    public static class MemberExtensions
    {
        public static ValidatePasswordResetTokenResult ValidatePasswordResetToken(this IMember member, Guid token, TimeSpan? validFor = null)
        {
            if (validFor == null)
            {
                validFor = new TimeSpan(1, 0, 0);
            }
            var tokenStr = member.GetValue<string>("forgottenPasswordToken");
            if (!string.IsNullOrEmpty(tokenStr))
            {
                var realGuid = Guid.Parse(tokenStr);

                if (realGuid.Equals(token))
                {
                    //token is correct, lets verify if its expired
                    var dateTimeStr = member.GetValue<string>("forgottenPasswordTokenGenerated");
                    if (!string.IsNullOrEmpty(tokenStr))
                    {
                        var tokenGeneratedAt = DateTime.Parse(dateTimeStr);

                        if (DateTime.UtcNow < tokenGeneratedAt.Add(validFor.Value))
                        {
                            return ValidatePasswordResetTokenResult.Valid;
                        }
                        else
                        {
                            return ValidatePasswordResetTokenResult.TokenExpired;
                        }
                    }
                }

            }
            return ValidatePasswordResetTokenResult.IncorrectToken;
        }

     

        public enum ValidatePasswordResetTokenResult
        {
            Valid,
            TokenExpired,
            IncorrectToken,

        }
    }
}
