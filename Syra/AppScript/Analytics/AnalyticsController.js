SyraApp.controller("AnalyticsController", ["$scope", "$http", "syraservice", "$sce","$state",
    function ($scope, $http, syraservice, $sce, $state) {

        $scope.fromdate = new Date();
        $scope.todate = new Date();

        $scope.IsEditMode = false;
        $scope.tab = 1;

        $scope.minstartDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth() - 2,
            $scope.fromdate.getDate());

        $scope.maxstartDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth(),
            $scope.fromdate.getDate());

        $scope.minendDate = new Date(
            $scope.fromdate.getFullYear(),
            $scope.fromdate.getMonth() - 2,
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
            
            //var url = "/Customer/GetAnalytics";
            //var userqueryurl = "/Customer/UserQuery";
            //var clickedlinkurl = "/Customer/GetClickedLink";
            var botreplyurl = "/Customer/BotReply";
            //var timeurl = "/Customer/LowPeakTime";
            //var usadataurl = "/Customer/GetUSAAnalytics";

            //$http.post(usadataurl, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.GetUsaMap(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

            //$http.post(url, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.GetWorldAnalysis(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

            //$http.post(clickedlinkurl, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.GetClickedLink(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

            //$http.post(userqueryurl, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.GetUserQuery(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

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
            var data = worlddata.Data;
            $('#container').highcharts('Map', {
                chart: {
                    map: 'custom/world',
                    borderWidth: 1,
                    width: 1000
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
                    color: '#6d5a59',
                    showInLegend: false,
                    enableMouseTracking: false
                }]
            });
        };
        $scope.GetUsaMap = function (usadata) {
            var data = usadata.Data,
                separators = Highcharts.geojson(Highcharts.maps['countries/us/us-all'], 'mapline')
            $('#usa-container').highcharts('Map', {
                    chart: {
                        map: 'countries/us/us-all',
                        borderWidth: 1,
                        width: 1000
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
            $scope.UserQueries = userquerydata.Data.AllResponse;
            var data = userquerydata.Data.firstTenArrivals;
            Highcharts.chart('container_query', {
                chart: {
                    type: 'column',
                    borderWidth: 1,
                    width: 1000
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
                        pointWidth: 30
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
            var linkdata = [];
            var finaldata = [];
            var arr = [];
            for (var item in clickedlinkdata) {
                linkdata.push(clickedlinkdata[item]);
            }
            finaldata.push(linkdata.slice(2, 3));
            for (var i = 0; i < finaldata.length; i++) {
                for (var j = 0; j <= i; j++) {
                    arr.push(finaldata[i][j]);
                }
            }
            Highcharts.chart('goalconversion', {
                chart: {
                    type: 'column',
                    borderWidth: 1,
                    width: 1000
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
                        pointWidth: 30
                    },
                    column: {
                        pointPadding: 0,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: 'Population',
                    data: arr[0],
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
        $scope.ShowQuery = function () {
            angular.forEach($scope.UserQueries, function (query) {
                query.BotAnswers1 = $sce.trustAsHtml(query.BotAnswers);
            });
        };
    }]);