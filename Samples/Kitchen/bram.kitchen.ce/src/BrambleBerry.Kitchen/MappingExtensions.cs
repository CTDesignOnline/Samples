namespace BrambleBerry.Kitchen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;

    using BrambleBerry.Kitchen.Models;
    using BrambleBerry.Kitchen.Models.Account.MyOrders;
    using BrambleBerry.Kitchen.Models.Checkout;
    using BrambleBerry.Kitchen.Models.Workflow;

    using Buzz.Hybrid;

    using Merchello.Core.Gateways.Payment;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Models;
    using Merchello.Web.Models.ContentEditing;
    using Merchello.Web.Workflow;

    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// Site specific utility extension methods for mapping domain models to view models
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed. Suppression is OK here.")]
    internal static class MappingExtensions
    {
        #region Address

        /// <summary>
        /// Maps an <see cref="AddressModel"/> to <see cref="IAddress"/>, handy for Shipping an Billing. No customer key so no way to link back to customer
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="IAddress"/>.
        /// </returns>
        public static IAddress ToAddress(this AddressModel address)
        {
            return new Address()
            {
                Address1 = address.Address1,
                Address2 = address.Address2,
                CountryCode = address.CountryCode,
                Email = address.Email,
                IsCommercial = address.IsCommercial,
                Locality = address.City,
                Name = address.Fullname,
//                Organization = address.Organization,
                Phone = address.Phone,
                PostalCode = address.PostalCode,
                Region = address.Region,
            };
        }

/// <summary>
        /// Maps an <see cref="CheckoutAddress"/> to <see cref="IAddress"/>, handy for Shipping an Billing. No customer key so no way to link back to customer
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="IAddress"/>.
        /// </returns>
        public static IAddress ToAddress(this CheckoutAddress address)
        {
            return new Address()
            {
                Address1 = address.Address1,
                Address2 = address.Address2,
                CountryCode = address.CountryCode,
                Email = address.Email,
                IsCommercial = address.IsCommercial,
                Locality = address.City,
                Name = address.Fullname,
                Organization = address.Organization,
                Phone = address.Phone,
                PostalCode = address.PostalCode,
                Region = address.Region
            };
        }
		
        public static ICustomerAddress ToCustomerAddress( this AddressModel address )
        {
            return new CustomerAddress(address.CustomerKey)
            {
                Address1 = address.Address1,
                Address2 = address.Address2,
                AddressType = address.TypeOfAddress,
                CountryCode = address.CountryCode,
                FullName = address.Fullname,
                IsDefault = address.IsDefaultAddress,
                Label = address.Alias,
                Locality = address.City,
                Phone = address.Phone,
                PostalCode = address.PostalCode,
                Region = address.Region,
                Company = ""
            };
        }

        /// <summary>
        /// Maps an <see cref="ICustomerAddress"/> to <see cref="AddressModel"/>
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="AddressModel"/>.
        /// </returns>
        public static AddressModel ToAddress( this ICustomerAddress address )
        {
            return new AddressModel( address.CustomerKey, address.Key )
            {
                Address1 = address.Address1,
                Address2 = address.Address2,
                CountryCode = address.CountryCode,
                // Email = address.Email,
                //AddressUsageRole = address.Label.IsCommercial ? AddressModel.AddressRole.Business : AddressModel.AddressRole.Residential,
                Alias = address.Label,
                City = address.Locality,
                Fullname = address.FullName,
                Phone = address.Phone,
                PostalCode = address.PostalCode,
                Region = address.Region
            };
        }

        public static IEnumerable<SelectListItem> ToSelectListItems( this IEnumerable<ICountry> countries )
        {
            return countries.ToSelectListItems( String.Empty ); 
        }
        
        
        /// <summary>
        /// Converts a list of ICountry objects into a usable SelectListItems list for use in a dropdown
        /// </summary>
        /// <param name="countries"></param>
        /// <param name="selectedCountryCode">The selected country code if known</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems( this IEnumerable<ICountry> countries, string selectedCountryCode )
        {
            var items = new List<SelectListItem>();
            if (countries != null)
            {
                foreach (var country in countries)
                {
                    items.Add(new SelectListItem()
                    {
                        Selected = country.CountryCode == selectedCountryCode,
                        Text = country.Name,
                        Value = country.CountryCode
                    });
                }
            }
            return items;
        }

        
        #endregion

        #region Basket

        /// <summary>
        /// Maps a <see cref="IBasket"/> to a <see cref="BasketFormModel"/>
        /// </summary>
        /// <param name="basket">
        /// The basket.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="emptyBasketText">
        /// The empty basket text
        /// </param>
        /// <returns>
        /// The <see cref="BasketFormModel"/>.
        /// </returns>
        public static BasketFormModel ToBasketFormModel(this IBasket basket, UmbracoHelper umbraco, string emptyBasketText = "")
        {
            return new BasketFormModel()
            {
                EmptyBasketText = emptyBasketText,
                TotalPrice = basket.TotalBasketPrice,
                Items = basket.Items.Select(x => x.ToBasketLineItem(umbraco)).OrderBy(x => x.Name).ToArray()
            };
        }

        /// <summary>
        /// Maps a <see cref="ILineItem"/> to a <see cref="BasketLineItem"/>.
        /// </summary>
        /// <param name="lineItem">
        /// The line item.
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <returns>
        /// The <see cref="BasketLineItem"/>.
        /// </returns>
        internal static BasketLineItem ToBasketLineItem(this ILineItem lineItem, UmbracoHelper umbraco)
        {
            var contentId = lineItem.ExtendedData.GetValueAsInt("umbracoContentId");

            IPublishedContent content = null;

            if (contentId > 0)
            {
                content = umbraco.TypedContent(contentId);
            }

            return new BasketLineItem()
            {
                Key = lineItem.Key,
                ContentId = contentId,
                Thumbnail = content != null ? content.GetSafeImage(umbraco, "images") : null,
                Name = content != null ? content.Name : string.Empty,
                VariantName = lineItem.Name,
                Sku = lineItem.Sku,
                UnitPrice = lineItem.Price,
                TotalPrice = lineItem.TotalPrice,
                Quantity = lineItem.Quantity,
                Url = content != null ? content.Url : string.Empty,
                IsVariant = lineItem.ExtendedData.GetValueAsBool("isVariant")
            };
        }

        #endregion

        #region Product

        /// <summary>
        /// Maps <see cref="IPublishedContent"/> to a product list item.
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/>
        /// </param>
        /// <param name="umbraco">
        /// The <see cref="UmbracoHelper"/>
        /// </param>
        /// <param name="product">
        /// The <see cref="ProductDisplay"/> representation of the Merchello product
        /// </param>
        /// <returns>
        /// The <see cref="ProductListItem"/>.
        /// </returns>        
        public static ProductListItem ToProductListItem(this IPublishedContent content, UmbracoHelper umbraco, ProductDisplay product)
        {
            // This always for a content item (Product) to exist without a Merchello product associated.
            if (product == null)
            {
                product = new ProductDisplay() { ProductVariants = new List<ProductVariantDisplay>(), Key = Guid.Empty, Price = 0 };
            }
                                
            return new ProductListItem()
                {
                    Name = content.GetSafeString("headline", content.Name),
                    Description = content.GetSafeHtmlString("bodyText"),
                    Thumbnail = content.GetSafeImage(umbraco, "images"),
                    Images = content.GetSafeImages(umbraco, "images", null),
                    ContentId = content.Id,
                    Url = content.Url,
                    HasVariants = product.ProductVariants.Any(),
                    Price = product.Price,
                    ProductKey = product.Key,
                    Options = product.ProductOptions,
                    AddItemFormModel = new AddItemFormModel()
                                           {
                                               ContentId = content.Id,
                                               ProductKey = product.Key,
                                               Product = product,
                                               Quantity = 1
                                           }
                };
        }

        #endregion

        #region Orders
        public static OrderModel ToOrderViewModel(this IOrder order, IInvoice invoice)
        {
            return new OrderModel(order.Key)
            {
                CreatedOn = order.CreateDate,
                Items = order.Items.ToOrderItems(),
                BillingAddress = invoice.GetBillingAddress(),
                State = order.OrderStatus.ToOrderStatusEnum()

            };
            
        }

        public static OrderModel ToOrderViewModel(this OrderDisplay order, InvoiceDisplay invoice)
        {
            return new OrderModel(order.Key) { };
        }

        public static IEnumerable<Models.Account.MyOrders.OrderLineItem> ToOrderItems(this LineItemCollection orderLineItems)
        {
            return orderLineItems.Select(item => new Models.Account.MyOrders.OrderLineItem( item.Key ) { 
                Sku = item.Sku,
                DateCreated = item.CreateDate,
                Price = item.Price,
                TotalPrice = item.TotalPrice,
                Quantity = item.Quantity,
                IsShippable = item.IsShippable(),
                Name = item.Name,
            });
        }

        public static OrderModel.OrderState ToOrderStatusEnum(this IOrderStatus orderStatus)
        {
            // TODO this is going to need to be refactored once ShipmentStatus is introduced.

            switch (orderStatus.Alias)
            {
                case "notfulfilled":
                    return OrderModel.OrderState.NotFulfilled;
                case "backOrder":
                    return OrderModel.OrderState.BackOrder;
                case "cancelled":
                    return OrderModel.OrderState.Cancelled;
                case "fulfilled":
                    return OrderModel.OrderState.Fulfilled;
                default:
                    throw new Exception("Unknown state:"+orderStatus.Alias);
            }
        }
        
        #endregion

        #region Shipping

        /// <summary>
        /// The to ship method quotes.
        /// </summary>
        /// <param name="shipmentRateQuotes">
        /// The shipment rate quotes.
        /// </param>
        /// <returns>
        /// The collection of <see cref="ShipMethodQuote"/>
        /// </returns>
        internal static IEnumerable<ShipMethodQuote> ToShipMethodQuotes(this IEnumerable<IShipmentRateQuote> shipmentRateQuotes)
        {
            return shipmentRateQuotes.Select(quote => new ShipMethodQuote()
            {
                Key = quote.ShipMethod.Key,
                ShippingMethodName = quote.ShipMethod.Name,
                Rate = quote.Rate
            });
        }


        /// <summary>
        /// Utility method to convert payment arguments submitted at checkout to a <see cref="ProcessorArgumentCollection"/>
        /// </summary>
        /// <param name="paymentArgs">
        /// The payment args.
        /// </param>
        /// <returns>
        /// The <see cref="ProcessorArgumentCollection"/>.
        /// </returns>
        internal static ProcessorArgumentCollection ToProcessorArgumentCollection(this IEnumerable<KeyValuePair<string, string>> paymentArgs)
        {
            var processorArgs = new ProcessorArgumentCollection();

            foreach (var item in paymentArgs)
            {
                processorArgs.Add(item.Key, item.Value);
            }

            return processorArgs;
        }

        #endregion
    }
}