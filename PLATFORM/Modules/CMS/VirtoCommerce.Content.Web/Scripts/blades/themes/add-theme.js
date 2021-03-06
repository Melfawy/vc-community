﻿angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.addThemeController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.themes', 'FileUploader', 'platformWebApp.bladeNavigationService', function ($scope, dialogService, themes, FileUploader, bladeNavigationService) {
	var blade = $scope.blade;
	blade.themeLoaded = false;

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	blade.initialize = function () {
		blade.isLoading = false;
	}

	if (!$scope.uploader) {
		// Creates a uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/assets/',
			autoUpload: true,
			removeAfterUpload: true
		});

		// ADDING FILTERS
		// Zips only
		//uploader.filters.push({
		//	name: 'zipFilter',
		//	fn: function (i /*{File|FileLikeObject}*/, options) {
		//		var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
		//		return '|zip|'.indexOf(type) !== -1;
		//	}
		//});


		uploader.onSuccessItem = function (fileItem, files, status, headers) {
			blade.themeFileUrl = files[0].url;
			blade.themeLoaded = true;
		};
	}

	blade.save = function () {
		if (!$scope.formScope.$invalid) {
			themes.createTheme({ storeId: blade.choosenStoreId, themeName: blade.name, themeFileUrl: blade.themeFileUrl }, function (data) {
				bladeNavigationService.closeBlade(blade);
			});
		}
	}

	blade.initialize();
}]);