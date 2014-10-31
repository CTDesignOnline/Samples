namespace BrambleBerry.Kitchen.Models.Workflow
{
    using System;
    using System.Collections.Generic;

    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public Guid CustomerKey { get; set; }
        
        /// <summary>
        /// Gets or sets the Firstname of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Surname of the customer
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Email address of the customer
        /// </summary>
        public string Email{ get; set; }

        /// <summary>
        /// Gets or sets the DateCreated of the customer
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the MarketingOptIn of the customer
        /// </summary>
        public bool MarketingOptIn { get; set; }

        /// <summary>
        /// Gets or sets the Addresses of the customer
        /// </summary>
        public IEnumerable<AddressModel> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the Wishlists of the customer
        /// </summary>
        public IEnumerable<WishListModel> WishLists { get; set; } 

        /// <summary>
        /// Gets or sets the DigitalGifts of the customer
        /// </summary>
    //    public IEnumerable<object> DigitalGifts { get; set; } // TODO: Implement DigitalGiftNNodle here

        /// <summary>
        /// Gets or sets the LastLoggedIn date of the customer
        /// </summary>
        public DateTime LastLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the ImportedFromOldSystem of the customer
        /// </summary>
        public bool ImportedFromOldSystem { get; set; }
    }

   
}
