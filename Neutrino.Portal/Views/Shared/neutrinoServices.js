
console.log("neutrino service");

angular.module('neutrinoProject')
.service('ajaxService', ['$http', '$location', '$window', 'blockUI', 'envService',
function ($http, $location, $window, blockUI, envService) {
    "use strict";
    this.msg = 'لطفا صبر کنید ...'
    this.ajaxPost = function (data, route, successFunction, errorFunction, message) {

        return this.ajaxCall(data, route, 'post', successFunction, errorFunction, message);
        //return $http.post(route, data).then(
        //    function (response, status, headers, config) {
        //        blockUI.stop();
        //        return successFunction(response, status);
        //    }, function (response) {
        //        blockUI.stop();
        //        return errorFunction(response);
        //    });
    }
    this.ajaxCall = function (data, route, httpMethod, successFunction, errorFunction, message) {
        if (httpMethod === undefined) {
            console.error('http method cannot null or empty.');
            return;
        }
        
        if (message != null) {
            this.msg = message;
        }
        else {
            this.msg = 'لطفا صبر کنید ...'
        }
       

        httpMethod = new String(httpMethod).toUpperCase();
        var methods = ['GET', 'POST', 'DELETE', 'PUT'];
        if (methods.indexOf(httpMethod) == -1) {
            console.error('http method is invalid.');
            return;
        }
        blockUI.start(this.msg);
            

        if (data != null) {
            envService.suitablePostingModel(data);
        }

        if (httpMethod == 'GET') {
            return $http({
                params: data,
                method: httpMethod,
                url: route
            })
            .then(
               function (response, status, headers, config) {
                   blockUI.stop();
                   return successFunction(response, status);
               }, function (response) {
                   blockUI.stop();
                   if (response.status == 403) {
                       $location.url('account/forbidden');
                       //$window.location.href = '/account/login';
                   }
                   else {
                       return errorFunction(response);
                   }

               });
        }

        return $http({
            data: data,
            method: httpMethod,
            url: route
        }).then(
               function (response, status, headers, config) {
                   blockUI.stop();
                   return successFunction(response, status);
               }, function (response) {
                   blockUI.stop();
                   if (response.status == 403) {
                       $location.url('account/forbidden');
                   }
                   else {
                       return errorFunction(response);
                   }

               });

    }
}])
.service('alertService', ['ngToast', '$window'
, function (ngToast, $window) {

    var _alerts = [];
    var _messageBox = "";

    this.setValidationErrors = function (scope, validationErrors) {

        for (var prop in validationErrors) {
            var property = prop + "InputError";
            scope[property] = true;
        }

    }

    this.returnFormattedMessage = function () {
        return _messageBox;
    }

    this.returnAlerts = function () {
        return _alerts;
    }

    this.showError = function (response) {
        var message = ''
        if (response != undefined) {
            if (typeof response == 'string' || response.data instanceof String) {
                message = response;
            }
            else if (typeof response.data == 'object' || response.data instanceof Object) {
                if (response.data.returnMessage !== undefined)
                    message = response.data.returnMessage;
                else if (response.data.message !== undefined)
                    message = response.data.message;
            }
            else if (typeof response.data == 'string' || response.data instanceof String) {
                message = response.data;
            }
        }

        var messageBox = formatMessage(message);

        ngToast.create({
            content: messageBox
            , className: 'alert alert-danger'
            , dismissButton: true
            ,timeout : 8000
            , animation: 'fade'
            , horizontalPosition: 'right'
            , verticalPosition: 'top'
        })

        //_alerts = [];
        //_messageBox = messageBox;
        //_alerts.push({ 'type': 'danger', 'msg': '' });

    };

    this.showSuccess = function (message) {

        var messageBox = formatMessage(message);

        ngToast.create({
            content: messageBox
            , className: 'alert alert-success'
            , dismissButton: true
            , animation: 'fade'
            , horizontalPosition: 'left'
            , verticalPosition: 'top'
        })

        //_alerts = [];
        //_messageBox = messageBox;
        //_alerts.push({ 'type': 'success', 'msg': '' });

    };

    this.showWarning = function (message) {

        var messageBox = formatMessage(message);

        ngToast.create({
            content: messageBox
             , className: 'alert alert-warning'
             , dismissButton: true
             , animation: 'fade'
             , horizontalPosition: 'left'
             , verticalPosition: 'top'
        })

        //_alerts = [];
        //_messageBox = messageBox;
        //_alerts.push({ 'type': 'warning', 'msg': '' });

    };

    this.showInfo = function (message) {

        var messageBox = formatMessage(message);

        ngToast.create({
            content: messageBox
             , className: 'alert alert-info'
             , dismissButton: true
             , animation: 'fade'
             , horizontalPosition: 'left'
             , verticalPosition: 'top'
        })
        //_alerts = [];
        //_messageBox = messageBox;
        //_alerts.push({ 'type': 'info', 'msg': '' });

    };

    function formatMessage(message) {
        var messageBox = "";
        if (angular.isArray(message) == true) {
            for (var i = 0; i < message.length; i++) {
                messageBox = messageBox + message[i] + "<br/>";
            }
        }
        else {
            messageBox = message;
        }

        return messageBox;

    }


}])
.service('modalService', ['$uibModal',
function ($uibModal) {

    var modalDefaults = {
        backdrop: true,
        keyboard: true,
        modalFade: true,
        templateUrl: '/views/shared/modal.tmpl.html'
    };

    var modalOptions = {
        closeButtonText: 'انصراف',
        actionButtonText: 'تایید',
        headerText: 'نوترینو',
        bodyText: 'مطمئن هستید ؟',
        actionButtonIcon: 'fa fa-bolt',

    };



    this.showModal = function (customModalDefaults, customModalOptions) {
        if (!customModalDefaults) customModalDefaults = {};
        customModalDefaults.backdrop = 'static';
        return this.show(customModalDefaults, customModalOptions);
    };

    this.show = function (customModalDefaults, customModalOptions) {

        //Create temp objects to work with since we're in a singleton service
        var tempModalDefaults = {};
        var tempModalOptions = {};

        //Map angular-ui modal custom defaults to modal defaults defined in service
        angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

        //Map modal.html $scope custom properties to defaults defined in service
        angular.extend(tempModalOptions, modalOptions, customModalOptions);


        if (!tempModalDefaults.controller) {
            tempModalDefaults.controller = function ($scope) {
                $scope.modalOptions = tempModalOptions;
                $scope.modalOptions.ok = function (result) {
                    $scope.$close('ok');
                };
                $scope.modalOptions.close = function (result) {
                    $scope.$dismiss('cancel');
                };
            };
        }

        return $uibModal.open(tempModalDefaults).result;
    };

}])
.service('persianCalendar', [
 function () {
     this.getMonthNames = function () {
         return [{ id: 1, name: 'فروردین' }
            , { id: 2, name: 'اردیبهشت' }
            , { id: 3, name: 'خرداد' }
            , { id: 4, name: 'تیر' }
            , { id: 5, name: 'مرداد' }
            , { id: 6, name: 'شهریور' }
            , { id: 7, name: 'مهر' }
            , { id: 8, name: 'آبان' }
            , { id: 9, name: 'آذر' }
            , { id: 10, name: 'دی' }
            , { id: 11, name: 'بهمن' }
            , { id: 12, name: 'اسفند' }
         ];
     }

     this.getYears = function () {
         var years = [];
         for (var i = 1397; i < 1407; i++) {
             years.push(i);
         }
         years.sort();
         return years;
     }
 }]);