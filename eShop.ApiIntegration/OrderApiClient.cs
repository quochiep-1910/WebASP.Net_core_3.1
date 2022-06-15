using eShop.Utilities.Constants;
using eShop.ViewModels.Common;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.ViewModels.Sales.RevenueStatistics;
using eShop.ViewModels.Utilities.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;  //truy cập HttpContext thông qua  IHttpContextAccessor

        public OrderApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
               IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CreateOrder(OrderCreateRequest request)
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var json = JsonConvert.SerializeObject(request); //convert json to string
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/orders/", httpContent);
            var orderId = response.Content.ReadAsStringAsync();
            return Int32.Parse(orderId.Result);
        }

        public async Task<bool> CreateOrderDetail(List<OrderDetailViewModel> request)
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var json = JsonConvert.SerializeObject(request); //convert json to string
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var orderId = request.Select(x => x.OrderId);
            var response = await client.PostAsync($"/api/orders/postorderdetail?orderId={orderId}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateSendEmail(SendMailViewModel sendMailViewModel)
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(sendMailViewModel.ToEmail.ToString()), "ToEmail" },
                { new StringContent(sendMailViewModel.Subject.ToString()), "Subject" },
                { new StringContent(sendMailViewModel.Content.ToString()), "Content" },
                //{ new StringContent(sendMailViewModel.Attachments), "Attachments" },
            };
            if (sendMailViewModel.Attachments != null)
            {
                foreach (var file in sendMailViewModel.Attachments)
                {
                    byte[] data;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)file.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "Attachments", file.FileName);
                }
            }

            var response = await client.PostAsync($"/api/Orders/SendEmail", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            return await Delete($"/api/orders/" + id);
        }

        public async Task<OrderViewModel> GetById(int orderId)
        {
            var data = await GetAsync<OrderViewModel>($"/api/orders/{orderId}");
            return data;
        }

        public async Task<PagedResult<OrderViewModel>> GetPagings(GetOrderPagingRequest request)
        {
            var result = await GetAsync<PagedResult<OrderViewModel>>(
                "/api/Orders/paging?pageIndex="
                 + $"{request.PageIndex}&pageSize={request.PageSize}" +
                 $"&keyword={request.Keyword}");
            return result;
        }

        public async Task<int> GetTotalOrder()
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var response = await client.GetAsync($"/api/orders/totalOrder");
            var totalOrder = response.Content.ReadAsStringAsync();
            return Int32.Parse(totalOrder.Result);
        }

        public async Task<List<RevenueStatisticViewModel>> RevenueStatistic(StatisticsRequest request)
        {
            //add Data
            request.FromDate = "01-01-2022";
            request.ToDate = "01-01-2023";

            var result = await GetListAsync<RevenueStatisticViewModel>("/api/Orders/getrevenue?fromDate="
                + $"{request.FromDate}&toDate={request.ToDate}");

            return result;
        }

        public async Task<bool> UpdateOrder(OrderUpdateRequest orderUpdate)
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(orderUpdate.OrderDate.ToString()), "OrderDate" },
                { new StringContent(orderUpdate.ShipName.ToString()), "ShipName" },
                { new StringContent(orderUpdate.ShipAddress.ToString()), "ShipAddress" },
                { new StringContent(orderUpdate.ShipEmail.ToString()), "ShipEmail" },
                { new StringContent(orderUpdate.ShipPhoneNumber.ToString()), "ShipPhoneNumber" },
                { new StringContent(orderUpdate.Status.ToString()), "Status" }
            };

            var response = await client.PutAsync($"/api/Orders/" + orderUpdate.Id, requestContent);
            return response.IsSuccessStatusCode;
        }
    }
}