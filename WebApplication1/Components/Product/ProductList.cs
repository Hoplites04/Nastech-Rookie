using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SharedViewModels.ClientViewModels;

namespace WebApplication1.Components.Product
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductListViewComponent(
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

            // Extract brandIds from query string
            var query = _httpContextAccessor.HttpContext?.Request?.Query;
            var brandIds =
                query
                    ?["brandIds"].Select(id => int.TryParse(id, out var v) ? v : (int?)null)
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .ToList() ?? new List<int>();

            // Build query string to call API
            string queryString = brandIds.Any()
                ? $"?{string.Join("&", brandIds.Select(id => $"brandIds={id}"))}"
                : "";

            var response = await client.GetAsync($"https://localhost:7251/api/Phone{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<PhoneModel>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var phones = JsonSerializer.Deserialize<List<PhoneModel>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return View(phones);
        }
    }
}
