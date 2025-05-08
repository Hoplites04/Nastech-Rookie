using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Server.AspNetCore;

namespace AuthServer.Pages
{
    public class RegisterPageModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterPageModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string ConfirmPassword { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"Registering user with email: {Email}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine($"Invalid model state: {ModelState}");
                return Page();
            }
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            var existingUser = await _userManager.FindByEmailAsync(Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Email,
                Email = Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            // Ensure "Customer" role exists
            if (!await _roleManager.RoleExistsAsync("Customer"))
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

            await _userManager.AddToRoleAsync(user, "Customer");

            ViewData["Success"] = true;
            return Page(); // ✅ Stay on page to show success message
        }
    }
}
