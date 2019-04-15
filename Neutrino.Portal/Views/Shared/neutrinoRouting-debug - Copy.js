
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
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion(); },

            resolve: {

                load: ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                    var path = $location.path().split("/");
                    var directory = path[1];
                    var controllerName = path[2];

                    var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + this.getApplicationVersion();

                    var deferred = $q.defer();
                    require([controllerToLoad], function () {
                        $rootScope.$apply(function () {
                            deferred.resolve();
                        });
                    });

                    return deferred.promise;

                }]
            }


        });


        $routeProvider.when('/:rootSection/:section/:tree',
    {                                                           //goal/distributor/index
                                                                //goal/index.distributorController
        templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.rootSection + '/' + rp.tree +'.' + rp.section + '.html?v=' + this.getApplicationVersion(); },

        resolve: {

            load: ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                var path = $location.path().split("/");
                var root_directory = path[1];
                var directory = path[2];
                var controllerName = path[3];

                var controllerToLoad = "Views/" + root_directory + "/" + controllerName + '.' + directory + "Controller.js?v=" + this.getApplicationVersion();

                var deferred = $q.defer();
                require([controllerToLoad], function () {
                    $rootScope.$apply(function () {
                        deferred.resolve();
                    });
                });

                return deferred.promise;

            }]
        }


    });

        $routeProvider.when('/:section/:tree/:id',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion(); },

            resolve: {

                load: ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                    var path = $location.path().split("/");
                    var directory = path[1];
                    var controllerName = path[2];

                    var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + this.getApplicationVersion();

                    var deferred = $q.defer();
                    require([controllerToLoad], function () {
                        $rootScope.$apply(function () {
                            deferred.resolve();
                        });
                    });

                    return deferred.promise;

                }]
            }

        });

        $routeProvider.when('/',
        {

            templateUrl: function (rp) { return baseSiteUrlPath + 'views/Home/Index.html?v=' + this.getApplicationVersion(); },

            resolve: {

                load: ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                    var controllerToLoad = "Views/Home/IndexController.js?v=" + this.getApplicationVersion();

                    var deferred = $q.defer();
                    require([controllerToLoad], function () {
                        $rootScope.$apply(function () {
                            deferred.resolve();
                        });
                    });

                    return deferred.promise;

                }]
            }


        });

        $locationProvider.html5Mode(true);

    }]);



