SyraApp.controller("PlanViewController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {


        $scope.Plans = {};
        $scope.PlanDetails = false;

        $scope.GetPlans = function () {
            $http.get('/Plans/GetPlans').success(function (data) {
                if (data.isSaved)
                {
                    $scope.Plans = data.Data;
                }
                else
                {
                    window.location.href = ("Account/Login");
                }
            });
        };
        $scope.GetPlans();
        $scope.Delete = function (id) {
            if (confirm('Are you sure ? You want to delete plan')) {
                $http.post("/Plans/DeletePlan", { id: id }).success(function (data) {
                    console.log("Plan Id is : " + id);
                    if (data.isSaved) {
                        syraservice.RecordStatusMessage("success", data.Message);
                        alert(data.Message);
                        $scope.GetPlans();
                    }
                    else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };
        $scope.Cancel = function () {
            $scope.PlanDetails = false;
            $state.go("newplan");
        };

    }
]);

SyraApp.controller("PlanAddController", ["$scope", "$http", "syraservice", "$state", "$stateParams",
    function ($scope, $http, syraservice, $state, $stateParams) {

        $scope.CreatePlan = {};
        $scope.IsEditMode = false;
        $scope.tab = 1;
       
        $scope.id = $stateParams.id;

        if ($scope.id == undefined) {
            $scope.IsEditMode = false;
        } else {
            $scope.IsEditMode = true;
        }
        if ($scope.IsEditMode) {
            $http.post("/Plans/GetPlanEntry/", { id: $scope.id }).success(function (data) {
                $scope.CreatePlan = data.Data;
            });
        }

        $scope.PlanCreate = function () {
            if ($scope.IsEditMode) {
                $http.post('/Plans/UpdatePlan', { plan: $scope.CreatePlan }).success(function (data) {
                    if (data.isSaved) {
                        $scope.CreatePlan = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("adminplan");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
            else {
                $http.post('/Plans/CreateNewPlan', { plan: $scope.CreatePlan }).success(function (data) {
                    if (data.isSaved) {
                        $scope.CreatePlan = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("adminplan");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
            
        }

        $scope.Cancel = function () {
            $state.go("adminplan");
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