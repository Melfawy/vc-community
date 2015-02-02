﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class AddressEntity : Entity
	{
		public string AddressType { get; set; }
		public string Organization { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Zip { get; set; }

		public ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }
		public ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }
		public PaymentEntity Payment { get; set; }
		public string PaymentId { get; set; }
	}
}
