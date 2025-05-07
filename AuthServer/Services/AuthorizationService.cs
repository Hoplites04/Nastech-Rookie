using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;

namespace AuthServer.Services;

public class AuthorizationService
{
    public IDictionary<string, StringValues> ParseOAuthParameters(HttpContext httpContext, List<string>? excluding = null)
    {
        excluding ??= new List<string>();

        var parameters = httpContext.Request.HasFormContentType
            ? httpContext.Request.Form
                .Where(v => !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value)
            : httpContext.Request.Query
                .Where(v => !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value);

        return parameters;
    }

    public string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> oAuthParameters)
    {
        var url = request.PathBase + request.Path + QueryString.Create(oAuthParameters);
        return url;
    }

    public bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request)
    {
        if (!authenticateResult.Succeeded)
        {
            return false;
        }

        if (request.MaxAge.HasValue && authenticateResult.Properties != null)
        {
            var maxAgeSeconds = TimeSpan.FromSeconds(request.MaxAge.Value);

            var expired = !authenticateResult.Properties.IssuedUtc.HasValue ||
                          DateTimeOffset.UtcNow - authenticateResult.Properties.IssuedUtc > maxAgeSeconds;
            if (expired)
            {
                return false;
            }
        }

        return true;
    }

    public static List<string> GetDestinations(ClaimsIdentity identity, Claim claim)
    {
        var destinations = new List<string>();
        switch (claim.Type)
        {
            case OpenIddictConstants.Claims.Name:
            case OpenIddictConstants.Claims.Email:
            case ClaimTypes.Role: // ✅ Include Role
            case OpenIddictConstants.Claims.Role: // ✅ Also support OpenIddict role key
                destinations.Add(OpenIddictConstants.Destinations.AccessToken);

                if (identity.HasScope(OpenIddictConstants.Scopes.OpenId))
                {
                    destinations.Add(OpenIddictConstants.Destinations.IdentityToken);
                }
                break;

            // Add other sensitive claim filters here if needed (e.g., never include password hash)
        }

        return destinations;
    }
}