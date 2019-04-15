
console.log("Neutrino routing - debug");

angular.module("neutrinoProject").config(['$routeProvider', '$locationProvider', 'applicationConfigurationProvider',
    function ($routeProvider, $locationProvider, applicationConfigurationProvider) {

        this.getApplicationVersion = function () {
            var applicationVersion = applicationConfigurationProvider.getVersion();
            return applicationVersion;
        }
        var baseSiteUrlPath = applicationConfigurationProvider.getBaseURL();
        $routeProvider.when('/:rootSection/:section/item/:id', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.rootSection + '/item.' + rp.section + '.html?v=' + this.getApplicationVersion();
            },

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
        $routeProvider.when('/:rootSection1/:rootSection2/:section/:tree', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.rootSection1 + '/' + rp.rootSection2 + '/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion();
                                         
            },
            resolve: {
                loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                    var path = $location.path().split("/");
                    var root_directory1 = path[1];
                    var root_directory2 = path[2];
                    var directory = path[3];
                    var controllerName = path[4];

                    var controllerToLoad = "Views/" + root_directory1 + "/" + root_directory2 + "/" + directory + "/" + controllerName +  "Controller.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]
            }
        });
        $routeProvider.when('/:rootSection1/:rootSection2/:section/:tree/:id', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.rootSection1 + '/' + rp.rootSection2 + '/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion();

            },
            resolve: {
                loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                    var path = $location.path().split("/");
                    var root_directory1 = path[1];
                    var root_directory2 = path[2];
                    var directory = path[3];
                    var controllerName = path[4];

                    var controllerToLoad = "Views/" + root_directory1 + "/" + root_directory2 + "/" + directory + "/" + controllerName + "Controller.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]
            }
        });
        $routeProvider.when('/:rootSection1/:rootSection2/:section/:tree/param/:param', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.rootSection1 + '/' + rp.rootSection2 + '/' + rp.section + '/' + rp.tree + '.html?v=' + this.getApplicationVersion();

            },
            resolve: {
                loadMyCtrl: ['$q', '$rootScope', '$location', '$ocLazyLoad', function ($q, $rootScope, $location, $ocLazyLoad) {

                    var path = $location.path().split("/");
                    var root_directory1 = path[1];
                    var root_directory2 = path[2];
                    var directory = path[3];
                    var controllerName = path[4];

                    var controllerToLoad = "Views/" + root_directory1 + "/" + root_directory2 + "/" + directory + "/" + controllerName + "Controller.js?v=" + this.getApplicationVersion();
                    return $ocLazyLoad.load(controllerToLoad);
                }]
            }
        });
        
        $routeProvider.when('/:section/item/:id', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.section + '/item.html?v=' + this.getApplicationVersion();
            },

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
        $routeProvider.when('/:section/index/:id', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.section + '/index.html?v=' + this.getApplicationVersion();
            },

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
        $routeProvider.when('/:rootSection/:section/:tree', {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.rootSection + '/' + rp.tree + '.' + rp.section + '.html?v=' + this.getApplicationVersion();
                                           
            },
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
        $routeProvider.when('/:section/:tree', {
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
        $routeProvider.when('/', {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/Home/Index.html?v=' + this.getApplicationVersion(); },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', 'permissions'
                     , function ($ocLazyLoad, permissions) {
                         var controllerToLoad = "Views/Home/IndexController.js?v=" + this.getApplicationVersion();
                         return $ocLazyLoad.load(controllerToLoad);
                     }]
            }
        });
        

        $locationProvider.html5Mode(true);

    }]);



