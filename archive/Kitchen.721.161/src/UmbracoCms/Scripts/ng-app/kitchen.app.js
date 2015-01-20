// kitchen.app.js | requires AngularJs | created by Kyle Weems on March 11, 2014
//
// The checkout AngularJS app.

// The Angular App declaration & namespacing

var appName = 'checkoutApp';

(function () {
    angular.module(appName, []);
}());

(function (checkout, undefined) {

    // global namepsaces
    checkout.API = {};
    checkout.Models = {};
    checkout.Tools = {};

}(window.checkout = window.checkout || {}));

// TOOLS //////////////////////////////////////////////////////////////////////

(function(tools, undefined) {

    tools.downCaseProperties = function(object) {
        var newObject = {};
        for (var prop in object) {
            if (object.hasOwnProperty(prop)) {
                var propertyName = prop;
                var propertyValue = object[prop];
                var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
                if ((typeof propertyValue) == "object") {
                    propertyValue = checkout.Tools.downCaseProperties(propertyValue);
                }
                newObject[newPropertyName] = propertyValue;
            }
        };
        return newObject;
    };

}(window.checkout.Tools = window.checkout.Tools || {}));