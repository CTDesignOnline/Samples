(function (app) {

    // shippingMethodService
    var shippingMethodService = function ($http) {

        var shippingMethodFactory = {};

        // Gets a list of shipping method quotes for the shipping address (via API).
        shippingMethodFactory.getQuotes = function (iAddress) {
            var shippingMethodApi = new checkout.API.ShippingMethods();
            var api = shippingMethodApi.getQuotes(iAddress);
            return $http.post(api.url, api.request).then(function (response) {
                return api.response(response.data.quotes);
            });
        };

        shippingMethodFactory.saveRate = function (salesPrep) {
            var shippingMethodApi = new checkout.API.ShippingMethods();
            var api = shippingMethodApi.saveRate(salesPrep);
            return $http.post(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        shippingMethodFactory.saveShipping = function (salesPrep) {
            var shippingMethodApi = new checkout.API.ShippingMethods();
            var api = shippingMethodApi.saveShipping(salesPrep);
            return $http.post(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        return shippingMethodFactory;
    };

    app.factory("shippingMethodService", shippingMethodService);

}(angular.module(appName)));