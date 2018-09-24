SyraApp.controller("RegistrationController", ["$scope", "$http","syraservice","$state",
    function ($scope, $http, syraservice, $state) {

        $scope.Register = {};
        $scope.IsSuccess = false;
        $scope.IsError = false;
        
    $scope.GetPlans = function () {
        $http.get('/Account/GetPlans').success(function (data) {
            console.log(data);
            $scope.Plans = data.Data;
        });
    };
    $scope.GetPlans();

    $scope.Registration = function () {
        $http.post('/Account/Register', { model: $scope.Register }).success(function (data) {
            if (data.IsSuccess) {
                $state.go("Confirmation");
            }
        });
    };
}])