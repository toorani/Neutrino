console.log("global validation methods");

function ifSelectNotEmpty(field, rules, i, options) {
    if ($(field).find("option").length > 0 &&
        $(field).find("option:selected").length == 0
        || $(field).find("option:selected")[0].value == "") {
        rules.push('required');
        return $.validationEngineLanguage.allRules.required.alertTextCheckboxMultiple;
    }
}

function ifDateNotEmpty(field, rules, i, options) {

    if ($(field).find('input:text').length > 0 &&
        $(field).find('input:text')[0].value == "") {
        rules.push('required');
    }
}

var ifCheckBoxTreeNotEmpty = function (field, rules, i, options) {
    var jstreeCtrls = $(field).find(".jstree");
    if (jstreeCtrls.length == 1) {
        var jstreeCtrl = jstreeCtrls[0];
        var selectedNodes = $('#' + jstreeCtrl.id).jstree("get_checked", null, true);
        if (selectedNodes.length == 0) {
            rules.push('required');
            return $.validationEngineLanguage.allRules.required.alertTextCheckboxMultiple;
        }
    }
}

function ifTypeaheadNotEmpty(field, rules, i, options) {
    //
    if ($(field).attr('ng-Model') != undefined) {
        if (eval('angular.element($(field)).scope().' + $(field).attr('ng-Model')) == undefined) {
            rules.push('required');
            return $.validationEngineLanguage.allRules.required.alertTextCheckboxMultiple;
        }
    }
}

function ifSelectMultiSelectNotEmpty(field, rules, i, options) {
    if ($(field).attr('ng-Model') != undefined) {
        var dataValue = eval('angular.element($(field)).scope().' + $(field).attr('ng-Model'));
        if (dataValue == undefined || dataValue.length == 0) {
            rules.push('required');
            return $.validationEngineLanguage.allRules.required.alertTextCheckboxMultiple;
        }
    }


}
function ifUiSelectNotEmpty(field, rules, i, options) {

    if ($(field).attr('ng-Model') != undefined) {
        if (eval('angular.element($(field)).scope().' + $(field).attr('ng-Model')) == undefined) {
            rules.push('required');
            return $.validationEngineLanguage.allRules.required.alertTextCheckboxMultiple;
        }
    }
}


