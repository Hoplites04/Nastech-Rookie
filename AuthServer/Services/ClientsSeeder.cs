using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Client.WebIntegration.OpenIddictClientWebIntegrationConstants;
using Microsoft.AspNetCore.Identity;
using AuthServer.Data;

namespace AuthServer.Services
{
    public class ClientsSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientsSeeder(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public async Task AddWebClient()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // TODO: Seed Web client

            var client = await manager.FindByClientIdAsync("web-client");

            if (client != null)
            {
                await manager.DeleteAsync(client);
            }

            var descriptor = new OpenIddictApplicationDescriptor
            {
            ClientId = "web-client",
            ClientSecret = "web-secret",
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "Swagger Client Application",
            RedirectUris =
                {
                    new Uri("https://localhost:7002/swagger/oauth2-redirect.html")
                },
            PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7002/swagger/")
                },
            Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "api",
                    Permissions.Prefixes.Scope + "resource_server_1"
                },

            };

            if (await manager.FindByClientIdAsync(descriptor.ClientId) is null)
            {
                await manager.CreateAsync(descriptor);
            }
        }

        public async Task AddMVCClient()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // TODO: Seed Web client

            var client = await manager.FindByClientIdAsync("mvc-client");

            if (client != null)
            {
                await manager.DeleteAsync(client);
            }

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "mvc-client",
                ClientSecret = "xbxEvLyJN0lS28IW6FnC77Y4YSAlBGFS",
                ConsentType = ConsentTypes.Implicit,
                DisplayName = "MVC Client Application",
                RedirectUris =
                    {
                        new Uri("https://localhost:7274/signin-oidc"),
                    },
                PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:7274/"),
                    },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.EndSession,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "api",
                    Permissions.Prefixes.Scope + "resource_server_1"

                },
            };

            if (await manager.FindByClientIdAsync(descriptor.ClientId) is null)
            {
                await manager.CreateAsync(descriptor);
            }
        }

        public async Task AddReactClient() 
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // TODO: Seed react client

            var react_client = await manager.FindByClientIdAsync("react-client");

            if (react_client != null)
            {
                await manager.DeleteAsync(react_client);
            }

            var react_descriptor = new OpenIddictApplicationDescriptor
            {
            ClientId = "react-client",
            ConsentType = ConsentTypes.Implicit,
            DisplayName = "React Client Application",
            RedirectUris =
                {
                    new Uri("https://localhost:7054/signin-oidc"),
                },
            PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7054/")
                },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.EndSession,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Prefixes.Scope + "api",
                Permissions.Prefixes.Scope + "resource_server_1"
            },

            };

            if (await manager.FindByClientIdAsync(react_descriptor.ClientId) is null)
            {
            await manager.CreateAsync(react_descriptor);
            }
        }

        public async Task AddScopeAsync(string scopeName)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

            if (await scopeManager.FindByNameAsync(scopeName) is null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = scopeName,
                    DisplayName = $"{scopeName.Replace('_', ' ').ToUpperInvariant()} API",
                    Resources = { scopeName } // 👈 This defines the "aud" claim for the access token
                });
            }
        }
        

        public async Task SeedUsersAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles if they don't exist
            foreach (var role in new[] { "Admin", "Customer" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user
            var adminEmail = "admin@example.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create customer user
            var customerEmail = "user@example.com";
            var customer = await userManager.FindByEmailAsync(customerEmail);
            if (customer == null)
            {
                customer = new IdentityUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(customer, "Customer123!");
                await userManager.AddToRoleAsync(customer, "Customer");
            }
        }


    }
}