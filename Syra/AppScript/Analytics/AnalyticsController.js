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

        //functions to call active tabs
        $scope.GetBotReply = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            var botreplyurl = "/Customer/BotReply";
            $http.post(botreplyurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.BotResponse(response);
                } else {
                    alert("Something went wrong");
                }
            });
        };
        $scope.GetQuestionsAsked = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            var userqueryurl = "/Customer/UserQuery";
            $http.post(userqueryurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetUserQuery(response);
                } else {
                    alert("Something went wrong");
                }
            });
        };
        $scope.GetLinks = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            var clickedlinkurl = "/Customer/GetClickedLink";
            $http.post(clickedlinkurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetClickedLink(response);
                } else {
                    alert("Something went wrong");
                }
            });
        };
        $scope.GetUSAAnalysis = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            var usadataurl = "/Customer/GetUSAAnalytics";
            $http.post(usadataurl, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetUsaMap(response);
                } else {
                    alert("Something went wrong");
                }
            });
        };
        $scope.GetWorld = function () {
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            var url = "/Customer/GetAnalytics";
            $http.post(url, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    $scope.GetWorldAnalysis(response);
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
                    width: 1000,
                    height:600
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
                                    //console.log(worlddata.Data.AllResponse);
                                    for (var i = 0; i < worlddata.Data.AllResponse.length; i++) {
                                        if (worlddata.Data.AllResponse[i].Country == "United States") {
                                            country = worlddata.Data.AllResponse[i].Country + usa;
                                        }
                                        else {
                                            country = worlddata.Data.AllResponse[i].Country;
                                        }
                                        if (country == e.point.name) {
                                            tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + worlddata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                        }
                                    }
                                    tablerow += "</tbody>";
                                    //console.log(tablerow)
                                    var tabledata = "<br><br><table dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                        tablerow + "</table>";
                                    var table = tabledata;
                                    var worldTable = document.getElementById("worldTable");
                                    worldTable.innerHTML = table;

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
                    color: '#1c2638',
                    showInLegend: false,
                    enableMouseTracking: false
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
                        width: 1000,
                        height: 600
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
                                        for (var i = 0; i < usadata.Data.AllResponse.length; i++) {
                                            console.log(usadata.Data.AllResponse[i].Region);
                                            if (usadata.Data.AllResponse[i].Region == e.point.name) {
                                                tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + usadata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                            }
                                        }
                                        tablerow += "</tbody>";
                                        console.log(tablerow)
                                        var tabledata = "<br><br><table dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                            "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                            "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                            "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                            tablerow + "</table>";
                                        var table = tabledata;
                                        var usaTable = document.getElementById("usaTable");
                                        usaTable.innerHTML = table;
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
                    height:600
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
                                    for (var i = 0; i < userquerydata.Data.AllResponse.length; i++) {
                                        if (userquerydata.Data.AllResponse[i].UserQuery == this.category) {
                                            tabelrow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].Region + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].UserQuery + "</td><td class='text-align-left' style='width:650px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + userquerydata.Data.AllResponse[i].BotAnswers + "</td>" + "</tr>";
                                        }
                                    }
                                    tabelrow += "</tbody>";
                                    console.log(tabelrow);
                                    var tabeldata = "<br><br><table datatable='ng' dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>User Query</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Bot Answer</th></tr></thead>" +
                                        tabelrow +"</table>";
                                    var tabel = tabeldata;
                                    var dvTable = document.getElementById("dvTable");
                                    dvTable.innerHTML = tabel;
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
            console.log(data);
            for (var item = 0; item < data.length; item++) {
                for (var link = 0; link < 1; link++)
                    category.push(data[item][link]);
            }
            Highcharts.chart('goalconversion', {
                chart: {
                    type: 'column',
                    width: 1000,
                    height:600
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
                                    for (var i = 0; i < clickedlinkdata.Data.AllResponse.length; i++) {
                                        if (clickedlinkdata.Data.AllResponse[i].ClickedLink == this.category) {
                                            tablerow += "<tr>" + "<td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].LogDate + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].SessionId + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].IPAddress + "</td><td class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].Region  +  "</td><td class='text-align-left' style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1;border-bottom:solid 1px #adbbd1'>" + clickedlinkdata.Data.AllResponse[i].ClickedLink + "</td>" + "</tr>";
                                        }
                                    }
                                    tablerow += "</tbody>";
                                    console.log(tablerow);
                                    var tabledata = "<br><br><table dt-options='vm.dtOptions' dt-columns='vm.dtColumns' class='table table-responsive table - bordered table - striped' data-pagination='true'><thead>" + "<tr style='border-top: solid 1px #adbbd1;'>" +
                                        "<th class='text-align-center' style='width:150px;border-left:solid 1px #adbbd1'>Log Date</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Session Id</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>IP Address</th>" +
                                        "<th class='text-align-center'style='width:150px;border-left:solid 1px #adbbd1'>Region</th>" +
                                        "<th class='text-align-center'style='width:400px;border-left:solid 1px #adbbd1;border-right:solid 1px #adbbd1'>Clicked Link</th></tr></thead>"+
                                        tablerow + "</table>";
                                    var table = tabledata;
                                    var dvTable = document.getElementById("linkTable");
                                    dvTable.innerHTML = table;
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
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [
                        {
                            name: 'Successfull Response',
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