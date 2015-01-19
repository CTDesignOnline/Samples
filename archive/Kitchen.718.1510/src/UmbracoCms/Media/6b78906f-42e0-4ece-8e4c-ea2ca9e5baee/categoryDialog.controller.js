'use strict';

(function () {
    function categoryDialog($scope) {

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
        }
        init();


        /**
         * @ngdoc method
         * @name categoryLoaded
         * @function
         * 
         * @description
         * Checks the category object to see if the category is already loaded.
         */
        $scope.isSelected = function (key) {
            var arr = $scope.dialogData.selectedCategories;
            for (var i = 0; i < arr.length; i ++ ) {
                if (arr[i] === key) {
                    return true;
                }
            }
            return false;
        }


        $scope.selectCategory = function (categories) {
            if (categories[categories.length - 1].isFilter) {
                for (var i = 0; i < categories.length; i++) {
                    if (!angular.isUndefined(categories[i])) {
                        if (!$scope.isSelected(categories[i].key)) {
                            $scope.dialogData.selectedCategories.push(categories[i].key);
                        }
                    }
                }
            }
        }
    }

    angular.module("umbraco").controller("BrambleBerry.CategoryEditor.CategoryDialogController", ['$scope', categoryDialog]);
})();