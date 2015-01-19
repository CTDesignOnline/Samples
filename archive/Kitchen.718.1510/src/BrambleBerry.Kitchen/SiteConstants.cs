namespace BrambleBerry.Kitchen
{
    /// <summary>
    /// The site constants.
    /// </summary>
    public class SiteConstants
    {

        /// <summary>
        /// Button names and ids used to ensure that the UI designer does not mistakenly change a name required by 
        /// a controller.  Generally used on form submissions.
        /// </summary>
        public static class ButtonBindings
        {
            /// <summary>
            /// Gets the "add to basket" form button name/id.
            /// </summary>
            public static string AddToBasket
            {
                get { return "AddToBasket"; }
            }

            /// <summary>
            /// Gets the "add to wishlist" form button name/id.
            /// </summary>
            public static string AddToWishlist
            {
                get { return "AddToWishlist"; }
            }
        }

        // TODO verify the hard coded Ids are set in the web.config appSettings and remove from constants

        ///// <summary>
        ///// The Umbraco Ids for the various important content nodes used through out the site
        ///// </summary>
        //public static class NodeIds
        //{
        //    /// <summary>
        //    /// The homepage.
        //    /// </summary>
        //    public const int Homepage = 1050;

        //    /// <summary>
        //    /// The my account.
        //    /// </summary>
        //    public const int MyAccount = 1116;

        //    /// <summary>
        //    /// The forgotten password form.
        //    /// </summary>
        //    public const int ForgottenPasswordForm = 9999; // TODO Set this
        //}

        /// <summary>
        /// The Umbraco Ids for the various member types available
        /// </summary>
        public static class MemberTypes
        {
            /// <summary>
            /// The customer id.
            /// </summary>
            public const int CustomerId = 1122;

            /// <summary>
            /// The customer alias.
            /// </summary>
            public const string CustomerAlias = "Customer";
        }

        /// <summary>
        /// The extended data keys.
        /// </summary>
        public static class ExtendedDataKeys
        {
            /// <summary>
            /// The anonymous customers invoices - intended to be used to store any invoices created
            /// by an anonymous customer so that if the customer logs in/signs up post sale, the invoices can be associated
            /// with the persisted customer record.
            /// </summary>
            public const string AnonymousCustomersInvoices = "bramAnonymousCustomerInvoices";

            /// <summary>
            /// Gets the extended data key used for saving Umbraco Content Id references
            /// </summary>
            public static string UmbracoContentId
            {
                get { return "umbracoContentId"; }
            }

            /// <summary>
            /// Gets the line item sort by date time.  This is used to order the basket and invoice line items
            /// to ensure their order is consistent.
            /// </summary>
            public static string LineItemSortByDateTime
            {
                get { return "bramLineItemSortByDateTime"; }
            }

            /// <summary>
            /// Gets the a key that is intended to be used to identify if the line item represents a product variant (a product with variants)
            /// rather than a single product.
            /// </summary>
            public static string IsVariant
            {
                get { return "bramIsVariant"; }
            }
        }
    }
}
