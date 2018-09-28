"use strict";
var SOFT_VER = "1.0108";
var SyraApp = angular.module("syra", ["ui.router", 'ngMaterial', 'ngMessages']);
SyraApp.directive("dateTimePicker", DatetimePicker);
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