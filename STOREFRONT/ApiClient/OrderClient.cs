﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.ApiClient.DataContracts.Orders;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class OrderClient : BaseClient
    {
        #region Constructors and Destructors

        public OrderClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public OrderClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Task<CustomerOrder> GetCustomerOrderAsync(string customerId, string orderId)
        {
            var parameters = new
            {
                customer = customerId
            };
            return GetAsync<CustomerOrder>(
                CreateRequestUri(string.Format(RelativePaths.GetSingleOrder, orderId), parameters),
                useCache: false);
        }

        public Task<OrderSearchResult> GetCustomerOrdersAsync(
            string storeId,
            string customerId,
            string query,
            int skip,
            int take)
        {
            var parameters = new
            {
                q = query,
                site = storeId,
                customer = customerId,
                start = skip.ToString(),
                count = take.ToString(),
                respGroup = "Full"
            };

            return GetAsync<OrderSearchResult>(
                CreateRequestUri(RelativePaths.GetMultipleOrders, parameters),
                useCache: false);
        }

        public Task<CustomerOrder> CreateOrderAsync(string cartId)
        {
            return SendAsync<CustomerOrder>(
                CreateRequestUri(string.Format(RelativePaths.PostOrder, cartId)),
                HttpMethod.Post);
        }

        public Task UpdateOrderAsync(CustomerOrder order)
        {
            return SendAsync<CustomerOrder>(
                CreateRequestUri(RelativePaths.UpdateOrder), HttpMethod.Put, order);
        }

        public Task<ProcessPaymentResult> ProcessPayment(string orderId, string paymentMethodId)
        {
            return GetAsync<ProcessPaymentResult>(
                CreateRequestUri(string.Format(RelativePaths.ProcessPayment, orderId, paymentMethodId)),
                useCache: false);
        }

        public Task<PostProcessPaymentResult> PostPaymentProcess(ICollection<KeyValuePair<string, string>> parameters)
        {
            return GetAsync<PostProcessPaymentResult>(
                CreateRequestUri(RelativePaths.PostPaymentProcess, parameters.ToArray()), useCache: false);
        }
        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string GetMultipleOrders = "order/customerOrders";
            public const string GetSingleOrder = "order/customerOrders/{0}";
            public const string PostOrder = "order/customerOrders/{0}";
            public const string UpdateOrder = "order/customerOrders";
            public const string ProcessPayment = "order/customerOrders/{0}/processPayment/{1}";
            public const string PostPaymentProcess = "paymentcallback";

            #endregion
        }
    }
}
