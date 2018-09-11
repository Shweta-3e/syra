SyraApp.controller("ChatBotViewController", ["$scope", "$http", "syraservice", "$state",
    function ($scope, $http, syraservice, $state) {

        $scope.MyChatBots = [];

        $scope.GetChatBots = function () {
            $http.get('/Customer/GetMyBots').success(function (data) {
                console.log(data);
                if (data.isSaved) {
                    $scope.MyChatBots = data.Data;
                } else {
                    window.location.href = ("Account/Login");
                }
            });
        };
        $scope.GetChatBots();


        $scope.Delete = function (id) {
            if (confirm('Are you sure ? You want to delete chatbot')) {
                $http.post("/Customer/Delete/", { id: id }).success(function (data) {
                    if (data.isSaved) {
                        syraservice.RecordStatusMessage("success", data.Message);
                        $scope.GetChatBots();
                    }
                    else {
                        syraservice.RecordStatusMessage("error", data.Message);
                    }
                });
            }
        };
       
    }
]);

SyraApp.controller("ChatBotAddController", ["$scope", "$http", "syraservice", "$state","$stateParams",
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