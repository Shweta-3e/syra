﻿SyraApp.controller("DatabaseViewController", ["$scope", "$http", "syraservice", "$sce","$state",
    function ($scope, $http, syraservice, $sce, $state) {

        $scope.alldata = [];

        $scope.GetData = function () {
            $http.get('/LuisDomains/GetDb').success(function (data) {
                if (data.IsSuccess) {
                    $scope.alldata = data.Data;
                    angular.forEach($scope.alldata, function (entry) {
                        entry.BotResponse = $sce.trustAsHtml(entry.Response);
                        
                    });
                }
                else {
                    window.href.location('/Home');
                }
            });
        };
        $scope.GetData();
        $scope.Delete = function (id) {
            if (confirm('Are you sure ? You want to delete data from db?')) {
                $http.post("/LuisDomains/DeleteData", { id: id }).success(function (data) {
                    console.log("DataEntry Id is : " + id);
                    if (data.IsSuccess) {
                        syraservice.RecordStatusMessage("success", data.Message);
                        $scope.GetLuisDomain();
                    }
                    else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };
    }
]);

SyraApp.controller("DatabaseAddController", ["$scope", "$http", "syraservice", "$state", "$stateParams",
    function ($scope, $http, syraservice, $state, $stateParams) {

        $scope.CreateDbEntry = {};
        $scope.IsEditMode = false;
        $scope.tab = 1;

        $scope.id = $stateParams.id;

        if ($scope.id == undefined) {
            $scope.IsEditMode = false;
        } else {
            $scope.IsEditMode = true;
        }
        if ($scope.IsEditMode) {
            $http.post("/LuisDomains/GetDbEntries/", { id: $scope.id }).success(function (data) {
                $scope.CreateDbEntry = data.Data;
            });
        }
        $scope.DbEntryCreate = function () {
            if ($scope.IsEditMode) {
                $http.post('/LuisDomains/UpdateDatabase', { managedb: $scope.CreateDbEntry }).success(function (data) {
                    if (data.IsSuccess) {
                        $scope.CreateDbEntry = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("/database");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
            else {
                $http.post('/LuisDomains/CreateEntries', { manageDb: $scope.CreateDbEntry }).success(function (data) {
                    if (data.IsSuccess) {
                        $scope.CreateDbEntry = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("/luisdomain");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        }

        $scope.Cancel = function () {
            $state.go("/database");
        };
        $scope.SetActiveTab = function (tab, control) {
            $scope.tab = tab;
            $timeout(function () {
                $(control).focus();
            }, 1500);
        };

        $scope.isActiveTab = function (tab) {
            return $scope.tab == tab;
        };

        $scope.NextTab = function (tab) {
            $scope.tab = tab + 1;
            $scope.isActiveTab($scope.tab);
        };

        $scope.PreviousTab = function (tab) {
            $scope.tab = tab - 1;
            $scope.isActiveTab($scope.tab);
        };
    }
]);