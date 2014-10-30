(function (app) {

    // orderService
    var orderService = function($http) {

        var orderFactory = {};

        // Gets the pre-sale summary (via API).
        orderFactory.getSummary = function(contactInfo, shippingAddress, billingAddress, shipMethodKey, paymentMethodKey, paymentArgs) {
            var orderApi = new checkout.API.Order();
            var api = orderApi.getSummary(contactInfo, shippingAddress, billingAddress, shipMethodKey, paymentMethodKey, paymentArgs);
            return $http.post(api.url, api.request).then(function(response) {
                return api.response(response.data);
            });
        };

        // Get the Pre-Order Summary (via API).
        orderFactory.getPreOrderSummary = function(customerKey) {
            var orderApi = new checkout.API.Order();
            var api = orderApi.getPreOrderSummary(customerKey);
            return $http.post(api.url, api.request).then(function(response) {
                return api.response(response.data);
            });
        };

        // Completes the order (via API).
        orderFactory.completeOrder = function (salesPrep) {
            var orderApi = new checkout.API.Order();
            var api = orderApi.completeOrder(salesPrep);
            return $http.post(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        orderFactory.saveBilling = function (salesPrep) {
            var orderApi = new checkout.API.Order();
            var api = orderApi.saveBilling(salesPrep);
            return $http.post(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        return orderFactory;
    };

    app.factory("orderService", orderService);

}(angular.module(appName)));