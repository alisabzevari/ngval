var ngval = angular.module('ngval', []);

ngval.directive('ngval', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, iElm, iAttrs, ngModel) {
            var messages = angular.fromJson(iAttrs.ngval);
            var getErrors = function() {
                var errors = [];
                for (var prop in messages) {
                    if (ngModel.$error[prop])
                        errors.push({ validator: prop, message: messages[prop] });
                }
                return errors;
            };
            scope.$watch(function() {
                return ngModel.$modelValue;
            }, function () {
                ngModel.ngval = {
                    hasError: ngModel.$dirty && ngModel.$invalid,
                    errors: getErrors()
                };
            });
        }
    };
}]);