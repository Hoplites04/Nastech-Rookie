using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthServer.Pages
{
    public class AuthenticationModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; } 
        [BindProperty]
        public string? ReturnUrl { get; set; }

        public string AuthStatus { get; set; } = "";

        public IActionResult OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, Password))
            {
                AuthStatus = "Email or password is invalid";
                return Page();
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal); // Razor Pages
            


            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToPage("/Index");
        }
    }
}