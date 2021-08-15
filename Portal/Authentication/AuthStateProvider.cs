using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using RMDesktopUi.Library.Api;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly IAPIHelper _aPIHelper;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient,
                                 ILocalStorageService localStorage,
                                 IConfiguration config,
                                 IAPIHelper aPIHelper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _config = config;
            _aPIHelper = aPIHelper;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authTokenStorageKey = _config["authTokenStorageKey"];
            var token = await _localStorage.GetItemAsync<string>(authTokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            bool isAuthenticated = await NotifyUserAuthentication(token);

            if (!isAuthenticated)
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));

        }

        public async Task<bool> NotifyUserAuthentication(string token)
        {
            bool isAuthenticadedOutput;
            Task<AuthenticationState> authState;
            try
            {
                await _aPIHelper.GetLoggedUserInfo(token);
                var authenticatedUser = new ClaimsPrincipal(
                   new ClaimsIdentity(
                       JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));

                authState = Task.FromResult(new AuthenticationState(authenticatedUser));
                NotifyAuthenticationStateChanged(authState);
                isAuthenticadedOutput = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await NotifyUserLogout();
                isAuthenticadedOutput = false;
            }

            return isAuthenticadedOutput;

        }

        public async Task NotifyUserLogout()
        {
            string authTokenStorageKey = _config["authTokenStorageKey"];
            await _localStorage.RemoveItemAsync(authTokenStorageKey);
            var authState = Task.FromResult(_anonymous);
            _aPIHelper.LogOffUser();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
