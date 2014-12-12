(function (app) {

    // paymentMethodService
    var paymentMethodService = function ($http) {

        var paymentMethodFactory = {};

        // Gets the payment methods available (via API).
        paymentMethodFactory.getPaymentMethods = function () {
            var paymentMethodApi = new checkout.API.PaymentMethods();
            var api = paymentMethodApi.get();
            return $http.get(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        return paymentMethodFactory;
    };

    app.factory("paymentMethodService", paymentMethodService);

}(angular.module(appName)));