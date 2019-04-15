
console.log("Neutrino routing - debug");

angular.module("neutrinoProject").config(['$routeProvider', '$locationProvider', 'applicationConfigurationProvider',
    function ($routeProvider, $locationProvider, applicationConfigurationProvider) {

        this.getApplicationVersion = function () {
            var applicationVersion = applicationConfigurationProvider.getVersion();
            return applicationVersion;
        }

        var baseSiteUrlPath = applicationConfigurationProvider.getBaseURL();

        $routeProvider.when('/:section/:tree',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.' + rp.section + '.html?v=' + this.getApplicationVersion(); },

            resolve: {
                loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                    var path = $location.path().split("/");
                    var directory = path[1];
                    var controllerName = path[2];

                    var controllerToLoad = "Views/" + directory + "/" + controllerName + '.' + directory + "Controller.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]

            }


        });


        $routeProvider.when('/:rootSection/:section/:tree',
    {                                         
        templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.rootSection + '/' + rp.tree + '.' + rp.section + '.html?v=' + this.getApplicationVersion(); },

        resolve: {
            loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                var path = $location.path().split("/");
                var root_directory = path[1];
                var directory = path[2];
                var controllerName = path[3];

                var controllerToLoad = "Views/" + root_directory + "/" + controllerName + '.' + directory + "Controller.js?v=" + this.getApplicationVersion();
                return $ocLazyLoad.load(controllerToLoad);
            }]
        }


    });

        $routeProvider.when('/:section/:tree/:id',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion(); },

            resolve: {
                loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                    var path = $location.path().split("/");
                    var directory = path[1];
                    var controllerName = path[2];

                    var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]

            }

        });

        $routeProvider.when('/',
        {

            templateUrl: function (rp) { return baseSiteUrlPath + 'views/Home/Index.html?v=' + this.getApplicationVersion(); },

            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    var controllerToLoad = "Views/Home/IndexController.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]
            }


        });

        $locationProvider.html5Mode(true);

    }]);



