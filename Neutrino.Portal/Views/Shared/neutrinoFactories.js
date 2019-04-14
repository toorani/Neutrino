angular.module('neutrinoProject')
.factory('envService', function () {
    var project = {};
    var pageTitle = '';
    var appMenu = [];
    project.initialize = function () {
        pageTitle = '';
    }

    project.setPageTitle = function (title) {
        pageTitle = title;
    }

    project.getPageTitle = function () {
        return pageTitle;
    }
    project.setAppMenu = function (menuItems) {
        appMenu = menuItems;
    }

    project.suitablePostingModel = function (obj) {
        if (typeof obj === 'string' || obj === "") {
            return;
        }
        if (typeof obj === 'number' || obj === 0) {
            return;
        }
        $.each(obj, function (key, value) {
            if (key == 'transactionalData') {
                delete obj[key];
            }
            else if (value === "" || value === null) {
                delete obj[key];
            } else if ($.isArray(value)) {
                if (value.length === 0) {
                    delete obj[key];
                    return;
                }
                $.each(value, function (k, v) {
                    project.suitablePostingModel(v);
                });
                if (value.length === 0) {
                    delete obj[key];
                }
            } else if (typeof value === 'object') {
                if (Object.keys(value).length === 0) {
                    delete obj[key];
                    return;
                }
                project.suitablePostingModel(value);
                if (Object.keys(value).length === 0) {
                    delete obj[key];
                }
            }
        });
    }

    return project;
})
.factory('focus', function ($timeout, $window) {
    return function (id) {
        // timeout makes sure that it is invoked after any other event has been triggered.
        // e.g. click events that need to run before the focus or
        // inputs elements that are in a disabled state but are enabled when those events
        // are triggered.
        $timeout(function () {
            var element = $window.document.getElementById(id);
            if (element)
                element.focus();
        });
    };
})
.factory('permissions', function ($rootScope, $filter, $q, $http, applicationConfiguration) {
    var permissionList;
    var checkAccess = applicationConfiguration.checkAccess.toLowerCase() == 'true';
    var getValidUrl = function (url) {
        url = url.trim();
        if (url[0] != '/')
            url = '/' + url;
        return url;
    }
    return {
        setPermissions: function (permissions) {
            permissionList = permissions;
            $rootScope.$broadcast('permissionsChanged');
        },
        isLoadPermission: function () {
            return permissionList != null;
        },
        checkRoutePermission: function (htmlUrl) {
            if (checkAccess) {
                htmlUrl = getValidUrl(htmlUrl);
                if (htmlUrl != '/home/index' && htmlUrl != '/account/forbidden') {
                    if (this.isLoadPermission()) {
                        var hasAccess = $filter('filter')(permissionList, { htmlUrl: htmlUrl }, false);
                        return hasAccess.length != 0;
                    }
                    return false;
                }
            }
            return true;
        },
        checkActionPermission: function (actionUrl) {
            if (checkAccess) {
                actionUrl = getValidUrl(actionUrl);
                if (this.isLoadPermission()) {
                    var hasAccess = $filter('filter')(permissionList, { actionUrl: actionUrl }, false);
                    return hasAccess.length != 0;
                }
                return false;
            }
            return true;
        },
        checkActionTypePermission: function (htmlUrl, actionTypeId) {
            if (checkAccess) {
                htmlUrl = getValidUrl(htmlUrl);
                if (this.isLoadPermission()) {
                    var hasAccess = $filter('filter')(permissionList, { htmlUrl: htmlUrl, actionTypeId: actionTypeId }, false);
                    return hasAccess.length != 0;
                }
                return false;
            }
            return true;


            //if (this.isLoadPermission() == false) {
            //    this.loadUserPermission().
            //    then(function (permission) {
            //        var hasAccess = $filter('filter')(permission, { htmlUrl: htmlUrl, actionTypeId: actionTypeId }, true);
            //        return hasAccess.length != 0;
            //    });
            //}
            //else {

            //}
        },
        checkActionUrlPermissionByHtmlUrl: function (htmlUrl, actionUrl) {
            if (checkAccess) {
                htmlUrl = getValidUrl(htmlUrl);
                if (this.isLoadPermission()) {
                    var hasAccess = $filter('filter')(permissionList, { htmlUrl: htmlUrl, actionUrl: actionUrl }, false);
                    return hasAccess.length != 0;
                }
                return false
            }
            return true;

        },
        loadUserPermission: function () {
            var deferedObject = $q.defer();
            $http.post('api/permissionService/getUserPermission', {}).then(
                function (response, status, headers, config) {
                    deferedObject.resolve(response.data);
                }, function (response) {
                    console.log("Error in loading user permission :" + response.data);
                });
            return deferedObject.promise;

        }
    };
});
