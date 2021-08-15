using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using RMDesktopUi.Library.Models;
using RMDesktopUI.Library.Models;
using Microsoft.Extensions.Configuration;

namespace RMDesktopUi.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient;
        private readonly ILoggedInUserModel _loggedInUser;
        readonly IConfiguration _configuration;

        public APIHelper(ILoggedInUserModel loggedInUser, IConfiguration configuration)
        {
            _loggedInUser = loggedInUser;
            _configuration = configuration;
            InitializeClient();
        }
        private void InitializeClient()
        {
            string api = _configuration.GetValue<string>("api");
            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(api)
            };
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        public async Task<AuthenticatedUser> Authenticate(string userName, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
                });
            using HttpResponseMessage response = await _apiClient.PostAsync("/Token", data);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        public void LogOffUser()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task GetLoggedUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using HttpResponseMessage response = await _apiClient.GetAsync("/api/User");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                _loggedInUser.CreateDate = result.CreateDate;
                _loggedInUser.EmailAdress = result.EmailAdress;
                _loggedInUser.FirstName = result.FirstName;
                _loggedInUser.LastName = result.LastName;
                _loggedInUser.Id = result.Id;
                _loggedInUser.Token = token;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

        }
    }
}
