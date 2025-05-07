using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;


namespace ResourceServer.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationTestController : ControllerBase
        {
            private readonly ILogger<AuthorizationTestController> _logger;

            public AuthorizationTestController(ILogger<AuthorizationTestController> logger)
            {
                _logger = logger;
            }

            [HttpGet("test")]
            [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Customer")]
            public IActionResult WhoAmI()
            {
                return Ok(new 
                { 
                    Message = "Authorization successful", 
                    User = User.Identity?.Name, 
                    Roles = User.Claims
                                .Where(c => c.Type == "role")
                                .Select(c => c.Value),
                    Claims = User.Claims.Select(c => new { c.Type, c.Value })
                });
            }
    }
}