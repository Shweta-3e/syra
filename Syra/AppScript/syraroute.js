"use strict";

var SOFT_VER = "0.0002";

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

    var registeradmin = {
        url: '/Account/RegisterAdmin',
        title: 'Register Admin',
        name: 'Register Admin',
        controller: 'RegistrationController',
        templateUrl: "/Appscript/Register/Template/registeradmin.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(registeradmin);

    var adminplan = {
        view: {
            url: '/adminplan',
            title: 'adminplan',
            name: 'newplan',
            id: 'plan',
            controller: 'PlanViewController',
            templateUrl: "/Appscript/AdminPlan/Template/index.html?VER=" + SOFT_VER,
        }
        
    };
    $stateProvider.state(adminplan.view);

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
}])

