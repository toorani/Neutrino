/**
 * @author xialei <xialeistudio@gmail.com>
 */
angular.module('angular-icheck', [])
.directive('iCheck', ['$timeout', '$parse', function ($timeout, $parse) {
        return {
            restrict: 'EA',
            transclude: true,
            require: 'ngModel',
            scope: {
                localClick: '&click'
            },
            replace: true,
            template: '<div class="angular-icheck">\n    <div class="checkbox"></div>\n    <label ng-transclude></label> \n</div>',
            link: function (scope, ele, attrs, ctrl) {
                var box = angular.element(ele[0].querySelector('.checkbox'));
                ele.bind("click", function () {
                    box.toggleClass("checked");
                    ctrl.$setViewValue(box.hasClass("checked"));
                    if (scope.localClick != null)
                        scope.localClick();
                });
                ctrl.$render = function () {
                    if (ctrl.$viewValue) {
                        box.addClass("checked");
                    } else {
                        box.removeClass("checked");
                    }
                };
                // https://github.com/angular/angular.js/issues/2594
                // override $isEmpty method
                ctrl.$isEmpty = function (value) {
                    return value === false;
                };
                ctrl.$setViewValue(box.hasClass("checked"));
                ctrl.$validate();


            }
        }
    }]);