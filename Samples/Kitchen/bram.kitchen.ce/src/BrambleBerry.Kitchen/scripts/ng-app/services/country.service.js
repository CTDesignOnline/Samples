(function (app) {

    // countryService
    var countryService = function ($http) {

        var countryFactory = {};

        // Gets just the countries that can be shipped to (via API).
        countryFactory.getCountries = function () {
            var countryApi = new checkout.API.Countries();
            var api = countryApi.get();
            return $http.get(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        // Gets all the countries (via API).
        countryFactory.getAllCountries = function () {
            var countryApi = new checkout.API.Countries();
            var api = countryApi.getAll();
            return $http.get(api.url, api.request).then(function (response) {
                return api.response(response.data);
            });
        };

        return countryFactory;
    };

    app.factory("countryService", countryService);

}(angular.module(appName)))