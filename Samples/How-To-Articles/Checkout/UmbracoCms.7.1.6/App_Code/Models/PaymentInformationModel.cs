namespace Models
{
    using System;

    /// <summary>
    /// Summary description for PaymentInformationModel
    /// </summary>
    public class PaymentInformationModel : AddressModel
    {
        public Guid PaymentMethodKey { get; set; }
    }
}