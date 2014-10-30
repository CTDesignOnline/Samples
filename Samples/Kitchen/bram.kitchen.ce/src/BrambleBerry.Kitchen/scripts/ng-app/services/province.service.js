(function (app) {

    // provinceService
    var provinceService = function ($http) {

        var provinceFactory = {};

        // Gets a list of shipping provinces (via API).
        provinceFactory.getProvinces = function () {
            var provinceApi = new checkout.API.Provinces();
            var api = provinceApi.get();
            return $http.get(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        // Gets a list of all provinces (via API).
        provinceFactory.getAllProvinces = function () {
            var provinceApi = new checkout.API.Provinces();
            var api = provinceApi.getAll();
            return $http.get(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        return provinceFactory;
    };

    app.factory("provinceService", provinceService);

}(angular.module(appName)));