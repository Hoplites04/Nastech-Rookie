using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using System.Web;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using AuthServer;
using AuthServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static OpenIddict.Abstractions.OpenIddictConstants;



namespace AuthServer.Controllers
{
    [ApiController]
    public class AuthorizationController : Controller 
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly AuthorizationService _authService;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly UserManager<IdentityUser> _userManager;


        public AuthorizationController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            AuthorizationService authService,
            UserManager<IdentityUser> userManager,
            ILogger<AuthorizationController> logger)
        {
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _authService = authService;
            _userManager = userManager;
            _logger = logger;
        }

            [HttpGet("~/connect/authorize")]
            [HttpPost("~/connect/authorize")]
            public async Task<IActionResult> Authorize() 
            {
                var request = HttpContext.GetOpenIddictServerRequest() ??
                            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

                // ! Logger for debugging
                // _logger.LogInformation("Request Info: {@Request}", request);

                var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

                // if (await _applicationManager.GetConsentTypeAsync(application) != ConsentTypes.Explicit)
                // {
                //     return Forbid(
                //         authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                //         properties: new AuthenticationProperties(new Dictionary<string, string?>
                //         {
                //             [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidClient,
                //             [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                //                 "Only clients with explicit consent type are allowed."
                //         }));
                // }
            

            var parameters = _authService.ParseOAuthParameters(HttpContext, new List<string> { Parameters.Prompt });

            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (!_authService.IsAuthenticated(result, request))
                {
                    return Challenge(properties: new AuthenticationProperties
                    {
                        RedirectUri = _authService.BuildRedirectUrl(HttpContext.Request, parameters)
                    }, new[] { IdentityConstants.ApplicationScheme});
                }
            
            if (request.HasPromptValue(PromptValues.Login))
                {
                    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                    return Challenge(properties: new AuthenticationProperties
                    {
                        RedirectUri = _authService.BuildRedirectUrl(HttpContext.Request, parameters)
                    }, new[] { IdentityConstants.ApplicationScheme });
                }

            var consentClaim = result.Principal.GetClaim(Consts.ConsentNaming);

            var consentType = await _applicationManager.GetConsentTypeAsync(application);

    if ((consentType == ConsentTypes.Explicit || consentType == ConsentTypes.Systematic)
        && (consentClaim != Consts.GrantAccessValue || request.HasPromptValue(PromptValues.Consent)))
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        var returnUrl = HttpUtility.UrlEncode(_authService.BuildRedirectUrl(HttpContext.Request, parameters));
        return Redirect($"/Consent?returnUrl={returnUrl}");
    }
                var userId = result.Principal.FindFirst(ClaimTypes.Email)!.Value;
                var user = await _userManager.FindByEmailAsync(userId);
                var userName = user?.UserName ?? string.Empty;
                var roles = await _userManager.GetRolesAsync(user);
  

                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);
                

                identity.SetClaim(Claims.Subject, userId)
                    .SetClaim(Claims.Email, userId)
                    .SetClaim(Claims.Name, userName)
                    .SetClaims(Claims.Role, roles.ToImmutableArray());
                    
                identity.SetScopes(request.GetScopes());
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

                return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
                throw new InvalidOperationException("The specified grant type is not supported.");

            var result =
                await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var userId = result.Principal.GetClaim(Claims.Subject);

            if (string.IsNullOrEmpty(userId))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "Cannot find user from the token."
                    }));
            }
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "User ID is missing or invalid."
                    }));
            }

            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "User not found."
                    }));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userName = user.UserName ?? string.Empty;

            var identity = new ClaimsIdentity(result.Principal.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity.SetClaim(Claims.Subject, userId)
                .SetClaim(Claims.Email, userId)
                .SetClaim(Claims.Name, userName)
                .SetClaims(Claims.Role, roles.ToImmutableArray());

            identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        // [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo")]
        // public async Task<IActionResult> Userinfo()
        // {
        //     var email = User.GetClaim(Claims.Email);
        //     if (string.IsNullOrEmpty(email))
        //     {
        //         return Challenge(
        //             authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
        //             properties: new AuthenticationProperties(new Dictionary<string, string?>
        //             {
        //                 [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
        //                 [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
        //                     "The specified access token is bound to an account that no longer exists."
        //             }));
        //     }

        //     var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        //     {
        //         // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
        //         [Claims.Subject] = Consts.Email
        //     };

        //     if (User.HasScope(Scopes.Email))
        //     {
        //         claims[Claims.Email] = Consts.Email;
        //     }

        //     return Ok(claims);
        // }

        [HttpGet("~/connect/logout")]
        [HttpPost("~/connect/logout")]
        public async Task<IActionResult> LogoutPost()
        {
            try
            {
                if (HttpContext == null)
                {
                    throw new InvalidOperationException("HttpContext is null.");
                }

                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                return SignOut(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = "/"
                    });
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
        }
    }
}