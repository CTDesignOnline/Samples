(function (app) {

    // contactInfoService
    var contactInfoService = function () {

        var contactInfoFactory = {};

        // Creates a new Contact Info object.
        contactInfoFactory.newContactInfo = function (contactInfoSource) {
            var contactInfo;
            if (contactInfoSource === undefined) {
                contactInfo = new checkout.Models.ContactInfo();
            } else {
                contactInfo = new checkout.Models.ContactInfo(contactInfoSource);
            }
            return contactInfo;
        };

        return contactInfoFactory;
    };

    app.factory("contactInfoService", contactInfoService);

}(angular.module(appName)));