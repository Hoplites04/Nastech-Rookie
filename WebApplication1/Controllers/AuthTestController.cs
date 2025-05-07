using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace MvcClient.Controllers
{
    [Authorize]
    public class AuthTestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthTestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("/call-api")]
        public async Task<IActionResult> CallApi()
        {
            // var accessToken = await HttpContext.GetTokenAsync("access_token");

            var authResponse = new AuthResponse
            {
                Token = await HttpContext.GetTokenAsync(
                    OpenIdConnectDefaults.AuthenticationScheme,
                    "access_token"
                ),
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email),
                Role = User.FindFirstValue(ClaimTypes.Role),
            };

            string accessToken = authResponse.Token;
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);

            var aud = jwt.Audiences.FirstOrDefault();
            var exp = jwt.ValidTo;

            string userId = authResponse.UserId;
            string userName = authResponse.UserName;
            string email = authResponse.Email;
            string role = authResponse.Role;

            if (string.IsNullOrEmpty(accessToken))
                return Content("No access token found. Try re-logging in.");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync(
                "https://localhost:7251/api/AuthorizationTest/test"
            ); // ✅ Your Resource Server URL

            if (!response.IsSuccessStatusCode)
            {
                var status = (int)response.StatusCode;
                var error = await response.Content.ReadAsStringAsync();
                return Content(
                    @$"Error from Resource Server
                Status: {status}
                
                {error}
                
                Access Token: {accessToken}
                
                User ID: {userId}
                
                User Name: {userName}
                
                Email: {email}

                Audience: {aud}
                
                Role: {role}",
                    "text/plain"
                );
            }
            var content = await response.Content.ReadAsStringAsync();

            return Content($"Response from Resource Server:\n\n{content}", "text/plain");
        }
    }
}
