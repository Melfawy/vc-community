﻿angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeCurrenciesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            itemId: blade.itemId,
            title: blade.title,
            subtitle: 'Manage currencies',
            controller: 'virtoCommerce.storeModule.storeCurrenciesListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-currencies-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);