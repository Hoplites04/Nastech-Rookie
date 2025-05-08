using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


public class AuthenticationController : Controller
{
    [HttpGet]
    public IActionResult Signup()
    {
        return View(); // Looks for Views/Authentication/Signup.cshtml
    }
}