
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

    }
]);