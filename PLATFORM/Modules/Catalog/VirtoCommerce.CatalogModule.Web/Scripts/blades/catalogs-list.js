﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogsListController', ['$injector', '$rootScope', '$scope', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.authService',
function ($injector, $rootScope, $scope, catalogs, bladeNavigationService, dialogService, authService) {
	var selectedNode = null;
	var preventCategoryListingOnce;

	$scope.blade.refresh = function () {
		$scope.blade.isLoading = true;

		catalogs.getCatalogs({}, function (results) {
			$scope.blade.isLoading = false;

			$scope.objects = results;

			if (selectedNode != null) {
				//select the node in the new list
				angular.forEach(results, function (node) {
					if (selectedNode.id === node.id) {
						selectedNode = node;
					}
				});
			}
			setBreadcrumps();
		});

	};

	$scope.blade.onClose = function (closeCallback) {
		if ($scope.blade.childrenBlades.length > 0) {
			var callback = function () {
				if ($scope.blade.childrenBlades.length == 0) {
					closeCallback();
				};
			};
			angular.forEach($scope.blade.childrenBlades, function (child) {
				bladeNavigationService.closeBlade(child, callback);
			});
		}
		else {
			closeCallback();
		}
	};

	//Breadcrumps
	function setBreadcrumps() {
		//Clone array (angular.copy leave a same reference)
		$scope.blade.breadcrumbs = $scope.blade.breadcrumbs.slice(0);

		var breadCrumb = {
			id: "Catalogs",
			name: "Catalogs",
		};

		//prevent dublicate items
		if (!_.some($scope.blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
			$scope.blade.breadcrumbs.push(breadCrumb);
		}

		breadCrumb.navigate = function (breadcrumb) {
			bladeNavigationService.closeBlade($scope.blade,
			function () {
				bladeNavigationService.showBlade($scope.blade);
				$scope.blade.refresh();
			});
		};

	};

	$scope.refreshItems = function () {
		if (preventCategoryListingOnce) {
			preventCategoryListingOnce = undefined;
		} else {
			var newBlade = {
				id: 'itemsList1',
				level: 1,
				breadcrumbs: $scope.blade.breadcrumbs,
				title: 'Categories & Items',
				subtitle: 'Browsing ' + (selectedNode != null ? '"' + selectedNode.name + '"' : ''),
				catalogId: (selectedNode != null) ? selectedNode.id : null,
				catalog: selectedNode,
				controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
				template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
			};

			bladeNavigationService.showBlade(newBlade, $scope.blade);
		}
	};

	$scope.editCatalog = function (catalog) {
		if (catalog.virtual) {
			showVirtualCatalogBlade(catalog.id, null, catalog.name);
		}
		else {
			showCatalogBlade(catalog.id, null, catalog.name);
		}
		preventCategoryListingOnce = true;
	};

	$scope.deleteCatalog = function (node) {
		var dialog = {
			id: "confirmDelete",
			title: "Delete confirmation",
			message: "Are you sure you want to delete catalog '" + node.name + "'?",
			callback: function (remove) {
				if (remove) {
					catalogs.delete({ id: node.id }, function (data, headers) {
						$scope.blade.refresh();
						$scope.refreshItems();
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);

		preventCategoryListingOnce = true;
	};


	function showCatalogBlade(id, data, title) {
		var newBlade = {
			currentEntityId: id,
			currentEntity: data,
			title: title,
			id: 'catalogEdit',
			subtitle: 'edit catalog',
			controller: 'virtoCommerce.catalogModule.catalogDetailController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

	function showVirtualCatalogBlade(id, data, title) {
		var newBlade = {
			currentEntityId: id,
			currentEntity: data,
			title: title,
			subtitle: 'Virtual catalog details',
			id: 'catalogEdit',
			controller: 'virtoCommerce.catalogModule.virtualCatalogDetailController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

	$scope.selectNode = function (node) {
		selectedNode = node;
		$scope.selectedNodeId = selectedNode.id;

		$scope.refreshItems();
	};



	$scope.blade.toolbarCommands = [
        {
        	name: "Manage", icon: 'fa fa-edit',
        	executeMethod: function () {
        		$scope.editCatalog(selectedNode);
        	},
        	canExecuteMethod: function () {
        		return selectedNode;
        	},
        	permission: 'catalog:catalogs:manage'
        },

      {
      	name: "Delete", icon: 'fa fa-trash-o',
      	executeMethod: function () {
      		$scope.deleteCatalog(selectedNode);
      	},
      	canExecuteMethod: function () {
      		return selectedNode;
      	},
      	permission: 'catalog:catalogs:manage'
      }
	];

	if (authService.checkPermission('catalog:catalogs:manage') || authService.checkPermission('catalog:virtual_catalogs:manage')) {
		$scope.blade.toolbarCommands.splice(0, 0, {
			name: "Add",
			icon: 'fa fa-plus',
			executeMethod: function () {
				var newBlade = {
					id: 'listItemChild',
					title: 'New catalog',
					subtitle: 'Choose new catalog type',
					controller: 'virtoCommerce.catalogModule.catalogAddController',
					template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-add.tpl.html'
				};

				bladeNavigationService.showBlade(newBlade, $scope.blade);
			},
			canExecuteMethod: function () {
				return true;
			}
		});
	}

	// actions on load
	$scope.blade.refresh();
}]);