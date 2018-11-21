SyraApp.controller("AnalyticsController", ["$scope", "$http", "syraservice", "$sce","$state",
    function ($scope, $http, syraservice, $sce, $state) {

        $scope.fromdate = new Date();
        $scope.todate = new Date();

        $scope.IsEditMode = false;
        $scope.tab = 1;

        $scope.minstartDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth() - 3,
            $scope.fromdate.getDate());

        $scope.maxstartDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth(),
            $scope.fromdate.getDate());

        $scope.minendDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth() - 3,
            $scope.fromdate.getDate());

        $scope.maxendDate = new Date(
            $scope.todate.getFullYear(),
            $scope.todate.getMonth(),
            $scope.todate.getDate());

        $scope.isActiveTab = function (tab) {
            return $scope.tab == tab;
        };

        $scope.GetLowPeakTime = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#timespinner").show();
            var timingurl = "/Customer/LowPeakTime";
            $http.post(timingurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.TimingAnalysis(response);
                    $("#timespinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };
        //functions to call active tabs
        $scope.GetBotReply = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#botresponsespinner").show();
            var botreplyurl = "/Customer/BotReply";
            $http.post(botreplyurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.BotResponse(response);
                    $("#botresponsespinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };

        $scope.GetQuestionsAsked = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#spinner").show();
            var userqueryurl = "/Customer/UserQuery";
            $http.post(userqueryurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetUserQuery(response);
                    $("#spinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };

        $scope.GetLinks = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#goalconversionspinner").show();
            var clickedlinkurl = "/Customer/GetClickedLink";
            $http.post(clickedlinkurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetClickedLink(response);
                    $("#goalconversionspinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };

        $scope.GetUSAAnalysis = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#usaspinner").show();
            var usadataurl = "/Customer/GetUSAAnalytics";
            $http.post(usadataurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetUsaMap(response);
                    $("#usaspinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };

        $scope.GetWorld = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            $("#worldspinner").show();
            var url = "/Customer/GetAnalytics";
            $http.post(url, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetWorldAnalysis(response);
                    $("#worldspinner").hide();
                } else {
                    alert("Something went wrong");
                }
            });
        };

        //functions to call api
        $scope.GetWorldAnalysis = function (worlddata) {
            var data = worlddata.Data._data;
            var country = "";
            $('#container').highcharts('Map', {
                chart: {
                    map: 'custom/world',
                    width: 1200,
                    height:500
                },
                title: {
                    text: 'World Analysis',
                    style: {
                        color: '#8d3052',
                        fontWeight: 'bold',
                        fontSize: '24px'
                    }
                },
                mapNavigation: {
                    enabled: true,
                    buttonOptions: {
                        verticalAlign: 'bottom'
                    }
                },
                colorAxis: {
                    min: 1,
                    max: 500,
                    type: 'logarithmic',
                    minColor: '#8c0c09',
                    maxColor: '#11e056',
                    
                },
                plotOptions: {
                    series: {
                        pointWidth: 30,
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    var tablerow = "<tbody>";
                                    var usa = " of America";
                                    var srno = 1;
                                    //console.log(worlddata.Data.AllResponse);
                                    for (var i = 0; i < worlddata.Data.AllResponse.length; i++) {
                                        if (worlddata.Data.AllResponse[i].Country == "United States") {
                                            country = worlddata.Data.AllResponse[i].Country + usa;
                                        }
                                        else {
                                            country = worlddata.Data.AllResponse[i].Country;
                                        }
                                        if (country == e.point.name) {
                                            tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + srno + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                            srno++;
                                        }
                                    }
                                    tablerow += "</tbody>";
                                    //console.log(tablerow)
                                    var tabledata = "<br><br><table id='worldmap' dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Sr. No</th>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                        tablerow + "</table>";
                                    var table = tabledata;
                                    var selectlable = "<label class='control-label col - sm - 2' style='font - family: 'Times New Roman', Times, serif; font - size: large; fo; font - weight: normal;'>Show Entries </label>"
                                        + "<div class='form-group'><select class='form-control' name='state' id='worldmaxRows' style='width:20%'>"
                                        + "<option value='5000'>Show All Rows</option><option value='5'>5</option><option value='10'>10</option>"
                                        + "<option value='15'>15</option><option value='20'>20</option><option value='50'>50</option>"
                                        + "<option value='70'>70</option><option value='100'>100</option></select></div>";
                                    var tablestyle = "<style></style>";
                                    var pagination = "<div class='pagination-container' ><nav><ul class='pagination' id='worldpagination'>"
                                        + "<li data-page='prev' ><span>Previous<span class='sr-only'>(current)</span></span></li>"
                                        + "<li data-page='next' id='prev'><span> Next <span class='sr-only'>(current)</span></span>"
                                        + "</li></ul></nav></div>";
                                    var modal = "<button type='button' id='showworlddetail' onclick='disableButton(this)' class='btn btn - primary' data-toggle='modal' data-target='#worldmodal'style='margin-left: 10%;color: white;background-color: #3e8e41;'>Show Details</button >"
                                        + "<div class='modal fade' id='worldmodal' role='dialog' aria-labelledby='exampleModalLongTitle' aria-hidden='true'>"
                                        + "<div class='modal-dialog modal-dialog-centered' style='width:80%;padding-top:70px;position:unset' role='document'>"
                                        + "<div class='modal-content' style='margin-left:-50px;'><div class='modal-header'>"
                                        + "<h5 class='modal-title' id='worldmodal' style='text-align:center;font - family: Times New Roman; font - size: large; fo; font - weight: bold;'>" + "Query asked from" + " " + "country" + " " + " : " + e.point.name + "</h5>"
                                        + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                                        + "<span aria-hidden='true'>&times;</span>"
                                        + "</button></div><div class='modal-body' style='margin-bottom: 5%;'>"
                                        + selectlable + table + pagination
                                        + "</div><div class='modal-footer'>"
                                        + "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>"
                                        + "</div></div></div></div>";
                                    var worldTable = document.getElementById("worldTable");
                                    worldTable.innerHTML = modal;
                                    getWorldAnalysisPagination('#worldmap');
                                }
                            }
                        }
                    },
                    column: {
                        pointPadding: 0,
                        borderWidth: 0
                    }
                },
                series: [{
                    data: data,
                    nullColor: '#c4d5f2',
                    mapData: Highcharts.maps['custom/world'],
                    joinBy: ['iso-a3', 'code3'],
                    name: 'Question asked',
                    states: {
                        hover: {
                            color: '#42f4aa'
                        }
                    },
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }, {
                    name: 'Separators',
                    type: 'mapline',
                    data: Highcharts.geojson(Highcharts.maps['custom/world'], 'mapline'),
                    color: '#13223a',
                    showInLegend: false,
                    enableMouseTracking: false
                }]
            });
        };

        $scope.TimingAnalysis = function (timedata) {
            var data = timedata.Data.Epochtime;
            Highcharts.chart('timing-container', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Time Analysis',
                    style: {
                        color: '#8d3052',
                        fontWeight: 'bold',
                        fontSize: '24px'
                    }
                },
                xAxis: {
                    type: 'datetime'
                },
                yAxis: {
                    title: {
                        text: 'No of time question asked ',
                        style: {
                            color: '#3c1414',
                            fontWeight: 'bold',
                            fontSize: '12px'
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: {
                                x1: 0,
                                y1: 0,
                                x2: 0,
                                y2: 1
                            },
                            stops: [
                                [0, Highcharts.getOptions().colors[0]],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (event) {
                                }
                            }
                        },
                        threshold: null
                    }
                },
                series: [{
                    type: 'area',
                    name: 'Questions asked in a day',
                    data: data
                }]
            });
        };

        $scope.GetUsaMap = function (usadata) {
            var data = usadata.Data.usadata,
                separators = Highcharts.geojson(Highcharts.maps['countries/us/us-all'], 'mapline'),
                allresponse = usadata.Data.AllResponse;
                $('#usa-container').highcharts('Map', {
                    chart: {
                        map: 'countries/us/us-all',
                        width: 1100,
                        height: 500
                    },
                    title: {
                        text: 'USA Analysis',
                        style: {
                            color: '#8d3052',
                            fontWeight: 'bold',
                            fontSize: '24px'
                        }
                    },
                    mapNavigation: {
                        enabled: true,
                        buttonOptions: {
                            verticalAlign: 'bottom'
                        }
                    },
                    colorAxis: {
                        min: 1,
                        max: 500,
                        type: 'logarithmic',
                        minColor: '#bf1515',
                        maxColor: '#4286f4',
                    },
                    plotOptions: {
                        series: {
                            pointWidth: 30,
                            cursor: 'pointer',
                            point: {
                                events: {
                                    click: function (e) {
                                        var country = "";
                                        var tablerow = "<tbody>";
                                        var srno = 1;
                                        for (var i = 0; i < usadata.Data.AllResponse.length; i++) {
                                            if (usadata.Data.AllResponse[i].Region == e.point.name) {
                                                tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + srno + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                                srno++;
                                            }
                                        }
                                        tablerow += "</tbody>";
                                        var tabledata = "<br><br><table id='usamap' dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                            "<th class='text-align-center' style='width:100px;border-left:solid 1px #adbbd1'>Sr. No</th>"+
                                            "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                            "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                            tablerow + "</table>";
                                        var table = tabledata;
                                        var selectlable = "<label class='control-label col - sm - 2' style='text-align:center;font - family: 'Times New Roman', Times, serif; font - size: large; fo; font - weight: normal;'>Show Entries </label>"
                                            + "<div class='form-group'><select class='form-control' name='state' id='usamaxRows' style='width:20%'>"
                                            + "<option value='5000'>Show All Rows</option><option value='5'>5</option><option value='10'>10</option>"
                                            + "<option value='15'>15</option><option value='20'>20</option><option value='50'>50</option>"
                                            + "<option value='70'>70</option><option value='100'>100</option></select></div>";
                                        var tablestyle = "<style></style>";
                                        var pagination = "<div class='pagination-container' ><nav><ul class='pagination' id='usapagination'>"
                                            + "<li data-page='prev' ><span>Previous<span class='sr-only'>(current)</span></span></li>"
                                            + "<li data-page='next' id='prev'><span> Next <span class='sr-only'>(current)</span></span>"
                                            + "</li></ul></nav></div>";
                                        var modal = "<button type='button' id='showusadetail' onclick='disableButton(this)' class='btn btn - primary' data-toggle='modal' data-target='#usamodal' style='margin-left: 10%;color: white;background-color: #3e8e41;'>Show Details</button >"
                                            + "<div class='modal fade' id='usamodal' role='dialog' aria-labelledby='exampleModalLongTitle' aria-hidden='true'>"
                                            + "<div class='modal-dialog modal-dialog-centered' style='width:80%;padding-top:70px;position:unset' role='document'>"
                                            + "<div class='modal-content' style='margin-left:-50px;'><div class='modal-header'>"
                                            + "<h5 class='modal-title' id='usamodal' style='text-align:center;font - family: Times New Roman ; font - size: large; fo; font - weight: bold;'>" + "Query asked from USA" + " " + "region" + " " + " : " + e.point.name + "</h5>"
                                            + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                                            + "<span aria-hidden='true'>&times;</span>"
                                            + "</button></div><div class='modal-body' style='margin-bottom: 5%;'>"
                                            + selectlable + table + pagination
                                            + "</div><div class='modal-footer'>"
                                            + "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>"
                                            + "</div></div></div></div>";
                                        var usaTable = document.getElementById("usaTable");
                                        usaTable.innerHTML = modal;
                                        getUSAAnalysisPagination('#usamap');
                                    }
                                }
                            }
                        },
                        column: {
                            pointPadding: 0,
                            borderWidth: 0
                        }
                    },
                    series: [{
                        data: data,
                        mapData: Highcharts.maps['countries/us/us-all'],
                        nullColor: '#98d187',
                        joinBy: 'hc-key',
                        name: 'Question asked',
                        states: {
                            hover: {
                                color: '#BADA55'
                            }
                        },
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}'
                        }
                    },
                    {
                        type: 'mapline',
                        data: separators,
                        color: 'silver',
                        enableMouseTracking: false,
                        animation: {
                            duration: 500
                        }
                    },
                    {
                        name: 'Separators',
                        type: 'mapline',
                        data: Highcharts.geojson(Highcharts.maps['countries/us/us-all'], 'mapline'),
                        color: 'silver',
                        showInLegend: false,
                        enableMouseTracking: false
                    }]
                });
        };

        $scope.GetUserQuery = function (userquerydata) {
            var data = userquerydata.Data.firstTenArrivals,
                category = [];
            for (var item = 0; item < data.length; item++) {
                for (var query = 0; query < 1; query++)
                    category.push(data[item][query]);
            }
            Highcharts.chart('container_query', {
                chart: {
                    type: 'column',
                    width: 1000,
                    height: 450,
                    spacingLeft: 100,
                },
                title: {
                    text: 'Questions Analysis',
                    style: {
                        color: '#8d3052',
                        fontWeight: 'bold',
                        fontSize: '24px'
                    }
                },
                xAxis: {
                    type: 'category',
                    categories: category,
                    title: {
                        text: 'Questions asked',
                        style: {
                            color: '#3c1414',
                            fontWeight: 'bold',
                            fontSize: '16px'
                        }
                    },
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Count of asked Questions',
                        style: {
                            color: '#3c1414',
                            fontWeight: 'bold',
                            fontSize: '16px'
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    pointFormat: 'Questions Asked: <b>{point.y:.1f}</b>'
                },
                plotOptions: {
                    series: {
                        pointWidth: 30,
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var tabelrow = "<tbody>";
                                    var dataset = [];
                                    var srno = 1;
                                    for (var i = 0; i < userquerydata.Data.AllResponse.length; i++) {
                                        if (userquerydata.Data.AllResponse[i].UserQuery == this.category) {
                                            tabelrow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + srno + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:650px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                            srno++;
                                        }
                                    }
                                    tabelrow += "</tbody>";
                                    var tabeldata = "<br><br><table id='userquery' class='table table-responsive table - bordered table - striped' style='width:100%'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:100px;border-left:solid 1px #adbbd1'>Sr. No</th>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                        tabelrow + "</table>";
                                    var tabel = tabeldata;
                                    var selectlable = "<label class='control-label col - sm - 2' style='text-align:center;font - family: 'Times New Roman', Times, serif; font - size: large; fo; font - weight: normal;'>Show Entries </label>"
                                        +"<div class='form-group'><select class='form-control' name='state' id='querymaxRows' style='width:15%'>"
                                        +"<option value='5000'>Show All Rows</option><option value='5'>5</option><option value='10'>10</option>"
                                        +"<option value='15'>15</option><option value='20'>20</option><option value='50'>50</option>"
                                        + "<option value='70'>70</option><option value='100'>100</option></select></div>";
                                    var pagination = "<div class='pagination-container' style='margin-bottom:5%;'><nav><ul class='pagination' id='pagemark'>"
                                        + "<li data-page='prev' ><span>Previous<span class='sr-only'>(current)</span></span></li>"
                                        + "<li data-page='next' id='prev'><span> Next <span class='sr-only'>(current)</span></span>"
                                        + "</li></ul></nav></div>";
                                    var dvTable = document.getElementById("dvTable");
                                    var modal = "<button type='button' id='showdetail' onclick='disableButton(this)' class='btn btn - primary' data-toggle='modal' data-target='#exampleModalLong' style='margin-left: 10%;color: white;background-color: #3e8e41;'>Show Details</button >"
                                        +"<div class='modal fade' id='exampleModalLong' role='dialog' aria-labelledby='exampleModalLong' aria-hidden='true'>"
                                        + "<div class='modal-dialog modal-dialog-centered' style='width:80%;padding-top:70px;position:unset' role='document'>"
                                        + "<div class='modal-content' style='margin-left:-50px;'><div class='modal-header'>"
                                        + "<h5 class='modal-title' id='exampleModalLong' style='text-align:center;font - family: Times New Roman ; font - size: large; fo; font - weight: bold;'>" +"User"+" "+"Query"+" "+" : " + this.category + "</h5>"
                                        + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                                        + "<span aria-hidden='true'>&times;</span>"
                                        + "</button></div><div class='modal-body' style='margin-bottom: 5%;'>"
                                        + selectlable + tabel + pagination
                                        + "</div><div class='modal-footer'>"
                                        + "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>"
                                        + "</div></div></div></div>";
                                    dvTable.innerHTML = modal;
                                    getUserQueryPagination('#userquery');
                                }
                            }
                        }
                    },
                    column: {
                        pointPadding: 0,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: 'Population',
                    data: data,
                    color: '#4195f4',
                    dataLabels: {
                        enabled: true,
                        rotation: -90,
                        color: '#FFFFFF',
                        align: 'right',
                        format: '{point.y:.1f}', // one decimal
                        y: 10, // 10 pixels down from the top
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }]
            });
        };

        $scope.GetClickedLink = function (clickedlinkdata) {
            var data = clickedlinkdata.Data.firstTenArrivals,
                category = [];
            for (var item = 0; item < data.length; item++) {
                for (var link = 0; link < 1; link++)
                    category.push(data[item][link]);
            }
            Highcharts.chart('goalconversion', {
                chart: {
                    type: 'column',
                    width: 1000,
                    height: 450,
                    spacingLeft: 100,
                },
                title: {
                    text: 'Clicked Links',
                    style: {
                        color: '#8d3052',
                        fontWeight: 'bold',
                        fontSize: '24px'
                    }
                },
                xAxis: {
                    type: 'category',
                    categories: category,
                    title: {
                        text: 'Links Clicked',
                        style: {
                            color: '#3c1414',
                            fontWeight: 'bold',
                            fontSize: '16px'
                        }
                    },
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Count of clicked links',
                        style: {
                            color: '#3c1414',
                            fontWeight: 'bold',
                            fontSize: '16px'
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    pointFormat: 'Link clicked: <b>{point.y:.1f}</b>'
                },
                plotOptions: {
                    series: {
                        pointWidth: 30,
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var tablerow = "<tbody>";
                                    var srno = 1;
                                    for (var i = 0; i < clickedlinkdata.Data.AllResponse.length; i++) {
                                        if (clickedlinkdata.Data.AllResponse[i].ClickedLink == this.category) {
                                            tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + srno + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].Region + "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].ClickedLink + "</td>" + "</tr>";
                                            srno++;
                                        }
                                    }
                                    tablerow += "</tbody>";
                                    var tabledata = "<br><br><table id='goalconversiontable' dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:100px;border-left:solid 1px #adbbd1'>Sr. No</th>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Clicked Link</th></tr></thead>"+
                                        tablerow + "</table>";
                                    var table = tabledata;
                                    var selectlable = "<label class='control-label col - sm - 2' style='text-align:center;font - family: 'Times New Roman', Times, serif; font - size: large; fo; font - weight: normal;'>Show Entries </label>"
                                        + "<div class='form-group'><select class='form-control' name='state' id='maxRows' style='width:15%'>"
                                        + "<option value='5000'>Show All Rows</option><option value='5'>5</option><option value='10'>10</option>"
                                        + "<option value='15'>15</option><option value='20'>20</option><option value='50'>50</option>"
                                        + "<option value='70'>70</option><option value='100'>100</option></select></div>";
                                    var pagination = "<div class='pagination-container' ><nav><ul class='pagination' id='linkpagination'>"
                                        + "<li data-page='prev' ><span>Previous<span class='sr-only'>(current)</span></span></li>"
                                        + "<li data-page='next' id='prev'><span> Next <span class='sr-only'>(current)</span></span>"
                                        + "</li></ul></nav></div>";
                                    var modal = "<button type='button' id='showlinkdetail' onclick='disableButton(this)' class='btn btn - primary' data-toggle='modal' data-target='#linkmodal'style='margin-left: 10%;color: white;background-color: #3e8e41;'>Show Details</button >"
                                        + "<div class='modal fade' id='linkmodal' role='dialog' aria-labelledby='linkmodal' aria-hidden='true'>"
                                        + "<div class='modal-dialog modal-dialog-centered' style='width:80%;padding-top:70px;position:unset' role='document'>"
                                        + "<div class='modal-content' style='margin-left:-50px;'><div class='modal-header'>"
                                        + "<h5 class='modal-title' id='linkmodal' style='text-align:center;font - family: Times New Roman; font - size: large; fo; font - weight: bold;'>" + "User clicked" + " " + "link" + " " + " : " + this.category + "</h5>"
                                        + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                                        + "<span aria-hidden='true'>&times;</span>"
                                        + "</button></div><div class='modal-body' style='margin-bottom: 5%;'>"
                                        + selectlable + table + pagination
                                        + "</div><div class='modal-footer'>"
                                        + "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>"
                                        + "</div></div></div></div>";
                                    var dvTable = document.getElementById("linkTable");
                                    dvTable.innerHTML = modal;
                                    getGoalConversionPagination('#goalconversiontable');
                                }
                            }
                        }
                    },
                    column: {
                        pointPadding: 0,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: 'Population',
                    data: data,
                    color: '#f4a742',
                    dataLabels: {
                        enabled: true,
                        rotation: -90,
                        color: '#FFFFFF',
                        align: 'right',
                        format: '{point.y:.1f}',
                        y: 10,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }]
            });
        };

        $scope.BotResponse = function (response) {

            $scope.BotReplyData = response.Data;

            Highcharts.chart('botreply', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Bot Response',
                    style: {
                        color: '#8d3052',
                        fontWeight: 'bold',
                        fontSize: '24px'
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var tabelrow = "<tbody>";
                                    var botreplyflag = 'Bot failed to reply';
                                    var srno = 1;
                                    for (var i = 0; i < response.Data.AllQuestions.length; i++) {
                                        if (response.Data.AllQuestions[i].BotResponse == this.name) {
                                            if (this.name == 'Successful Response') {
                                                botreplyflag = 'Bot replied successfully';
                                            }
                                            else {
                                                botreplyflag = 'Bot failed to reply';
                                            }
                                            tabelrow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + srno + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].UserQuery + "</td><td class='text-align-left' style='width:650px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + response.Data.AllQuestions[i].BotAnswers + "</td>" + "</tr>";
                                            srno++;
                                        }
                                        
                                    }
                                    tabelrow += "</tbody>";
                                    //console.log(tabelrow);
                                    var tabeldata = "<br><br><table id='botreplytable' class='table table-responsive table - bordered table - striped' style='width:100%'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                            "<th class='text-align-center' style='width:100px;border-left:solid 1px #adbbd1'>Sr. No</th>"+
                                            "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                            "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                            tabelrow + "</table>";
                                        var tabel = tabeldata;
                                        var selectlable = "<label class='control-label col - sm - 2' style='font - family: 'Times New Roman', Times, serif; font - size: large; fo; font - weight: normal;'>Show Entries </label>"
                                            + "<div class='form-group'><select class='form-control' name='state' id='botresponsemaxRows' style='width:15%'>"
                                            + "<option value='5000'>Show All Rows</option><option value='5'>5</option><option value='10'>10</option>"
                                            + "<option value='15'>15</option><option value='20'>20</option><option value='50'>50</option>"
                                            + "<option value='70'>70</option><option value='100'>100</option></select></div>";
                                        var pagination = "<div class='pagination-container' style='margin-bottom:5%;'><nav><ul class='pagination' id='botresponsepagination'>"
                                            + "<li data-page='prev' ><span>Previous<span class='sr-only'>(current)</span></span></li>"
                                            + "<li data-page='next' id='prev'><span> Next <span class='sr-only'>(current)</span></span>"
                                            + "</li></ul></nav></div>";
                                        var dvTable = document.getElementById("botresponse");
                                    var modal = "<button type='button' id='showdetail' onclick='disableButton(this)' class='btn btn - primary' data-toggle='modal' data-target='#botresponsemodal' style='margin-left: 10%;color: white;background-color: #3e8e41;'>Show Details</button >"
                                            + "<div class='modal fade' id='botresponsemodal' role='dialog' aria-labelledby='botresponsemodal' aria-hidden='true'>"
                                            + "<div class='modal-dialog modal-dialog-centered' style='width:80%;padding-top:70px;position:unset' role='document'>"
                                            + "<div class='modal-content' style='margin-left:-50px;'><div class='modal-header'>"
                                            + "<h5 class='modal-title' id='botresponsemodal' style='text-align:center;text-align:center;font - family: Times New Roman; font - size: large; font-weight: bold;'>" + botreplyflag + "</h5>"
                                            + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                                            + "<span aria-hidden='true'>&times;</span>"
                                            + "</button></div><div class='modal-body'>"
                                            + selectlable + tabel + pagination
                                            + "</div><div class='modal-footer'>"
                                            + "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button>"
                                            + "</div></div></div></div>";
                                        dvTable.innerHTML = modal;
                                    getBotResponsePagination('#botreplytable');
                                }
                            }
                        },
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
                                fontSize: '13px'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [
                        {
                            name: 'Successful Response',
                            y: ((response.Data.RightAnswers * 100) / response.Data.TotalQuestions),

                        }, {
                            name: 'Failed to Response/understand user question',
                            y: ((response.Data.WrongAnswers * 100) / response.Data.TotalQuestions),
                        }]
                }]
            });
        };

        $scope.ShowData = false;
        $scope.ShowQuestions = function (type) {
            $scope.ShowData = !$scope.ShowData;
            $scope.ReportType = type;
            var questions = $scope.BotReplyData.AllQuestions;
            angular.forEach($scope.BotReplyData.AllQuestions, function (row) {
                row.BotAnswers1 = $sce.trustAsHtml(row.BotAnswers);
            });
        };
    }]);