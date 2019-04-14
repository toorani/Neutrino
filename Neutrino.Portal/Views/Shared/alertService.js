console.log("alert service");

angular.module('neutrinoProject').service('alertService', ['ngToast', '$window', function (ngToast,$window) {

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

    this.showError = function (message) {

        var messageBox = formatMessage(message);

        ngToast.create({
            content: messageBox
            , className: 'alert alert-danger'
            , dismissButton: true
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


}]);
