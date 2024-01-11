using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Dtos;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, ILogger logger, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshToken()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> SignIn(SigninInput signinInput)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }); //Discovery endpoint e istek atar. https false (IdentityModel üzerinden atıyoruz)
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            var passwordTokenReq = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = disco.TokenEndpoint
            };
            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenReq);
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errorDto.Errors, 400);
            }
            var userInfoReg = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };
            var userInfo = await _httpClient.GetUserInfoAsync(userInfoReg);
            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>() { 
                new AuthenticationToken 
                {
                    Name = OpenIdConnectParameterNames.AccessToken, Value= token.AccessToken
                },
                  new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken, Value= token.RefreshToken
                },
                    new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn, Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                }
            });
            authenticationProperties.IsPersistent = signinInput.IsRemember;
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
