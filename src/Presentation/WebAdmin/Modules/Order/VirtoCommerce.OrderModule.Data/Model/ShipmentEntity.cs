﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class ShipmentEntity : OperationEntity
	{
		public string CustomerOrderId { get; set; }
		public virtual CustomerOrderEntity CustomerOrder { get; set; }

		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<PaymentInEntity> InPayments { get; set; }
		public virtual AddressEntity DeliveryAddress { get; set; }
		public virtual DiscountEntity Discount { get; set; }
	}
}
