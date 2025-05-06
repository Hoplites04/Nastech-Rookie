using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Product;

namespace WebApplication1.Controllers;

public class ProductDetailController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public ProductDetailController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        var client = _clientFactory.CreateClient("ResourceServer");
        var response = await client.GetAsync($"https://localhost:7251/api/Phone/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound(); // or a custom error view
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var model = await JsonSerializer.DeserializeAsync<PhoneDetailModel>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // To match camelCase from JSON
            }
        );

        return View(model); // Will use Views/ProductDetail/Index.cshtml
    }
}
