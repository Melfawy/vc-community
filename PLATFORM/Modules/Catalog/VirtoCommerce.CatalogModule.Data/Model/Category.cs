﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class Category : CategoryBase
    {
		public Category()
		{
			CategoryPropertyValues = new NullCollection<CategoryPropertyValue>();
			OutgoingLinks = new NullCollection<CategoryRelation>();
			IncommingLinks = new NullCollection<CategoryRelation>();
		}
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

        #region Navigation Properties

		[StringLength(128)]
        [ForeignKey("PropertySet")]
		public string PropertySetId { get; set; }

        public virtual PropertySet PropertySet { get; set; }
		public virtual ObservableCollection<CategoryPropertyValue> CategoryPropertyValues { get; set; }
		//It new navigation property for link replace to stupid CategoryLink (will be removed later)
		public virtual ObservableCollection<CategoryRelation> OutgoingLinks { get; set; }
		public virtual ObservableCollection<CategoryRelation> IncommingLinks { get; set; }
	    #endregion
    }
}
