SyraApp.controller("ChatBotViewController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.MyChatBots = [];
        $scope.pageSize = 10;
        $scope.pageNumber = 0;
        $scope.BotDeployment = {};
        $scope.BotDetail = false;
        $scope.CustomerPlan = {};

        $scope.GetCurrentUser = function () {
            $http.post('/Customer/GetCurrentUser'
               ).success(function (data) {
                   $scope.Customer = data.Data;

                   $http.post('/Customer/GetCustomerActivePlan',
                       {
                           customerId: $scope.Customer.Id
                       }).success(function (data) {
                           console.log(data.Data);
                           $scope.CustomerPlan = data.Data;
                       });
               });
        }
        $scope.GetCurrentUser();

        $scope.GetChatBots = function () {
            $http.post('/Customer/GetMyBots',
                {
                    pagesize: $scope.pageSize,
                    pageno: $scope.pageNumber
                }).success(function (data) {
                    if (data.isSaved) {
                        $scope.MyChatBots = data.Entities;
                        $scope.HasNext = data.HasNext;
                        $scope.HasPrevious = data.HasPrevious;
                        $scope.TotalPages = data.TotalPages;
                    } else {
                        window.location.href = ("Account/Login");
                    }
                });
        };
        $scope.GetChatBots();

        $scope.NextRecords = function () {
            if ($scope.pageNumber < $scope.TotalPages) {
                $scope.pageNumber = $scope.pageNumber + 1;
            }
            $scope.GetChatBots();
        };

        $scope.PreviousRecords = function () {
            $scope.pageNumber = $scope.pageNumber - 1;
            if ($scope.pageNumber <= 0) {
                $scope.pageNumber = 0;
            }
            $scope.GetChatBots();
        };

        $scope.ViewBotDetails = function (chatbot) {
            $scope.BotDetail = true;
            $scope.BotDeployment = chatbot;
        };

        $scope.Delete = function (id) {
            if (confirm('Are you sure ? You want to delete chatbot')) {
                $http.post("/Customer/Delete/", { id: id }).success(function (data) {
                    if (data.isSaved) {
                        console.log("Called")
                        syraservice.RecordStatusMessage("success", data.Message);
                        $scope.GetChatBots();
                    }
                    else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };

        $scope.Cancel = function () {
            $scope.BotDetail = false;
            $state.go("chatbot");
        };

    }
]);

SyraApp.controller("ChatBotAddController", ["$scope", "$http", "syraservice", "$state", "$stateParams",
    function ($scope, $http, syraservice, $state, $stateParams) {

        $scope.BotDeployment = {};
        $scope.IsEditMode = false;
        $scope.tab = 1;
        $scope.BotDeployment.BotQuestionAnswers = [];
        $scope.BotQuestionAnswer = {};

        $scope.id = $stateParams.id;

        if ($scope.id == undefined) {
            $scope.IsEditMode = false;
        } else {
            $scope.IsEditMode = true;
        }

        if ($scope.IsEditMode) {
            $http.post("/Customer/GetChatBotEntry/", { id: $scope.id }).success(function (data) {
                $scope.BotDeployment = data.Data;
            });
        }

        $scope.GetLuisDomain = function () {
            $http.get('/Customer/GetLuisDomains').success(function (data) {
                $scope.LuisDomains = data.Data;
            });
        };
        $scope.GetLuisDomain();

        $scope.CreateChatBot = function () {
            if ($scope.IsEditMode) {
                $http.post('/Customer/UpdateChatBot', { botdeploymentview: $scope.BotDeployment }).success(function (data) {
                    if (data.isSaved) {
                        $scope.BotDeployment = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("chatbot");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            } else {
                $http.post('/Customer/CreateNewChatBot', { botdeployment: $scope.BotDeployment }).success(function (data) {
                    if (data.isSaved) {
                        $scope.BotDeployment = {};
                        syraservice.RecordStatusMessage("success", data.Message);
                        $state.go("chatbot");
                    } else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };

        $scope.Cancel = function () {
            $state.go("chatbot");
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

        $scope.AddMore = function () {
            if ($scope.BotQuestionAnswer.Question != null && $scope.BotQuestionAnswer.Answer != null) {
                $scope.BotDeployment.BotQuestionAnswers.push({
                    Question: $scope.BotQuestionAnswer.Question,
                    Answer: $scope.BotQuestionAnswer.Answer
                });
            }
            $scope.BotQuestionAnswer = {};
        };

        $scope.Edit = function (item) {
            $scope.BotQuestionAnswer = item;
            $scope.BotQuestionAnswer.IsEditMode = true;
        };

        $scope.Remove = function (index) {
            $scope.BotDeployment.BotQuestionAnswers.splice(index, 1);
        };

        $scope.Update = function (data) {
            $scope.BotQuestionAnswer = {};
        }
    }
]);