using Microsoft.AspNetCore.Mvc;
using ResourceServer.Database;
using ResourceServer.Services;
using ResourceServer.Models;
using ResourceServer.Mapping.PhoneDto;
using Microsoft.EntityFrameworkCore;


namespace ResourceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhoneController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhones([FromQuery] int? brandId, [FromQuery] List<int> brandIds)
        {
            var query = _context.Phones.AsQueryable();

            if (brandIds != null && brandIds.Count > 0)
            {
                query = query.Where(p => brandIds.Contains(p.PhoneBrandId));
            }
            else if (brandId.HasValue)
            {
                query = query.Where(p => p.PhoneBrandId == brandId.Value);
            }

            var result = await query.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.PhoneBrandId
            }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoneDetail(int id)
        {
            var phone = await _context.Phones
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phone == null)
            {
                return NotFound();
            }

            // Load Brand Name manually based on PhoneBrandId
            var brand = await _context.PhoneBrands
                .FirstOrDefaultAsync(b => b.Id == phone.PhoneBrandId);

            var variants = await _context.PhoneVariants
                .Where(v => v.PhoneId == id)
                .ToListAsync();

            var detail = new PhoneDetailDto
            {
                PhoneId = phone.Id,
                Name = phone.Name,
                Description = phone.Description,
                BrandName = brand?.Name ?? "Unknown",

                AvailableColors = variants.Select(v => v.Color).Distinct().ToList(),
                AvailableStorages = variants.Select(v => v.Storage).Distinct().ToList(),

                Variants = variants.Select(v => new PhoneVariantDto
                {
                    VariantId = v.Id,
                    Color = v.Color,
                    Storage = v.Storage,
                    Price = v.Price
                }).ToList()
            };

            return Ok(detail);
        }
    }
}
