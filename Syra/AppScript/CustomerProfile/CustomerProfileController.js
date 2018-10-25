SyraApp.controller("CustomerProfileController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.Customer = {};
        $scope.CustomerPlan = {};

        $scope.IsEditMode = false;
        $scope.GetCurrentUser = function () {
            $http.post('/Customer/GetCurrentUser'
            ).success(function (data) {
                $scope.Customer = data.Data;
                $http.post('/Customer/GetCustomerActivePlan',
                    {
                        customerId: $scope.Customer.Id
                    }).success(function (data) {
                        $scope.CustomerPlan = data.Data;
                        console.log("Customer plan is : ");
                        console.log($scope.CustomerPlan);
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
