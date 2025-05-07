using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuthServer;
using AuthServer.Services;
using AuthServer.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        // Register OpenIddict with EF Core stores
        options.UseOpenIddict();
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Authentication";
//         options.AccessDeniedPath = "/Authentication/Denied";
//     });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication"; // Redirect unauthenticated users to this path
    options.AccessDeniedPath = "/Authentication/Denied"; // Redirect users without access
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set cookie expiration
    options.SlidingExpiration = true; // Extend expiration on activity
    options.Cookie.HttpOnly = true; // Prevent client-side access to the cookie
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use cookies only over HTTPS
});


builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
                .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("connect/authorize")
                .SetEndSessionEndpointUris("connect/logout")
                .SetTokenEndpointUris("connect/token")
                .SetUserInfoEndpointUris("connect/userinfo");

        options.RegisterPromptValues("login");

        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "api", "resource_server_1", "resource_server_2");

        options.AllowAuthorizationCodeFlow();

        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
        
        options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();

        // by default tokens are decrypted. If you would like to take a look in the claims - you can disable it
        //options.DisableAccessTokenEncryption();
        
        options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableEndSessionEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableUserInfoEndpointPassthrough();
    });

builder.Services.AddTransient<AuthorizationService>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddTransient<ClientsSeeder>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => 
    {
        policy.WithOrigins("https://localhost:7002",
                            "https://localhost:7274")
            .AllowAnyHeader();

        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader();
    });
});



var app = builder.Build();

await SeedClientsAsync(app); // ✅ Use proper async

async Task SeedClientsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ClientsSeeder>();

    await seeder.AddMVCClient();
    await seeder.AddReactClient();
    await seeder.AddWebClient();
    await seeder.AddScopeAsync("resource_server_1");
    await seeder.SeedUsersAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();


