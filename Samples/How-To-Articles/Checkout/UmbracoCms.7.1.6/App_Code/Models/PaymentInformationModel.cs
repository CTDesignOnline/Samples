using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{

    /// <summary>
    /// Summary description for PaymentInformationModel
    /// </summary>
    public class PaymentInformationModel : AddressModel
    {
        public Guid PaymentMethodKey { get; set; }
    }
}