using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService ordersService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService ordersService, IProductService productService, ICustomerService customerService)
        {
            this.ordersService = ordersService;
            this.productService = productService;
            this.customerService = customerService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var customerResult = await customerService.GetCustomerAsync(customerId);
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productService.GetProductsAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product information is not available.";
                    }
                }
                var result = new
                {
                    Customer = customerResult.IsSuccess ?
                        customerResult.Customer :
                        new
                        {
                            Name = "Customer information is not available."
                        },
                    Orders = ordersResult.Orders
                };

                return (true, result);
            }
            return (false, null);
        }
    }
}
