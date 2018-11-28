SyraApp.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise("/Home");

    var home = {
        url: '/Home',
        title: 'Home',
        name: 'Home',
        templateUrl: "/Appscript/Home/Template/home.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(home);
    var database = {
        view: {
            url: '/database',
            title: 'DB Details',
            name: 'dbdetail',
            id: 'entry',
            controller: 'DatabaseViewController',
            templateUrl: "/Appscript/ManageDb/Template/view.html?VER=" + SOFT_VER,
        },
        add: {
            url: '/database/new',
            title: 'New DataEntry',
            name: 'newdataentry',
            controller: 'DatabaseAddController',
            templateUrl: "/AppScript/ManageDb/Template/index.html?VER=" + SOFT_VER,
        },
        edit: {
            url: '/database/:id',
            title: 'Edit Database',
            name: 'entryedit',
            controller: 'DatabaseAddController',
            templateUrl: "/AppScript/ManageDb/Template/index.html?VER=" + SOFT_VER,
        }
    };
    $stateProvider.state(database.view);
    $stateProvider.state(database.add);
    $stateProvider.state(database.edit);
    var resetpassword = {
        url: '/Account/ForgotPassWord',
        title: 'Forgot Password',
        name: 'forgotpassword',
        controller: 'ChangepasswordController',
        templateUrl: "/Appscript/ChangePassword/Template/forgotpassword.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(resetpassword);

    var analytics = {
        url: '/analytics',
        title: 'Preview Analytics',
        name: 'analytics',
        controller: 'AnalyticsController',
        templateUrl: "/Appscript/Analytics/Template/index.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(analytics);

    var register = {
        url: '/register',
        title: 'Register',
        name: 'register',
        controller: 'RegisterController',
        templateUrl: "/Appscript/Register/Template/registration.html?VER=" + SOFT_VER,
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
        },
        add: {
            url: '/adminplan/new',
            title: 'New Plan',
            name: 'newplancreate',
            controller: 'PlanAddController',
            templateUrl: "/Appscript/AdminPlan/Template/add.html?VER=" + SOFT_VER,
        },
        edit: {
            url: '/adminplan/:id',
            title: 'New Plan',
            name: 'planedit',
            controller: 'PlanAddController',
            templateUrl: "/Appscript/AdminPlan/Template/add.html?VER=" + SOFT_VER,
        }
    };
    $stateProvider.state(adminplan.view);
    $stateProvider.state(adminplan.add);
    $stateProvider.state(adminplan.edit);

    var luisdomain = {
        view: {
            url: '/luisdomain',
            title: 'Luis Domain',
            name: 'luisdomain',
            id: 'luis',
            controller: 'DomainViewController',
            templateUrl: "/Appscript/LuisDomain/Template/index.html?VER=" + SOFT_VER,
        },
        add: {
            url: '/luisdomain/new',
            title: 'New LuisDomain',
            name: 'newluisdomain',
            controller: 'DomainAddController',
            templateUrl: "/AppScript/LuisDomain/Template/add.html?VER=" + SOFT_VER,
        },
        edit: {
            url: '/luisdomain/:id',
            title: 'New LuisDomain',
            name: 'luisdomainedit',
            controller: 'DomainAddController',
            templateUrl: "/AppScript/LuisDomain/Template/add.html?VER=" + SOFT_VER,
        }
    };
    $stateProvider.state(luisdomain.view);
    $stateProvider.state(luisdomain.add);
    $stateProvider.state(luisdomain.edit);

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

    var logs = {
        url: '/logs',
        title: 'Logs',
        name: 'logs',
        controller: 'LogsController',
        templateUrl: "/Appscript/Logs/Template/index.html?VER=" + SOFT_VER,
    };
    $stateProvider.state(logs);
}]);