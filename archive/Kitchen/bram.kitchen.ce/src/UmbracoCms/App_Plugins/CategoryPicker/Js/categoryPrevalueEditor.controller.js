'use strict';

(function () {
    function categoryPrevalueEditor($scope, dialogService, entityResource) {
        $scope.editingCategory = null;
        $scope.newCategory = null;

        function init() {
            if ($scope.model.value === null) {
                $scope.model.value = { categories: [] };
            }
        }
        init();

        $scope.cancel = function () {
            $scope.newCategory = null;
            $scope.editingCategory = null;
        }

        $scope.newRootCategory = function () {
            $scope.newCategory = {
                parent: null,
                category : newCategory("New Category", "")
            };
        }

        $scope.newSubCategory = function (category) {
            $scope.newCategory = {
                parent: category.key,
                category: newCategory("New Sub-Category", category.key)
            };
        }

        $scope.newFilter = function (category) {
            var filter = newCategory("New Filter", category.key);
            filter.isFilter = true;
            $scope.newCategory = {
                parent: category.key,
                category: filter
            };
        }

        $scope.addCategory = function (parent, category) {
            var parentKey = "";
            if (parent.key != undefined) {
                parentKey = parent.key;
            }
            category.key = $scope.safeKey(parentKey, category.name);

            if (category.isFilter) {
                delete category["categories"];
                delete category["content"];
            }

            parent.categories.push(category);
            $scope.newCategory = null;
        }

        $scope.editCategory = function (category) {
            $scope.editingCategory = {};
            $scope.newCategory = null;
            angular.copy(category, $scope.editingCategory);
            getContentDetails(category.content);
        }

        $scope.saveCategory = function (parent, editingCategory) {
            if (editingCategory.isFilter) {
                delete editingCategory["categories"];
                delete editingCategory["content"];
            }
            angular.forEach(parent.categories, function (category, index) {
                if (category.key == editingCategory.key) {
                    angular.copy(editingCategory, category);
                }
            });
            $scope.editingCategory = null;
        }

        $scope.deleteCategory = function (parent, deletingCategory) {
            if (confirm("Are you sure you wish to delete this category?")) {
                angular.forEach(parent.categories, function (category, index) {
                    if (category.key == deletingCategory.key) {
                        parent.categories.splice(index, 1);
                    }
                });
            }
        }
        function getContentDetails(content) {
            $scope.pickedNodeName = "";

            if (content != null) {
                entityResource.getById(content, "Document").then(function (data) {
                    $scope.pickedNodeName = data.name;
                })
            } else {
                $scope.pickedNodeName = "Pick content to associate with this category";
            }
        }

        $scope.openContentPicker = function () {
            var d = dialogService.treePicker({
                section: "content",
                treeAlias: "content",
                multiPicker: false,
                entityType: "Document",
                callback: populate
            });
        };

        function populate(data) {
            if ($scope.newCategory != null) {
                $scope.newCategory.category.content = data.id;
            }
            if ($scope.editingCategory != null) {
                $scope.editingCategory.content = data.id;
            }
            getContentDetails(data.id);
        }

        function newCategory(name, parentKey) {
            $scope.editingCategory = null;
            return {
                key: $scope.safeKey(parentKey, name),
                name: name,
                isFilter: false,
                content: null,
                categories: []
            };
        }

        $scope.safeKey = function(parentKey, name) {
            if (parentKey)
                parentKey += "-";

            var key = toCamelCase(name);
            // TODO: better safe alias algorythm
            return parentKey + key;
        }

        function toCamelCase(s) {
            if (s != null) {
                // remove all characters that should not be in a variable name
                // as well underscores an numbers from the beginning of the string
                s = s.replace(/([^a-zA-Z0-9_\- ])|^[_0-9]+/g, "").trim().toLowerCase();
                // uppercase letters preceeded by a hyphen or a space
                s = s.replace(/([ -]+)([a-zA-Z0-9])/g, function (a, b, c) {
                    return c.toUpperCase();
                });
                // uppercase letters following numbers
                s = s.replace(/([0-9]+)([a-zA-Z])/g, function (a, b, c) {
                    return b + c.toUpperCase();
                });
                return s;
            }
            return "";
        }

    }

    angular.module("umbraco").controller("BrambleBerry.CategoryEditor.PrevalueEditorController", ['$scope', 'dialogService', 'entityResource', categoryPrevalueEditor]);
})();