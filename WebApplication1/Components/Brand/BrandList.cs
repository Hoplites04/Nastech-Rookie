using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Brand;

namespace WebApplication1.Components.Brand
{
    public class BrandListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrandListViewComponent(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _clientFactory.CreateClient("ResourceServer");

            var response = await client.GetAsync("https://localhost:7251/api/Brand");

            var brands = new List<BrandModel>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                brands =
                    JsonSerializer.Deserialize<List<BrandModel>>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    ) ?? new();
            }

            // Get selected brandIds from query
            var query = _httpContextAccessor.HttpContext?.Request?.Query;
            var selectedBrandIds =
                query
                    ?["brandIds"].Select(id => int.TryParse(id, out var val) ? val : (int?)null)
                    .Where(val => val.HasValue)
                    .Select(val => val.Value)
                    .ToList() ?? new();

            return View(
                new BrandListModel { Brands = brands, SelectedBrandIds = selectedBrandIds }
            );
        }
    }
}
