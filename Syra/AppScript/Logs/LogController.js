SyraApp.controller("LogsController", ["$scope", "$http", "syraservice", "$state", "$sce", "$stateParams",
    function ($scope, $http, syraservice, $state, $sce, $stateParams) {
        $scope.test = "Angular Js Date time picker";

        $scope.myDate = new Date();
        $scope.endDate = new Date();

        $scope.minstartDate = new Date(
            $scope.myDate.getFullYear(),
            $scope.myDate.getMonth() - 6,
            $scope.myDate.getDate());

        $scope.maxstartDate = new Date(
            $scope.myDate.getFullYear(),
            $scope.myDate.getMonth(),
            $scope.myDate.getDate());

        $scope.minendDate = new Date(
            $scope.myDate.getFullYear(),
            $scope.myDate.getMonth() - 6,
            $scope.myDate.getDate());

        $scope.maxendDate = new Date(
            $scope.endDate.getFullYear(),
            $scope.endDate.getMonth(),
            $scope.endDate.getDate());

        //this.isOpen = false;
        $scope.showDate = function () {
            $http.post("/Customer/GetLogs", { startdt: $scope.myDate, enddt: $scope.endDate }).success(function (response) {
                //console.log(response.Data);
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