﻿"use strict";

var SOFT_VER = "0.01";

var SyraApp = angular.module("syra", ["ui.router"]);

SyraApp.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise("/Home");

    var home = {
        url: '/Home',
        title: 'Home',
        name: 'Home',
        templateUrl: "/Appscript/Home/Template/home.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(home);

    var register = {
        url: '/Account/Register',
        title: 'Register',
        name: 'Register',
        controller: 'RegistrationController',
        templateUrl: "/Appscript/Register/Template/register.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(register);

    var acc_confirmation = {
        url: '/Account/Confirmation',
        title: 'Account Confirmation',
        name: 'Confirmation',
        templateUrl: "/Appscript/AccountConfirmation/Template/accountconfirmation.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(acc_confirmation);

    var chatbot = {

        view: {
            url: '/chatbot',
            title: 'chatbot',
            name: 'chatbot',
            id: 'chatbot',
            controller: 'ChatBotViewController',
            templateUrl: "/Appscript/ChatBot/Template/index.html?VER=" + SOFT_VER,
        },
        add: {
            url: '/chatbot/new',
            title: 'New Chatbot',
            name: 'newchatbot',
            controller: 'ChatBotAddController',
            templateUrl: "/Appscript/ChatBot/Template/add.html?VER=" + SOFT_VER,
        },
        edit: {
            url: '/chatbot/:id',
            title: 'Edit Chatbot',
            name: 'editchatbot',
            controller: 'ChatBotAddController',
            templateUrl: "/Appscript/ChatBot/Template/add.html?VER=" + SOFT_VER,
        }
    };
    $stateProvider.state(chatbot.view);
    $stateProvider.state(chatbot.add);
    $stateProvider.state(chatbot.edit);

    var profile = {
        url: '/Profile',
        title: 'Profile',
        name: 'profile',
        controller: 'CustomerProfileController',
        templateUrl: "/Appscript/CustomerProfile/Template/profile.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(profile);

    var subscription = {
        url: '/Subscription',
        title: 'subscription',
        name: 'subscription',
        controller: 'CustomerProfileController',
        templateUrl: "/Appscript/CustomerProfile/Template/subscription.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(subscription);

    var customerplan = {
        url: '/managecustomer',
        title: 'managecustomer',
        name: 'managecustomer',
        controller: 'CustomerPlanController',
        templateUrl: "/Appscript/CustomerPlans/Template/index.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(customerplan);

    var viwdetails = {
        url: '/customerdetails/:Id',
        title: 'CustomerDetails',
        name: 'customerdetails',
        controller: 'CustomerDetailController',
        templateUrl: "/Appscript/CustomerPlans/Template/viewcustomerdetails.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(viwdetails);
}])