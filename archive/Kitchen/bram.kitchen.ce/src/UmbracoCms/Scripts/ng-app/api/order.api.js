(function (API, undefined) {

    // Order
    API.Order = function () {

        var self = this;

        // Get a pre-sale summary for the pending order with the provided addresses, shipping method, and payment method.
        this.getSummary = function (contactInfo, shippingAddress, billingAddress, shipMethodKey, paymentMethodKey, paymentArgs) {
            var url = checkout.API.URL_CONSTANTS.ORDER.GET_SUMMARY;
            var request = {
                contactInfo: contactInfo,
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                shipMethodKey: shipMethodKey,
                paymentMethodKey: paymentMethodKey,
                paymentArgs: paymentArgs
            };
            var response = function (apiResponse) {
                var output;
                if (apiResponse) {
                    output = apiResponse;
                } else {
                    output = false;
                }
                return output;
            };
            var apiCall = {
                url: url,
                request: request,
                response: response
            };
            return apiCall;
        };

        // Complete the order with the provided shipping and billing addresses, as well as the shipping method and payment method keys.
        this.completeOrder = function (salesPrep) {
            var url = checkout.API.URL_CONSTANTS.ORDER.PLACE;
            var request = new checkout.Models.SalesPrep(salesPrep);
            var response = function (apiResponse) {
                var output;
                if (apiResponse) {
                    output = apiResponse;
                } else {
                    output = false;
                }
                return output;
            };
            var apiCall = {
                url: url,
                request: request,
                response: response
            };
            return apiCall;
        };

        // 
        this.getPreOrderSummary = function(customerKey) {
            var url = checkout.API.URL_CONSTANTS.ORDER.GET_PREORDER_SUMMARY;
            var request = {
                customerKey: customerKey
            };
            var response = function (apiResponse) {
                var output;
                if (apiResponse) {
                    output = new checkout.Models.PreOrderSummary(apiResponse);
                } else {
                    output = false;
                }
                return output;
            };
            var apiCall = {
                url: url,
                request: request,
                response: response
            };
            return apiCall;
        };

        this.saveBilling = function (salesPrep) {
            var url = checkout.API.URL_CONSTANTS.ORDER.SAVE_BILLING;
            var request = new checkout.Models.SalesPrep(salesPrep);
            var response = function (apiResponse) {
                var output;
                if (apiResponse) {
                    output = new checkout.Models.PreOrderSummary(apiResponse);
                } else {
                    output = false;
                }
                return output;
            };
            var apiCall = {
                url: url,
                request: request,
                response: response
            };
            return apiCall;
        };

    };

}(window.checkout.API = window.checkout.API || {}));