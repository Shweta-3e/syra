SyraApp.service("syraservice",
["$rootScope", "$http", "$interval", "$filter", "$document", "$q",
    function ($rootScope, $serviceHttp, $interval, $filter, $document, $q) {

        this.RecordStatusMessage = function (messageType, messageFromServer) {
            var msg = " ";
            var position = "right";
            var autohide = true;
            if (messageType == "success") {
                msg = msg + messageFromServer;
            }
            if (messageType == "warning") {
                msg = msg + messageFromServer;
            }

            if (messageType == "error") {
                msg = msg + messageFromServer;
            }
            notif({
                msg: "<span  class='errormessage'><b class='capitalise'>" + messageType + "! :</b>" + msg + "</span>",
                type: messageType,
                position: position,
                fade: true,
                autohide: autohide,
                opacity: 1,
                multiline: true
            });
        };


        this.GetUserProfile = function () {
            var d = $q.defer();

            $serviceHttp.post('/Customer/GetCustomerActivePlan').success(function (data) {
                if (data.IsSuccess) {
                    if (data.RedirectToLogin) {
                        d.resolve("");
                        window.location.href = "Account/Login";
                    } else {
                        d.resolve(data.Data);
                    }
                } else {
                    //redirect to login or ivalid response
                    d.resolve("");
                }
            });

            return d.promise;
        };

    }
]);