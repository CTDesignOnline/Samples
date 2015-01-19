'use strict';

(function () {
        function categoryEditor ($scope, dialogService) {

            //--------------------------------------------------------------------------------------
            // Declare and initialize key scope properties
            //--------------------------------------------------------------------------------------
            
            /**
             * @ngdoc method
             * @name init
             * @function
             * 
             * @description
             * Method called on intial page load.  Loads in data from server and sets up scope.
             */
            function init() {
                if (isNullOrEmpty($scope.model.value)) {
                    $scope.model.value = [];
                }
            }
            init();
            
            function isNullOrEmpty(val) {
                return angular.isUndefined(val) || val === null || val === '';
            }

            /**
             * @ngdoc method
             * @name categoryLoaded
             * @function
             * 
             * @description
             * Checks the category object to see if the category is already loaded.
             */
            $scope.isSelected = function (key) {
                var arr = $scope.model.value;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i] === key) {
                        return true;
                    }
                }
                return false;
            }

            /**
             * @ngdoc method
             * @name selectCategory
             * @function
             * 
             * @description
             * Opens the category select dialog via the Umbraco dialogService.
             */
            $scope.selectCategories = function () {
                var selectedCategories = [];
                angular.copy($scope.model.value, selectedCategories);
                dialogService.open({
                    template: '/App_Plugins/CategoryPicker/Views/categoryDialog.html',
                    show: true,
                    callback: selectedCategoriesFromDialog,
                    dialogData: { categories: $scope.model.config.categories, selectedCategories: selectedCategories }
                });
            }

            /**
             * @ngdoc method
             * @name selectedCategoriesFromDialog
             * @function
             * 
             * @description
             * Handles the model update after recieving the categories to add from the dialog view/controller
             */
            function selectedCategoriesFromDialog (categories) {
                $scope.model.value = categories;
            }

            /**
             * @ngdoc method
             * @name selectedCategoriesFromDialog
             * @function
             * 
             * @description
             * Handles the model update after recieving the categories to add from the dialog view/controller
             */
            $scope.deleteCategory = function (category) {
                for (var i = 0; i < $scope.model.value.length; i++) {
                    if ($scope.model.value[i] == category.key) {
                        $scope.model.value.splice(i, 1);
                        // delete child categories
                        if (!angular.isUndefined(category.categories)) {
                            for (var i = 0; i < category.categories.length; i++) {
                                $scope.deleteCategory(category.categories[i])
                            }
                        }
                    }
                }
                CategoryHasFilter($scope.model.config.categories.categories);
            }

            function CategoryHasFilter(categories) {
                var hasFilter = false;
                if (!angular.isUndefined(categories)) {
                    for (var i = 0; i < categories.length; i++) {
                        // if the key is selected
                        if ($scope.isSelected(categories[i].key)) {
                            if (CategoryHasFilter(categories[i].categories)) {
                                hasFilter = true;
                            }
                            else if (categories[i].isFilter) {
                                hasFilter = true;
                            }
                            else {
                                //remove the category if it has no children.
                                $scope.deleteCategory(categories[i]);
                            }
                        }
                    }
                }
                return hasFilter;
            }
        }

        angular.module("umbraco").controller("BrambleBerry.CategoryEditor.CategoryPickerController", ['$scope', 'dialogService', categoryEditor]);
})();