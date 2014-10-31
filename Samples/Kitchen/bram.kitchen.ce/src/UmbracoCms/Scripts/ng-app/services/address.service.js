(function (app) {

    // addressService
    var addressService = function () {

        var addressFactory = {};

        // Creates a new address object.
        addressFactory.newAddress = function (addressSource) {
            var address;
            if (addressSource === undefined) {
                address = new checkout.Models.Address();
            } else {
                address = new checkout.Models.Address(addressSource);
            }
            return address;
        };

        // Converts the provided address to an iAddress object (for use with API calls).
        addressFactory.convertToIAddress = function (customerKey, addressSource) {
            var address;
            if (addressSource === undefined) {
                address = "";
            } else {
                address = {
                    customerKey: customerKey,
                    address1: addressSource.address1,
                    address2: addressSource.address2,
                    countryCode: addressSource.countryCode,
                    email: "",
                    isCommercial: false,
                    locality: addressSource.city,
                    name: addressSource.fullname,
                    organization: "",
                    phone: "",
                    postalCode: addressSource.postalCode,
                    region: addressSource.region
                };
            }
            return address;
        };

        return addressFactory;
    };

    app.factory("addressService", addressService);

}(angular.module(appName)));