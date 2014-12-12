(function (API, undefined) {

    //PaymentMethods
    API.PaymentMethods = function () {

        var self = this;

        // Get a list of payment methods.
        this.get = function () {
            var url = checkout.API.URL_CONSTANTS.PAYMENT_METHODS.GET;
            var request = {};
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
    };

}(window.checkout.API = window.checkout.API || {}));