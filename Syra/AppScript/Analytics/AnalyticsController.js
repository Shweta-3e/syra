﻿SyraApp.controller("AnalyticsController", ["$scope", "$http", "syraservice", "$sce","$state",
    function ($scope, $http, syraservice, $sce, $state) {

        $scope.fromdate = new Date();
        $scope.todate = new Date();

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

        $scope.GetReportData = function () {
            
            var startdate = $scope.fromdate;
            var enddate = $scope.todate;
            
            var url = "/Customer/GetAnalytics";
            var userqueryurl = "/Customer/UserQuery";
            var clickedlinkurl = "/Customer/GetClickedLink";
            var botreplyurl = "/Customer/BotReply";
            var timeurl = "/Customer/LowPeakTime";
            var usadataurl = "/Customer/GetUSAAnalytics";

            //$http.post(usadataurl, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.GetUsaMap(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

            $http.post(url, { startdt: startdate, enddt: enddate }).success(function (response) {
                if (response != null) {
                    console.log("Path is : ");
                    console.log(response);
                    $scope.GetWorldMap();
                    //$scope.GetUsaMap();
                } else {
                    alert("Something went wrong");
                }
            });

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

            //$http.post(botreplyurl, { startdt: startdate, enddt: enddate }).success(function (response) {
            //    if (response != null) {
            //        $scope.BotResponse(response);
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});

            //$http.post(timeurl, { }).success(function (response) {
            //    //console.log("Response is : "+ response);
            //    if (response != null) {
            //        $scope.TimeChart();
            //    } else {
            //        alert("Something went wrong");
            //    }
            //});
        };

        $scope.GetWorldMap = function () {
            $.getJSON('/AppScript/Analytics/Template/locations.json', function (data) {
                // Prevent logarithmic errors in color calulcation
                $.each(data, function () {
                    this.value = (this.value < 1 ? 1 : this.value);
                });
                // Initiate the chart
                Highcharts.mapChart('container-worldmap', {
                    chart: {
                        map: 'custom/world',
                        borderWidth: 1
                    },
                    title: {
                        text: 'World Based Analysis',
                        style: {
                            color: '#8d3052',
                            fontWeight: 'bold',
                            fontSize: '24px'
                        }
                    },
                    mapNavigation: {
                        enabled: true,
                        enableDoubleClickZoomTo: true
                    },
                    colorAxis: {
                        min: 1,
                        max: 500,
                        type: 'logarithmic',
                        minColor: '#f45041',
                        maxColor: '#4194f4',
                    },
                    series: [{
                        data: data,
                        nullColor: '#dbd072',
                        joinBy: ['iso-a3', 'code3'],
                        name: 'Questions asked',
                        states: {
                            hover: {
                                color: '#42f4aa'
                            }
                        },
                        tooltip: {
                            valueSuffix: ''
                        }
                    }]
                });
            });
        };

        $scope.GetUsaMap = function (usadata) {
           
            var finaldata = [];
            for (var item in usadata) {
                finaldata.push(usadata[item]);
            }
            //console.log("USA Data");
            //console.log(linkdata);
            
            var data = finaldata.slice(2, 3);
            $('#usa-container').highcharts('Map', {
                    chart: {
                        map: 'countries/us/us-all',
                        borderWidth: 1,
                        width: 1000
                    },
                    title: {
                        text: 'USA Based Analysis',
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
                        minColor: '#b1c9ef',
                        maxColor: '#4286f4',
                    },
                    series: [{
                        data: data,
                        mapData: Highcharts.maps['countries/us/us-all'],
                        nullColor: '#9fe079',
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
                    }, {
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
            var linkdata = [];
            var finaldata = [];
            var arr = [];
            
            for (var item in userquerydata) {
                linkdata.push(userquerydata[item]);
            }
            finaldata.push(linkdata.slice(2, 3));
            for (var i = 0; i < finaldata.length; i++) {
                for (var j = 0; j <= i; j++) {
                    console.log(finaldata[i][j]);
                    arr.push(finaldata[i][j]);
                }
            }
            //console.log("User Query Response is : ");
            //console.log(arr);
            Highcharts.chart('container_query', {
                chart: {
                    type: 'column',
                    borderWidth: 1,
                    width: 1000
                },
                title: {
                    text: 'Most Common Questions Asked',
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
                    data: arr[0],
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

        $scope.TimeChart = function () {
            $.getJSON('/AppScript/Analytics/Template/Epochtime.json', function (data) {
                    Highcharts.chart('time_container', {
                        chart: {
                            zoomType: 'x',
                            borderWidth: 1,
                            width: 1000,
                        },
                        title: {
                            text: 'Peak and Low Time',
                            style: {
                                color: '#8d3052',
                                fontWeight: 'bold',
                                fontSize: '24px'
                            }
                        },
                        xAxis: {
                            type: 'datetime',
                            title: {
                                text: 'Dates for Query',
                                style: {
                                    color: '#3c1414',
                                    fontWeight: 'bold',
                                    fontSize: '16px'
                                }
                            },
                        },
                        yAxis: {
                            title: {
                                text: 'Count of questions asked',
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
                                threshold: null
                            }
                        },
                        series: [{
                            type: 'area',
                            data: data
                        }]
                    });
                }
            );
        };

        $scope.GetClickedLink = function (clickedlinkdata) {
            var linkdata = [];
            var finaldata = [];
            var arr = [];
            for (var item in clickedlinkdata) {
                linkdata.push(clickedlinkdata[item]);
            }
            finaldata.push(linkdata.slice(2, 3));
            console.log(finaldata);
            for (var i = 0; i < finaldata.length; i++) {
                for (var j = 0; j <= i; j++) {
                    console.log(finaldata[i][j]);
                    arr.push(finaldata[i][j]);
                }
            }
            console.log(arr);
            Highcharts.chart('goalconversion', {
                chart: {
                    type: 'column',
                    borderWidth: 1,
                    width: 1000
                },
                title: {
                    text: 'Goal Conversions',
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
                    text: 'Bot response analysis'
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
                        //{
                        //name: 'Chrome',
                        //y: 61.41,
                        //sliced: true,
                        //selected: true
                        //},
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