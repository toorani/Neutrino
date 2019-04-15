
console.log("Neutrino Bootstrap");

(function () {

    var app = angular.module('neutrinoProject', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ui.router', 'ui.bootstrap'
    , 'ui.select', 'ngToast', 'blockUI', 'oc.lazyLoad'
        , 'jsTree.directive', 'ADM-dateTimePicker', 'dynamicNumber', 'multipleSelect'
        , 'rzModule', 'mgcrea.ngStrap', 'sticky', 'angularFileUpload', 'mgo-angular-wizard'
        ,'angular-icheck'
    ]);
    var animationClassNameFilter = /vnd-animated/
    app.config(['$controllerProvider', '$provide', 'ADMdtpProvider', 'dynamicNumberStrategyProvider', '$httpProvider', '$animateProvider',
        function ($controllerProvider, $provide, ADMdtpProvider, dynamicNumberStrategyProvider, $httpProvider, $animateProvider) {
            app.register =
              {
                  controller: $controllerProvider.register,
                  service: $provide.service
              };

            $httpProvider.defaults.useXDomain = true;
            $httpProvider.defaults.withCredentials = true;
            $httpProvider.interceptors.push(['$q', function ($q) {
                return {
                    'responseError': function (rejection) {
                        if (rejection.status === 401) {
                            window.console.log('Got a 401');
                        }
                        return $q.reject(rejection);
                    }
                };
            }]);

            $animateProvider.classNameFilter(animationClassNameFilter)

            ADMdtpProvider.setOptions({
                calType: 'jalali',
                format: 'YYYY/MM/DD',
                //default: 'today',
                multiple: false,
                dtpType: 'date',
                autoClose: true

            });

            dynamicNumberStrategyProvider.addStrategy('price', {
                numInt: 15,
                numFract: 5,
                numSep: '.',
                numPos: true,
                numNeg: false,
                numRound: 'round',
                numThousand: true,
                numThousandSep: "'"
            });

            dynamicNumberStrategyProvider.addStrategy('numb', {
                numInt: 15,
                numFract: 0,
                numSep: '.',
                numPos: true,
                numNeg: false,
                numRound: 'round',
                numThousand: true,
                numThousandSep: "'"
            });

            dynamicNumberStrategyProvider.addStrategy('percent', {
                numInt: 3,
                numFract: 4,
                numSep: '.',
                numPos: true,
                numNeg: false,
                //numRound: 'round',
                numThousand: false,
            });

            dynamicNumberStrategyProvider.addStrategy('phone', {
                numInt: 10,
                numFract: 0,
                numSep: '.',
                numPos: true,
                numNeg: false,
                numRound: 'round',
                numThousand: false,
            });


        }]);
    app.run(function ($http, $location, $rootScope, $locale, permissions) {
        $http.defaults.headers.common['X-XSRF-Token'] =
            angular.element('input[name="__RequestVerificationToken"]').attr('value');

        $rootScope.checkActionTypePermission = function (actionTypeId) {
            var htmlUrl = $location.path();
            return permissions.checkActionTypePermission(htmlUrl, actionTypeId);
        };

        $rootScope.checkActionUrlPermission = function (actionUrl) {
            return permissions.checkActionPermission(actionUrl);
        }
        $rootScope.checkActionUrlPermissionByHtmlUrl = function (actionUrl) {
            var htmlUrl = $location.path();
            return permissions.checkActionUrlPermissionByHtmlUrl(htmlUrl, actionUrl);
        }
        $rootScope.checkActionUrlPermissionByPassHtmlUrl = function (actionUrl, htmlUrl) {
            return permissions.checkActionUrlPermissionByHtmlUrl(htmlUrl, actionUrl);
        }

        $locale.NUMBER_FORMATS.GROUP_SEP = "'";
        $locale.NUMBER_FORMATS.DECIMAL_SEP = ".";
    });
})();


console.log("Neutrino Bootstrap FINISHED 2");



