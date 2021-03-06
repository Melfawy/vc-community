﻿using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class MenuLinkConverter
	{
		public static coreModels.MenuLink ToCoreModel(this webModels.MenuLink link)
		{
			var retVal = new coreModels.MenuLink { Id = link.Id, Title = link.Title, Url = link.Url, Type = link.Type, Priority = link.Priority, IsActive = link.IsActive, MenuLinkListId = link.MenuLinkListId };

		    return retVal;
		}

		public static webModels.MenuLink ToWebModel(this coreModels.MenuLink link)
		{
			var retVal = new webModels.MenuLink { Id = link.Id, Title = link.Title, Url = link.Url, Type = link.Type, Priority = link.Priority, IsActive = link.IsActive, MenuLinkListId = link.MenuLinkListId };

		    return retVal;
		}
	}
}