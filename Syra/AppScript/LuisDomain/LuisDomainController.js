SyraApp.controller("DomainViewController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.luisdomain = [];

        $scope.GetLuisDomain = function () {
            $http.get('/LuisDomains/GetDomain').success(function (data) {
                if (data.isSaved) {
                    $scope.luisdomain = data.Data;
                }
                else {
                    window.href.location('/Home');
                }
            });
            
        };
        $scope.GetLuisDomain();
    }
]);

SyraApp.controller("DomainAddController", ["$scope", "$http", "syraservice", "$state", "$stateParams",
    function ($scope, $http, syraservice, $state, $stateParams) {

        $scope.CreateLuis = {};
        $scope.IsEditMode = false;
        $scope.tab = 1;

        $scope.id = $stateParams.id;

        if ($scope.id == undefined) {
            $scope.IsEditMode = false;
        } else {
            $scope.IsEditMode = true;
        }
        $scope.LuisCreate = function () {
            $http.post('/LuisDomains/CreateDomain', { luis: $scope.CreateLuis }).success(function (data) {
                if (data.isSaved) {
                    $scope.CreateLuis = {};
                    syraservice.RecordStatusMessage("success", data.Message);
                    $state.go("luisdomain");
                } else {
                    syraservice.RecordStatusMessage("error", data.Message);
                }
            });
        }

        $scope.Cancel = function () {
            $state.go("luisdomain");
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