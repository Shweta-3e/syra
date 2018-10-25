SyraApp.controller("CustomerPlanController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.pageNumber = 0;
        $scope.pageSize = 10;

        $scope.GetCustomers = function () {
            $http.get("/Customer/GetCustomers").success(function (response) {
                console.log(response.Data);
                $scope.Customers = response.Data;
            });
        }
        $scope.GetCustomers();
    }]);

SyraApp.controller("CustomerDetailController", ["$scope", "$http", "syraservice", "$state","$stateParams",
    function ($scope, $http, syraservice, $state, $stateParams) {

        $scope.tab = 1;
        $scope.Chattab = 1;
        $scope.Id = $stateParams.Id;
        $scope.BotDetail = false;
        $scope.IsEditMode = false;
        $scope.BotQuestionAnswer = {};
        //$scope.BotDeployment.IsPlanActive = true;

        $scope.LoadCustomer = function () {
            $http.post("/Customer/GetCustomerById", { customerId: parseInt($scope.Id) }).success(function (response) {
                console.log(response.Data);
                $scope.Customer = response.Data;
            });
        }

        if ($scope.Id != undefined) {
            $scope.LoadCustomer();
        }

        
        $scope.isActiveTab = function (tab) {
            return $scope.tab == tab;
        };

        $scope.isActiveChatTab = function (tab) {
            return $scope.Chattab == tab;
        };

        $scope.Cancel = function () {
            $state.go("managecustomer");
        };

        $scope.GetLuisDomain = function () {
            $http.get('/Customer/GetLuisDomains').success(function (data) {
                $scope.LuisDomains = data.Data;
            });
        };
        $scope.GetLuisDomain();

        $scope.ViewBotDetails = function (chatbot) {
            $scope.BotDetail = true;
            $scope.BotDeployment = chatbot;
            console.log(chatbot);
        };

        $scope.Update = function () {
            $http.post("/Customer/UpdateProfile", { customerView: $scope.Customer }).success(function (response) {
                if (response.isSaved) {
                    syraservice.RecordStatusMessage("success", response.Message);
                    $scope.Customer = response.Data;
                } else {
                    syraservice.RecordStatusMessage("error", response.Message);
                }
            })
        };

        $scope.EditChatbot = function (Id) {
            $http.post("/Customer/GetChatBotEntry/", { id: Id }).success(function (data) {
                $scope.BotDeployment = data.Data;
                $scope.Chattab = 1;
                $scope.IsEditMode = true;
            });
        };

        $scope.AddMore = function () {
            if ($scope.BotQuestionAnswer.Question != null && $scope.BotQuestionAnswer.Answer != null) {
                $scope.BotDeployment.BotQuestionAnswers.push({
                    Question: $scope.BotQuestionAnswer.Question,
                    Answer: $scope.BotQuestionAnswer.Answer
                });
            }
            $scope.BotQuestionAnswer = {};
        };

        $scope.NextTab = function (Chattab) {
            $scope.Chattab = Chattab + 1;
            $scope.isActiveChatTab($scope.Chattab);
        };

        $scope.PreviousTab = function (Chattab) {
            $scope.Chattab = Chattab - 1;
            $scope.isActiveChatTab($scope.Chattab);
        };

        $scope.UpdateChatBot = function () {
            $http.post('/Customer/UpdateChatBot', { botdeploymentview: $scope.BotDeployment }).success(function (data) {
                if (data.IsSuccess) {
                    $scope.BotDeployment = {};
                    syraservice.RecordStatusMessage("success", data.Message);
                    $scope.IsEditMode = false;
                    $scope.LoadCustomer();
                } else {
                    syraservice.RecordStatusMessage("error", data.Message);
                }
            });
        };


        $scope.Edit = function (item) {
            $scope.BotQuestionAnswer = item;
            $scope.BotQuestionAnswer.IsEditMode = true;
        };

        $scope.Remove = function (index) {
            $scope.BotDeployment.BotQuestionAnswers.splice(index, 1);
        };

        $scope.UpdateBotQuestionAnswer = function (data) {
            $scope.BotQuestionAnswer = {};
        };

        $scope.Delete = function (id) {
            if (confirm('Are you sure ? You want to delete chatbot')) {
                $http.post("/Customer/Delete/", { id: id }).success(function (data) {
                    if (data.IsSuccess) {
                        syraservice.RecordStatusMessage("success", data.Message);
                        $scope.LoadCustomer();
                    }
                    else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };

        $scope.CopyToClipBoard = function () {
            var copyText = document.getElementById("txtEmbeddedScript");
            copyText.select();
            document.execCommand("copy");
        }
 }]);