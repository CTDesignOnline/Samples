(function (API, undefined) {

    // Shipping Methods
    API.ShippingMethods = function () {

        var self = this;

        // Get shipping method quotes with provided address.
        this.getQuotes = function (iAddress) {
            var url = checkout.API.URL_CONSTANTS.SHIPPING_METHODS.GET_QUOTES;
            var request = iAddress;
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

        this.saveRate = function (salesPrep) {
            var url = checkout.API.URL_CONSTANTS.SHIPPING_METHODS.SAVE_RATE;
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

        // Save the shippingAddress
        this.saveShipping = function (salesPrep) {
            var url = checkout.API.URL_CONSTANTS.SHIPPING_METHODS.SAVE_SHIPPING;
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