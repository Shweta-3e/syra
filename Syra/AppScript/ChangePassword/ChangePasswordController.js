SyraApp.controller("ChangepasswordController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.Changepassword = {};
        $scope.IsSuccess = false;
        $scope.IsError = false;


        $scope.ForgotPassword = function () {
            $http.post('/Account/ChangePassWord', { model: $scope.Changepassword }).success(function (data) {
                if (data.IsSuccess) {
                    $scope.Changepassword = {};
                    syraservice.RecordStatusMessage("success", data.Message);
                    $state.go("Home");
                } else {
                    syraservice.RecordStatusMessage("error", data.Message);
                }
            });
        };
    }])