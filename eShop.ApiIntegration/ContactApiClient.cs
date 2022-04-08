using eShop.Utilities.Constants;
using eShop.ViewModels.Common;
using eShop.ViewModels.Contact;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.ApiIntegration
{
    public class ContactApiClient : BaseApiClient, IContactApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactApiClient(IHttpClientFactory httpClientFactory
            , IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Create(ContactCreateViewModel request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.Status.ToString()), "status");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Email) ? "" : request.Email.ToString()), "Email");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.PhoneNumber) ? "" : request.PhoneNumber.ToString()), "PhoneNumber");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Message) ? "" : request.Message.ToString()), "Message");

            var response = await client.PostAsync($"/api/Contacts/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteContact(int contactId)
        {
            return await Delete($"/api/categories/" + contactId);
        }

        public async Task<List<ContactViewModel>> GetAll()
        {
            return await GetListAsync<ContactViewModel>("/api/Contacts");
        }

        public async Task<PagedResult<ContactViewModel>> GetAllPaging(ContactPagingRequest request)
        {
            var result = await GetAsync<PagedResult<ContactViewModel>>(
                "/api/Contacts/paging?pageIndex="
                 + $"{request.PageIndex}&pageSize={request.PageSize}" +
                 $"&keyword={request.Keyword}");
            return result;
        }

        public async Task<ContactViewModel> GetById(int contactId)
        {
            var data = await GetAsync<ContactViewModel>($"/api/Contacts/{contactId}");
            return data;
        }

        public async Task<bool> Update(ContactViewModel request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.Status.ToString()), "status");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Email) ? "" : request.Email.ToString()), "Email");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.PhoneNumber) ? "" : request.PhoneNumber.ToString()), "Phone number");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Message) ? "" : request.Message.ToString()), "Message");
            var response = await client.PutAsync($"/api/Contacts/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetTotalContact()
        {
            //1.Khởi tạo
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //lấy token

            var response = await client.GetAsync($"/api/Contacts/totalContact");
            var totalOrder = response.Content.ReadAsStringAsync();
            return Int32.Parse(totalOrder.Result);
        }
    }
}