console.log("model service");
angular.module('neutrinoProject').service('modalService', ['$uibModal',
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

}]);