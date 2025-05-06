using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View(); // Views/Login/Index.cshtml
    }
}
