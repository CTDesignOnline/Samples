// Models for the Checkout controller.
(function (models, undefined) {

    // Address
    models.Address = function (addressSource) {

        var self = this;

        if (addressSource === undefined) {
            self.address1 = "";
            self.address2 = "";
            self.city = "";
            self.countryCode = "";
            self.customerKey = "";
            self.email = "";
            self.isCommercial = false;
            self.fullname = "";
            self.organization = "";
            self.phone = "";
            self.postalCode = "";
            self.region = "";
        } else {
            self.address1 = addressSource.address1;
            self.address2 = addressSource.address2;
            self.city = addressSource.city;
            self.countryCode = addressSource.countryCode;
            self.customerKey = addressSource.customerKey;
            self.email = addressSource.email;
            if (addressSource.isCommercial !== undefined) {
                self.isCommercial = addressSource.isCommercial;
            } else {
                self.isCommercial = false;
            }
            self.fullname = addressSource.fullname;
            self.organization = addressSource.organization;
            self.phone = addressSource.phone;
            self.postalCode = addressSource.postalCode;
            self.region = addressSource.region;
        };

    };

    // Contact Info
    models.ContactInfo = function (contactInfoSource) {

        var self = this;

        if (contactInfoSource === undefined) {
            self.firstName = "";
            self.lastName = "";
            self.email = "";
            self.phone = "";
        } else {
            self.firstName = contactInfoSource.firstName;
            self.lastName = contactInfoSource.lastName;
            self.email = contactInfoSource.email;
            self.phone = contactInfoSource.phone;
        }

    };

    // Payment Args
    models.PaymentArg = function (paymentArgSource) {

        var self = this;

        if (paymentArgSource === undefined) {
            self.key = "";
            self.value = "";
        } else {
            self.key = paymentArgSource.key;
            self.value = paymentArgSource.value;
        }
    };

    // PreOrderSummary
    models.PreOrderSummary = function (preOrderSource) {

        var self = this;

        if (preOrderSource === undefined) {
            self.itemTotal = 0;
            self.shippingTotal = 0;
            self.taxTotal = 0;
            self.invoiceTotal = 0;
        } else {
            self.itemTotal = preOrderSource.itemTotal;
            self.shippingtotal = preOrderSource.shippingTotal;
            self.taxTotal = preOrderSource.taxTotal;
            self.invoiceTotal = preOrderSource.invoiceTotal;
        }
    };

    // Sales Prep Model
    models.SalesPrep = function (salesPrepSource) {

        var self = this;

        if (salesPrepSource === undefined) {
            self.contactInfo = new checkout.Models.ContactInfo();
            self.shippingAddress = new checkout.Models.Address();
            self.billingAddress = new checkout.Models.Address();
            self.paymentMethodKey = null;
            self.paymentArgs = [];
            self.shipMethodKey = null;
        } else {
            self.contactInfo = new checkout.Models.ContactInfo(salesPrepSource.contactInfo);
            self.shippingAddress = new checkout.Models.Address(salesPrepSource.shippingAddress);
            self.billingAddress = new checkout.Models.Address(salesPrepSource.billingAddress);
            self.paymentMethodKey = salesPrepSource.paymentMethodKey;
            self.paymentArgs = _.map(salesPrepSource.paymentArgs, function (attributes) {
                return new checkout.Models.PaymentArg(attributes);
            });
            self.shipMethodKey = salesPrepSource.shipMethodKey;
        }
    };

}(window.checkout.Models = window.checkout.Models || {}));