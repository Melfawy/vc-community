﻿using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Test
{
    [TestClass]
    public class CatalogControllerTest
    {
        [TestMethod]
        public void WorkingWithCatalogPropertyTest()
        {
            var catalogController = new CatalogModuleCatalogsController(GetCatalogService(), GetSearchService(), null, GetPropertyService(), GetPermissionService());
            var categoryController = new CatalogModuleCategoriesController(GetSearchService(), GetCategoryService(), GetPropertyService(), GetCatalogService());
            var propertyController = new CatalogModulePropertiesController(GetPropertyService(), GetCategoryService(), GetCatalogService());
            var productController = new CatalogModuleProductsController(GetItemService(), GetPropertyService(), null, null, null);
            var listEntryController = new CatalogModuleListEntryController(GetSearchService(), GetCategoryService(), GetItemService(), null);

            //var propertyResult = propertyController.GetNewCatalogProperty("Apple") as OkNegotiatedContentResult<webModel.Property>;
            //var property = propertyResult.Content;

            //property.Name = "CLP_Test2";
            //property.Type = webModel.PropertyType.Product;

            //propertyController.Post(property);

            var catalogResult = catalogController.Get("Apple") as OkNegotiatedContentResult<webModel.Catalog>;
            var catalog = catalogResult.Content;
			catalog.Properties[0].Values.Add(new webModel.PropertyValue { Value = "sssss", ValueType = PropertyValueType.ShortText });
            catalogController.Update(catalog);

            var serachResult = listEntryController.ListItemsSearch(new webModel.ListEntrySearchCriteria
            {
                CatalogId = "Apple",
                CategoryId = "186d61d8-d843-4675-9f77-ec5ef603fda3",
                ResponseGroup = webModel.ResponseGroup.Full
            })
                as OkNegotiatedContentResult<webModel.ListEntrySearchResult>;
            var listResult = serachResult.Content;

            var listEntryProduct = listResult.ListEntries.OfType<webModel.ListEntryProduct>().FirstOrDefault();
            var product = (productController.Get(listEntryProduct.Id) as OkNegotiatedContentResult<webModel.Product>).Content;


        }

        [TestMethod]
        public void VirtualCatalogWorkingTest()
        {

            var catalogController = new CatalogModuleCatalogsController(GetCatalogService(), GetSearchService(), null, GetPropertyService(), GetPermissionService());
            var categoryController = new CatalogModuleCategoriesController(GetSearchService(), GetCategoryService(), GetPropertyService(), GetCatalogService());
            var listEntryController = new CatalogModuleListEntryController(GetSearchService(), GetCategoryService(), GetItemService(), null);

            //Create virtual catalog
            var catalogResult = catalogController.GetNewVirtualCatalog() as OkNegotiatedContentResult<webModel.Catalog>;
            var vCatalog = catalogResult.Content;
            vCatalog.Name = "vCatalog1";
            catalogController.Update(vCatalog);

            Assert.IsTrue(vCatalog.Virtual);

            //Create virtual category
            var categoryResult = categoryController.GetNewCategory(vCatalog.Id) as OkNegotiatedContentResult<webModel.Category>;
            var vCategory = categoryResult.Content;
            vCatalog.Name = "vCategory";
            categoryController.Post(vCategory);

            Assert.IsTrue(vCategory.Virtual);

            //Link category to virtual category
            var link = new webModel.ListEntryLink { ListEntryId = "40773cd0-f2de-462f-9041-da742a274c38", ListEntryType = "Category", CatalogId = vCatalog.Id, CategoryId = vCategory.Id };
            listEntryController.CreateLinks(new webModel.ListEntryLink[] { link });


            //Check result
            var serachResult = listEntryController.ListItemsSearch(new webModel.ListEntrySearchCriteria
            {
                CatalogId = vCatalog.Id,
                CategoryId = vCategory.Id,
                ResponseGroup = webModel.ResponseGroup.WithCategories
            })
                as OkNegotiatedContentResult<webModel.ListEntrySearchResult>;
            var listResult = serachResult.Content;

            Assert.IsTrue(listResult.ListEntries.Any());
            var category = listResult.ListEntries.First();
            Assert.IsTrue(category.Id == "40773cd0-f2de-462f-9041-da742a274c38");

            //Remove link
            //listEntryController.DeleteLinks(new webModel.ListEntryLink[] { link });

        }

        [TestMethod]
        public void AssociationTest()
        {
            //Get all product associations
            var productController = new CatalogModuleProductsController(GetItemService(), GetPropertyService(), null, null, null);
            var productResult = productController.Get("v-b004y45rxi") as OkNegotiatedContentResult<webModel.Product>;
            var product = productResult.Content;
            Assert.IsFalse(product.Associations.Any());


            //Add association
            var association = new webModel.ProductAssociation
            {
                ProductId = "v-b0007zl6ds",
                Name = "Related Items"
            };
            product.Associations.Add(association);
            productController.Update(product);


            //Remove
            product.Associations.Remove(product.Associations.Last());
            productController.Update(product);

        }

        private ICatalogSearchService GetSearchService()
        {
            return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService(), null);
        }

        private IPermissionService GetPermissionService()
        {
            return new TestPermissionService();
        }

        private IPropertyService GetPropertyService()
        {
            return new PropertyServiceImpl(() => { return GetRepository(); });
        }

        private ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(() => { return GetRepository(); }, null);
        }

        private ICatalogService GetCatalogService()
        {
            return new CatalogServiceImpl(() => { return GetRepository(); });
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(() => { return GetRepository(); }, null);
        }


        private ICatalogRepository GetRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce");
            return retVal;
        }



		internal class TestPermissionService : IPermissionService
		{
			#region Implementation of IPermissionService

			public bool UserHasAnyPermission(string userName, params string[] permissionIds)
			{
				return true;
			}

			public Permission[] GetAllPermissions()
			{
				throw new System.NotImplementedException();
			}

			public string[] GetUserPermissionIds(string userName)
			{
				throw new System.NotImplementedException();
			}

			#endregion
		}
    }
}
