SyraApp.controller("LogsController", ["$scope", "$http", "syraservice", "$state", "$sce", "$stateParams",
    function ($scope, $http, syraservice, $state, $sce, $stateParams) {
        $scope.test = "Angular Js Date time picker";
        $scope.disabled = true;
        $scope.TimeList = [{ type: 'Last Week' }, { type: 'Last Month' }, { type: 'Last Quarter' }, { type: 'Last Year' }];
        $scope.fromdate = new Date();
        $scope.todate = new Date();

        $scope.GetTimeSpan = function (timespan) {

            $scope.disabled = false;
            if (timespan == 'Last Week') {
                $scope.fromdate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate() - 7);

                $scope.minstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate() + 7);

                $scope.minendDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxendDate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());
            }
            if (timespan == 'Last Month') {

                $scope.fromdate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth() - 1,
                    $scope.todate.getDate());

                $scope.minstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth() + 1,
                    $scope.fromdate.getDate());

                $scope.minendDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxendDate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());
            }
            if (timespan == 'Last Quarter') {
                $scope.fromdate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth() - 3,
                    $scope.todate.getDate());

                $scope.minstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth() + 3,
                    $scope.fromdate.getDate());

                $scope.minendDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxendDate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());
            }
            if (timespan == 'Last Year') {
                $scope.fromdate = new Date(
                    $scope.todate.getFullYear() - 1,
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());

                $scope.minstartDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxstartDate = new Date(
                    $scope.fromdate.getFullYear() + 1,
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.minendDate = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());

                $scope.maxendDate = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());
            }
        };

        //this.isOpen = false;
        $scope.showDate = function () {
            $http.post("/Customer/GetLogs", { startdt: $scope.fromdate, enddt: $scope.todate }).success(function (response) {
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
        }
        
    }]);