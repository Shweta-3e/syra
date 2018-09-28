SyraApp.controller("LogsController", ["$scope", "$http", "syraservice", "$state", "$sce", "$stateParams",
    function ($scope, $http, syraservice, $state, $sce, $stateParams) {
        $scope.test = "Angular Js Date time picker";

        this.myDate = new Date();
        this.isOpen = false;

        $scope.selectedDt = "";
        $scope.showDate = function () {
            //alert($scope.selectedDt);
            $http.post("/Customer/GetLogs", { customerdt: $scope.selectedDt }).success(function (response) {
                console.log(response.Data);
                if (response.IsSuccess) {
                    console.log("I am in success");
                    $scope.Logs = response.Data;
                    angular.forEach($scope.Logs, function (log) {
                        log.BotAnswers1 = $sce.trustAsHtml(log.BotAnswers);
                    });
                    syraservice.RecordStatusMessage("success", response.Message);
                } else {
                    syraservice.RecordStatusMessage("error", response.Message);
                }
            });
            //alert($scope.selectedDt);
        }
        
    }]);