var app = angular.module('app', ['ngRoute', 'ngAnimate',
    'ngSanitize', 'LocalStorageModule', 'ui.bootstrap']);

app.config([
    '$routeProvider',
    '$locationProvider',
    'localStorageServiceProvider',
    function($routeProvider, $locationProvider, localStorageServiceProvider){
        $routeProvider
            .when('/', {
                templateUrl: 'partials/home.html'
            });

        localStorageServiceProvider
            .setPrefix('app')
            .setStorageType('localStorage')
            .setNotify(false, false);
    }
]);