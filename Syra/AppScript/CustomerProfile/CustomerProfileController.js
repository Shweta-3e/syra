﻿SyraApp.controller("CustomerProfileController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.Customer = {};
        $scope.disabled = true;
        $scope.CustomerPlan = {};
        $scope.planlist = [{ "name": 'Starter!' }, { "name": 'Get Going!' }, { "name": 'Almost There!' }, { "name": 'There!' }, { "name": 'Custom There!'}];

        $scope.IsEditMode = false;
        $scope.UpgradeSubscription = function () {
            $scope.IsEditMode = true;
            $scope.disabled = false;
        }
        $scope.UpgradePlan = function (planname) {
            $http.post('/Customer/DisplaySubscription/', { planname: planname }).success(function (data) {
                if (data.IsSuccess = true) {
                    $scope.CustomerPlan = data.Data;
                }
                else {
                    syraservice.RecordStatusMessage("error", data.Message);
                }
            })
        }
        $scope.SubscriptionUpdate = function (planname) {
            console.log(planname);
            $http.post('/Customer/UpdateSubscription/', { planname: planname }).success(function (data) {
                if (data.IsSuccess = true) {
                    $scope.CustomerPlan = data.Data;
                    $state.go("profile");
                }
                else {
                    syraservice.RecordStatusMessage("error", data.Message);
                }
            })
        }
        $scope.CancelSubscription = function () {
            $scope.IsEditMode = false;
            $state.go("profile");
        }
        $scope.GetCurrentUser = function () {
            $http.post('/Customer/GetCurrentUser'
            ).success(function (data) {
                $scope.Customer = data.Data;
                $http.post('/Customer/GetCustomerActivePlan',
                    {
                        customerId: $scope.Customer.Id
                    }).success(function (data) {
                        $scope.CustomerPlan = data.Data;
                    });
            });
        }
        $scope.GetCurrentUser();

        $scope.Edit = function () {
            $scope.IsEditMode = true;
        }

        $scope.Update = function () {
            console.log($scope.Customer);
            $http.post("/Customer/UpdateProfile", { customerView: $scope.Customer }).success(function (response) {
                if (response.isSaved) {
                    syraservice.RecordStatusMessage("success", response.Message);
                    $scope.GetCurrentUser();
                    $scope.Customer = response.Data;
                    $scope.IsEditMode = false;
                } else {
                    syraservice.RecordStatusMessage("error", response.Message);
                }
            })
        };

        $scope.Cancel = function () {
            $scope.IsEditMode = false;
        }
}]);
