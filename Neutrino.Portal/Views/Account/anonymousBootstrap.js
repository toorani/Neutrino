console.log("Anonymous Bootstrap ");


(function () {

    var app = angular.module('neutrinoProject', ['ngRoute', 'ngAnimate', 'ngSanitize', 'blockUI','ngToast']);

    app.config(['$controllerProvider', '$provide', '$routeProvider', '$locationProvider',
            function ($controllerProvider, $provide, $routeProvider, $locationProvider) {
                app.register =
                  {
                      controller: $controllerProvider.register,
                      service: $provide.service
                  };

                $locationProvider.html5Mode(true);
                $routeProvider.caseInsensitiveMatch = true;
                $routeProvider.when('/account/login', {
                    templateUrl: '/views/account/login.html',
                    
                })

            }])
    .run(function ($http) {
        $http.defaults.headers.common['X-XSRF-Token'] =
            angular.element('input[name="__RequestVerificationToken"]').attr('value');
    });
})();


console.log("Neutrino Bootstrap FINISHED 2");





//angular.module('anonymousSection', ['ngRoute', 'ngAnimate', 'ngSanitize', 'blockUI'])
//.config(['$routeProvider', '$locationProvider',
//function ($routeProvider, $locationProvider) {
    

//}])
//.run(function ($http) {
//    $http.defaults.headers.common['X-XSRF-Token'] =
//        angular.element('input[name="__RequestVerificationToken"]').attr('value');
//})
//.controller('loginController', loginController);

//console.log("Anonymous Module FINISHED");



