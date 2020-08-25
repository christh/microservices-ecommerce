using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using ECommerce.Api.Customers.Db;

namespace ECommerce.Api.Search.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger logger;

        public CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomerService");
                var response = await client.GetAsync($"api/customers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<dynamic>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
