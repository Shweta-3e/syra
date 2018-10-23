"use strict";
var SOFT_VER = "1.0110";
var SyraApp = angular.module("syra", ["ui.router", 'ngMaterial', 'ngMessages', 'datatables']);
SyraApp.directive("dateTimePicker", DatetimePicker);


SyraApp.constant('KeyCode', {
    Enter: 13,
    Escape: 27,
    UpArrow: 38,
    DownArrow: 40,
    F1: 112,
    F2: 113,
    F3: 114,
    F4: 115,
    F5: 116,
    F6: 117,
    F7: 118,
    F8: 119,
    F9: 120,
    F10: 121,
    F11: 122,
    F12: 123,
    Tab: 9,
    CtrlKey: 17,
    Space: 32,
});


function DatetimePicker() {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModelCtrl) {
            var parent = $(element).parent();
            var dtp = parent.datetimepicker({
                format: "DD-MM-YYYY",
                showTodayButton: true
                //pickTime: true
            });
            dtp.on("dp.change", function (e) {
                ngModelCtrl.$setViewValue(moment(e.date).format("DD-MM-YYYY"));
                scope.$apply();
            });
        }
    };
}