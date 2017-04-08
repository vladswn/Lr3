//Angular module creation using angular.module method.
//It acts as a auto bootstraper for angular application.
//We bind this module to view using ng-app directive.
var myapp;
(function () {
    myapp = angular.module('myangularapp', []);
})();