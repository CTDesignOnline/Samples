(function (API, undefined) {

    // Countries
    API.Countries = function () {

        var self = this;

        // Get a list of countries that are shipped to.
        this.get = function () {
            var url = checkout.API.URL_CONSTANTS.COUNTRIES.GET;
            var request = {};
            var output;
            var response = function (apiResponse) {
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

        // Get a list of all countries.
        this.getAll = function () {
            var url = checkout.API.URL_CONSTANTS.COUNTRIES.GET_ALL;
            var request = {};
            var output;
            var response = function (apiResponse) {
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