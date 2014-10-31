(function (API, undefined) {

    // Provinces
    API.Provinces = function () {

        var self = this;

        // Get a list of provinces.
        this.get = function () {
            var url = checkout.API.URL_CONSTANTS.PROVINCES.GET;
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

        // Get a list of provinces.
        this.getAll = function () {
            var url = checkout.API.URL_CONSTANTS.PROVINCES.GET_ALL;
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