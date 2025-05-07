using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient(
    "ResourceServer",
    client =>
    {
        client.BaseAddress = new Uri("https://localhost:7251/"); // Your Resource API base URL
    }
);

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(
        OpenIdConnectDefaults.AuthenticationScheme,
        options =>
        {
            options.Authority = "https://localhost:7000"; // AuthServer base URL
            options.ClientId = "mvc-client";
            options.ClientSecret = "xbxEvLyJN0lS28IW6FnC77Y4YSAlBGFS"; // if client is confidential
            options.ResponseType = "code";

            options.SaveTokens = true;

            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Scope.Add("roles");
            options.Scope.Add("api"); // Your API scope
            options.Scope.Add("resource_server_1"); // Your API scope

            // options.GetClaimsFromUserInfoEndpoint = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role",
            };

            options.CallbackPath = "/signin-oidc"; // Must match RedirectUri in AuthServer
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapDefaultControllerRoute();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapGet(
    "/login",
    async context =>
    {
        await context.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = "/" }
        );
    }
);

app.MapGet(
    "/logout",
    async context =>
    {
        // Sign out of local cookie
        await context.SignOutAsync("Cookies");

        // Optional: get id_token for logout hint
        var idToken = await context.GetTokenAsync("id_token");

        var postLogoutRedirectUri = "https://localhost:7274/"; // ⬅️ your MVC home URL

        var logoutUrl =
            $"https://localhost:7000/connect/logout"
            + $"?post_logout_redirect_uri={Uri.EscapeDataString(postLogoutRedirectUri)}";

        if (!string.IsNullOrEmpty(idToken))
        {
            logoutUrl += $"&id_token_hint={Uri.EscapeDataString(idToken)}";
        }

        context.Response.Redirect(logoutUrl);
    }
);
app.Run();
