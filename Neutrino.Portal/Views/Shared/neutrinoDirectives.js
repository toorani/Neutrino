console.log("neutrinoDirectives");

angular.module('neutrinoProject')
.directive('treeView', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            localNodes: '=model',
            localClick: '&click'
        },
        link: function (scope, tElement, tAttrs, transclude) {

            var maxLevels = (angular.isUndefined(tAttrs.maxlevels)) ? 10 : tAttrs.maxlevels;
            var hasCheckBox = (angular.isUndefined(tAttrs.checkbox)) ? false : true;
            scope.showItems = [];

            scope.showHide = function (ulId) {
                var hideThis = document.getElementById(ulId);
                var showHide = angular.element(hideThis).attr('class');
                angular.element(hideThis).attr('class', (showHide === 'show' ? 'hide' : 'show'));
            }

            scope.showIcon = function (node) {
                if (!angular.isUndefined(node.children)) return true;
            }

            scope.checkIfChildren = function (node) {
                if (!angular.isUndefined(node.children)) return true;
            }

            /////////////////////////////////////////////////
            /// SELECT ALL CHILDRENS
            // as seen at: http://jsfiddle.net/incutonez/D8vhb/5/
            function parentCheckChange(item) {
                for (var i in item.children) {
                    item.children[i].checked = item.checked;
                    if (item.children[i].children) {
                        parentCheckChange(item.children[i]);
                    }
                }
            }

            scope.checkChange = function (node) {
                if (node.children) {
                    parentCheckChange(node);
                }
            }
            /////////////////////////////////////////////////

            function renderTreeView(collection, level, max) {
                var text = '';
                text += '<li ng-repeat="n in ' + collection + '" >';
                text += '<span ng-show=showIcon(n) class="show-hide" ng-click=showHide(n.id)><i class="fa fa-plus-square"></i></span>';
                text += '<span ng-show=!showIcon(n) style="padding-right: 13px"></span>';

                if (hasCheckBox) {
                    text += '<input class="tree-checkbox" type=checkbox ng-model=n.checked ng-change=checkChange(n)>';
                }


                text += '<span class="edit" ng-click=localClick({node:n})><i class="fa fa-pencil"></i></span>'


                text += '<label>{{n.name}}</label>';

                if (level < max) {
                    text += '<ul id="{{n.id}}" class="hide" ng-if=checkIfChildren(n)>' + renderTreeView('n.children', level + 1, max) + '</ul></li>';
                } else {
                    text += '</li>';
                }

                return text;
            }// end renderTreeView();

            try {
                var text = '<ul class="tree-view-wrapper">';
                text += renderTreeView('localNodes', 1, maxLevels);
                text += '</ul>';
                tElement.html(text);
                $compile(tElement.contents())(scope);
            }
            catch (err) {
                tElement.html('<b>ERROR!!!</b> - ' + err);
                $compile(tElement.contents())(scope);
            }
        }
    };
})
.directive('espValidateComplete', ['$window', '$parse', function ($window, $parse) {
    var options = {};

    return {
        restrict: 'A',
        require: 'form',
        link: function (scope, element, attributes) {
            var fn = $parse(attributes.espValidateComplete);
            var opts = angular.extend({}, options, scope.$eval(attributes.espValidateOptions));
            opts.binded = false;
            element.validationEngine('attach', opts);

            element.bind('submit', function (event) {
                if (!element.validationEngine('validate')) {
                    return false;
                }

                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
            });

            angular.element($window).bind('resize', function () {
                element.validationEngine('updatePromptsPosition');
            });
        }
    };
}])
.directive("limitToMax", function () {
    return {
        link: function (scope, element, attributes) {
            element.on("keydown keyup", function (e) {
                if (Number(element.val()) > Number(attributes.max) &&
                      e.keyCode != 46 // delete
                      &&
                      e.keyCode != 8 // backspace
                    ) {
                    e.preventDefault();
                    element.val(attributes.max);
                }
            });
        }
    };
})
.directive("preventTypingGreater", function () {
    return {
        link: function (scope, element, attributes) {
            var oldVal = null;
            element.on("keydown keyup", function (e) {
                if (Number(element.val()) > Number(attributes.max) &&
                      e.keyCode != 46 // delete
                      &&
                      e.keyCode != 8 // backspace
                    ) {
                    e.preventDefault();
                    element.val(oldVal);
                } else {
                    oldVal = Number(element.val());
                }
            });
        }
    };
})
.directive("disableAnimate", function ($animate) {
    return function (scope, element) {
        $animate.enabled(false, element);
    };
})
.directive('buttonId', function () {
    return {
        restrict: "A",
        link: function (scope, element, attributes) {
            element.bind("click", function () {
                // able to get the name of the button and pass it to the $scope
                // this is executed on every click
                scope.buttons.chosen = attributes.buttonId;
            });
        }
    }
});

