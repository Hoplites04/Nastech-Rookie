using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace AuthServer.Pages;

[Authorize]
public class Consent : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public Consent(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string grant)
    {
        if (string.IsNullOrEmpty(grant))
        {
            return BadRequest("Consent value is required.");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Create new claims identity with the consent claim added
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        var identity = (ClaimsIdentity)principal.Identity!;

        identity.AddClaim(new Claim(Consts.ConsentNaming, grant));

        // Re-sign in user with updated claims
        await _signInManager.SignOutAsync();
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        {
            return Redirect(ReturnUrl);
        }

        return RedirectToPage("/Index");
        // User.SetClaim(Consts.ConsentNaming, grant);
        // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
        // return Redirect(ReturnUrl);
    }

}