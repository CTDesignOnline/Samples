// The Checkout Controller
(function (app) {

    var checkoutController = function ($scope, addressService, countryService, paymentMethodService, orderService, provinceService, shippingMethodService) {

        // Build Payment Arguments
        $scope.buildPaymentArgs = function () {
            var args = [];
            if ($scope.visible.panels.creditCard) {
                args.push({ key: 'creditCardType', value: '' });
                args.push({ key: 'cardholderName', value: $scope.creditCard.name });
                args.push({ key: 'cardNumber', value: $scope.creditCard.number });
                args.push({ key: 'expireMonth', value: $scope.expireMonth });
                args.push({ key: 'expireYear', value: $scope.expireYear });
                args.push({ key: 'cardCode', value: $scope.creditCard.code });
                args.push({ key: 'customerIp', value: '' });
            }
            return args;
        };

        // Return sales prep model from current filled out data.
        $scope.buildPrep = function (shouldPassPaymentArgs) {
            var paymentArgs = [];
            if (shouldPassPaymentArgs) {
                paymentArgs = $scope.buildPaymentArgs();
            }
            var newSalesPrep = {
                contactInfo: new checkout.Models.ContactInfo($scope.contactInfo),
                shippingAddress: new checkout.Models.Address($scope.shippingAddress),
                billingAddress: new checkout.Models.Address($scope.billingAddress),
                paymentMethodKey: $scope.paymentMethodKey,
                paymentArgs: paymentArgs,
                shipMethodKey: $scope.shipMethodKey
            };
            newSalesPrep.shippingAddress.customerKey = $scope.customerGuid;
            newSalesPrep.billingAddress.customerKey = $scope.customerGuid;
            if ($scope.visible.panels.hideBillingAddress) {
                newSalesPrep.billingAddress = newSalesPrep.shippingAddress;
            }
            return new checkout.Models.SalesPrep(newSalesPrep);
        };

        // Validate and complete the Contact Info step
        $scope.completeContactInfo = function () {
            var validated = false;
            if ($scope.checkout.contactFirst.$valid && $scope.checkout.contactLast.$valid && $scope.checkout.contactEmail.$valid) {
                validated = true;
            }
            if (validated) {
                $scope.completed.contactInfo = true;
                $scope.visible.errors.contactInfo = false;
                $scope.visible.results.contactInfo = true;
            } else {
                $scope.visible.errors.contactInfo = true;
            }
        };

        // Validate and complete the Shipping step
        $scope.completeShipping = function () {
            var validated = false;
            if ($scope.checkout.shippingName.$valid && $scope.checkout.shippingAddress1.$valid && $scope.checkout.shippingCity.$valid && $scope.filters.shippingRegion.id != -1 && $scope.checkout.shippingPostalCode.$valid && $scope.filters.shippingCountry.id != -1) {
                validated = true;
            }
            if (validated) {
                $scope.completed.shipping = true;
                $scope.visible.errors.shipping = false;
                $scope.visible.results.shipping = true;
                $scope.getQuotes();
                var salesPrep = $scope.buildPrep();
                shippingMethodService.saveShipping(salesPrep).then(function (summary) {
                    $scope.summary = summary;
                });
            } else {
                $scope.visible.errors.shipping = true;
            }
        };

        // Validate and complete the Shipping Method step
        $scope.completeShippingMethod = function () {
            var validated = false;
            if ($scope.filters.shippingMethod.id != -1) {
                validated = true;
            }
            if (validated) {
                $scope.completed.shippingMethod = true;
                $scope.visible.errors.shippingMethod = false;
                $scope.visible.results.shippingMethod = true;
                var salesPrep = $scope.buildPrep();
                shippingMethodService.saveRate(salesPrep).then(function (summary) {
                    $scope.summary = summary;
                });
            } else {
                $scope.visible.errors.shippingMethod = true;
            }
        };

        // Complete and Validate the Payment Step
        $scope.completePayment = function () {
            var validated = false;
            if ($scope.checkout.billingName.$valid && $scope.checkout.billingAddress1.$valid && $scope.checkout.billingCity.$valid && $scope.checkout.billingPostalCode.$valid && $scope.filters.paymentMethod.id != -1) {
                validated = true;
                if (!$scope.visible.panels.hideBillingAddress) {
                    if ($scope.filters.billingRegion.id == -1 || $scope.filters.billingCountry.id == -1) {
                        validated = false;
                    }
                }
                if ($scope.visible.panels.creditCard) {
                    if ($scope.creditCard.name.$invalid || $scope.creditCard.number.$invalid || $scope.expireMonth == -1 || $scope.expireYear == -1 || $scope.creditCard.code.$invalid) {
                        validated = false;
                    }
                }
            }
            if (validated) {
                $scope.completed.payment = true;
                $scope.visible.errors.payment = false;
                $scope.visible.results.payment = true;
                var salesPrep = $scope.buildPrep();
                orderService.saveBilling(salesPrep).then(function (summary) {
                    $scope.summary = summary;
                });
            } else {
                $scope.visible.errors.payment = true;
            }
        };

        // Complete the order.
        $scope.completeOrder = function () {
            var salesPrep = $scope.buildPrep(true);
            orderService.completeOrder(salesPrep).then(function (response) {
                if (response.redirect) {
                    window.location = response.redirect;
                } else if (response.message) {
                    var errors = response.message.split('|');
                    $scope.processError(errors[3]);
                }
            });
        };

        // As shipping address is being changed, must reset the shipping methods to avoid conflict.
        $scope.editShipping = function () {
            $scope.visible.results.shipping = false;
            $scope.completed.shipping = false;
            $scope.completed.shippingMethod = false;
            $scope.visible.results.shippingMethod = false;
            $scope.shippingMethod = false;
            $scope.shipMethodKey = "";
            $scope.shippingCost = 0;
            $scope.filters.shippingMethod = $scope.options.shippingMethods[0];
            for (var i = 0; i < ($scope.options.shippingMethods.length - 1) ; i++) {
                $scope.options.shippingMethods.pop();
            };
        };

        // Get all the countries via API for the purposes of the shipping dropdown.
        $scope.getCountries = function () {
            countryService.getCountries().then(function (response) {
                var countries = [];
                for (var i = 0; i < response.length; i++) {
                    countries.push(checkout.Tools.downCaseProperties(response[i]));
                };
                $scope.countries = countries;
                for (var i = 0; i < countries.length; i++) {
                    var newCountry = {
                        id: i,
                        name: countries[i].name
                    };
                    $scope.options.shippingCountries.push(newCountry);
                };
            });
        };

        // Get all the countries for the purposes of billing dropdown.
        $scope.getAllCountries = function () {
            countryService.getAllCountries().then(function (response) {
                var countries = [];
                for (var i = 0; i < response.length; i++) {
                    countries.push(checkout.Tools.downCaseProperties(response[i]));
                };
                $scope.billingCountries = countries;
                for (var i = 0; i < countries.length; i++) {
                    var newCountry = {
                        id: i,
                        name: countries[i].name
                    };
                    $scope.options.billingCountries.push(newCountry);
                };
            });
        };

        // Get all the payment methods via API.
        $scope.getPaymentMethods = function () {
            paymentMethodService.getPaymentMethods().then(function (methods) {
                $scope.methods = methods;
                for (var i = 0; i < methods.length; i++) {
                    var newMethod = {
                        id: i,
                        name: methods[i].name
                    };
                    $scope.options.paymentMethods.push(newMethod);
                }
            });
        };

        // Get all the provinces via API.
        $scope.getProvinces = function () {
            provinceService.getProvinces().then(function (response) {
                var provinces = [];
                for (var i = 0; i < response.length; i++) {
                    provinces.push(checkout.Tools.downCaseProperties(response[i]));
                };
                $scope.provinces = provinces;
                for (var i = 0; i < provinces.length; i++) {
                    var newProvince = {
                        id: i,
                        name: provinces[i].name
                    };
                    $scope.options.shippingRegions.push(newProvince);
                };
            });
        };

        // Get all the provinces via API.
        $scope.getAllProvinces = function () {
            provinceService.getAllProvinces().then(function (provinces) {
                $scope.billingProvinces = provinces;
                for (var i = 0; i < provinces.length; i++) {
                    var newProvince = {
                        id: i,
                        name: provinces[i].name
                    };
                    $scope.options.billingRegions.push(newProvince);
                };
            });
        };

        // Get the shipping quotes for the customer based on their cart and shipping address.
        $scope.getQuotes = function () {
            var i;
            var address = new checkout.Models.Address($scope.shippingAddress);
            address.customerKey = $scope.customerGuid;
            address.email = $scope.contactInfo.email;
            shippingMethodService.getQuotes(address).then(function (response) {
                $scope.quotes = response;
                for (i = 0; i < ($scope.options.shippingMethods.length - 1) ; i++) {
                    $scope.options.shippingMethods.pop();
                };
                for (i = 0; i < $scope.quotes.length; i++) {
                    var newQuote = {
                        id: i,
                        name: $scope.quotes[i].shippingMethodName + " ($" + $scope.quotes[i].rate.toFixed(2) + ")"
                    };
                    $scope.options.shippingMethods.push(newQuote);
                };
            });
        };

        // Get the preSaleSummary via API.
        $scope.getSummary = function () {
            var contactInfo = $scope.contactInfo;
            var guid = $scope.customerGuid;
            var shipMethodKey = $scope.shipMethodKey;
            var paymentMethodKey = $scope.paymentMethodKey;
            var paymentArgs = $scope.paymentArgs;
            if (shipMethodKey) {
                var shippingAddress = $scope.shippingAddress;
                var billingAddress;
                if ($scope.visible.panels.hideBillingAddress) {
                    billingAddress = $scope.shippingAddress;
                } else {
                    billingAddress = $scope.billingAddress;
                }
                shipMethodKey = $scope.shipMethodKey;
                var iShippingAddress = addressService.convertToIAddress(guid, shippingAddress);
                var iBillingAddress = addressService.convertToIAddress(guid, billingAddress);
                orderService.getSummary(contactInfo, iShippingAddress, iBillingAddress, shipMethodKey, paymentMethodKey, paymentArgs).then(function (summary) {
                    $scope.summary = summary;
                });
            }
        };

        // Called when controller is loaded.
        $scope.init = function () {
            $scope.resetVariables();
            $scope.getCountries();
            $scope.getPaymentMethods();
            $scope.getProvinces();
            $scope.getAllCountries();
            $scope.getAllProvinces();
        };

        $scope.processError = function (error) {
            $scope.orderErrorText = error;
            $scope.hasOrderError = true;
        };

        // Set or reset variables.
        $scope.resetVariables = function () {
            $scope.completed = {
                contactInfo: false,
                shipping: false,
                shippingMethod: false,
                payment: false
            };
            $scope.creditCard = {
                name: '',
                number: '',
                code: ''
            };
            $scope.options = {
                billingCountries: [
                    { id: -1, name: "Country" }
                ],
                expireMonth: [
                    { id: -1, name: "Month" },
                    { id: 0, name: "01" },
                    { id: 1, name: "02" },
                    { id: 2, name: "03" },
                    { id: 3, name: "04" },
                    { id: 4, name: "05" },
                    { id: 5, name: "06" },
                    { id: 6, name: "07" },
                    { id: 7, name: "08" },
                    { id: 8, name: "09" },
                    { id: 9, name: "10" },
                    { id: 10, name: "11" },
                    { id: 11, name: "12" }
                ],
                expireYear: [
                    { id: -1, name: "Year" }
                ],
                paymentMethods: [
                    { id: -1, name: "Choose your payment method" }
                ],
                shippingCountries: [
                    { id: -1, name: "Country" }
                ],
                shippingMethods: [
                    { id: -1, name: "Choose your shipping method" }
                ],
                billingRegions: [
                    { id: -1, name: "State/Province" }
                ],
                shippingRegions: [
                    { id: -1, name: "State/Province" }
                ]
            };
            var d = new Date();
            var year = d.getFullYear();
            for (var i = 0; i < 11; i++) {
                var newYear = {
                    id: i,
                    name: year
                };
                $scope.options.expireYear.push(newYear);
                year += 1;
            };
            $scope.filters = {
                billingCountry: $scope.options.shippingCountries[0],
                billingRegion: $scope.options.shippingRegions[0],
                expireMonth: $scope.options.expireMonth[0],
                expireYear: $scope.options.expireYear[0],
                paymentMethod: $scope.options.paymentMethods[0],
                shippingCountry: $scope.options.shippingCountries[0],
                shippingMethod: $scope.options.shippingMethods[0],
                shippingRegion: $scope.options.shippingRegions[0]
            };
            $scope.visible = {
                errors: {
                    contactInfo: false,
                    shipping: false
                },
                panels: {
                    hideBillingAddress: true,
                    creditCard: false
                },
                results: {
                    contactInfo: false,
                    shipping: false,
                    shippingMethod: false,
                    payment: false
                }
            };
            $scope.customerGuid = "";
            $scope.contactInfo = new checkout.Models.ContactInfo();
            $scope.countries = [];
            $scope.provinces = [];
            $scope.billingCountries = [];
            $scope.billingProvinces = [];
            $scope.quotes = [];
            $scope.selectedPaymentMethod = {};
            $scope.selectedPaymentMethod.description = "";
            $scope.shippingAddress = addressService.newAddress();
            $scope.billingAddress = addressService.newAddress();
            $scope.paymentArgs = [];
            $scope.paymentAlias = "";
            $scope.paymentMethod = "";
            $scope.paymentMethodKey = null;
            $scope.readyToComplete = function () {
                var result = false;
                if ($scope.completed.contactInfo && $scope.completed.shipping && $scope.completed.shippingMethod && $scope.completed.payment) {
                    result = true;
                }
                return result;
            };
            $scope.shippingCost = 0;
            $scope.shippingMethod = false;
            $scope.shipMethodKey = "";
            $scope.subtotal = 0;
            $scope.summary = false;
            $scope.hasOrderError = false;
            $scope.orderErrorText = '';
        };

        // Returns the total taxes.
        $scope.taxes = function () {
            if ($scope.summary) {
                return $scope.summary.taxTotal.toFixed(2);
            } else {
                return (0).toFixed(2);
            }

        };

        // Returns the total price.
        $scope.totalPrice = function () {
            var result = 0;
            result += $scope.subtotal * 1;
            result += $scope.shippingCost * 1;
            result += $scope.taxes() * 1;
            return (result * 1).toFixed(2);
        };

        // Update the applicable binding associaetd with the filter after a dropdown is used.
        $scope.updateFilterBinding = function (value, key) {
            switch (key) {
                case "billingCountry":
                    if (value > -1) {
                        $scope.billingAddress.countryCode = $scope.billingCountries[value * 1].countryCode;
                    }
                    break;
                case "billingRegion":
                    if (value > -1) {
                        $scope.billingAddress.region = $scope.billingProvinces[value * 1].code;
                    }
                    break;
                case "paymentMethod":
                    $scope.paymentAlias = $scope.methods[value * 1].alias;
                    $scope.paymentMethod = $scope.methods[value * 1].name;
                    $scope.paymentMethodKey = $scope.methods[value * 1].key;
                    $scope.selectedPaymentMethod = $scope.methods[value * 1];
                    if ($scope.paymentAlias === "AuthorizeNet.CreditCard") {
                        $scope.visible.panels.creditCard = true;
                    } else {
                        $scope.visible.panels.creditCard = false;
                    }
                    break;
                case "shippingCountry":
                    if (value > -1) {
                        $scope.shippingAddress.countryCode = $scope.countries[value * 1].countryCode;
                    }
                    break;
                case "shippingMethod":
                    if (value > -1) {
                        $scope.shippingMethod = $scope.quotes[value * 1].shippingMethodName;
                        $scope.shipMethodKey = $scope.quotes[value * 1].key;
                        $scope.shippingCost = $scope.quotes[value * 1].rate.toFixed(2);
                    }
                    break;
                case "shippingRegion":
                    if (value > -1) {
                        $scope.shippingAddress.region = $scope.provinces[value * 1].code;
                    }
                    break;
                case "expireMonth":
                    if (value > -1) {
                        var month = $scope.filters.expireMonth.name;
                        month = month * 1;
                        if (month < 10) {
                            month = "0" + month;
                        }
                        $scope.expireMonth = month;
                    }
                    break;
                case "expireYear":
                    if (value > -1) {
                        $scope.expireYear = $scope.filters.expireYear.name;
                    }
                    break;
            };
        };

        // Load the init function.
        $scope.init();

    };

    app.controller("checkoutController", ["$scope", "addressService", "countryService", "paymentMethodService", "orderService", "provinceService", "shippingMethodService", checkoutController]);

}(angular.module(appName)));