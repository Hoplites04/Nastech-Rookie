using Microsoft.AspNetCore.Mvc;
using ResourceServer.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedViewModels.ResourceModels.Phone;
using SharedViewModels.ClientViewModels;
using SharedViewModels.ViewModels;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;



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
            var query = _context.Phones
                .Include(p => p.PhoneBrand)
                .Include(p => p.Images)
                .AsQueryable();

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
                p.PhoneBrandId,
                BrandName = p.PhoneBrand.Name,
                MainImageUrl = p.Images
                .Where(img => img.IsMain)
                .OrderBy(img => img.DisplayOrder)
                .Select(img => img.ImageUrl)
                .FirstOrDefault()
            }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("detail/{id}")]
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
            
            var images = await _context.PhoneImages
                .Where(img => img.PhoneId == id)
                .OrderBy(img => img.DisplayOrder)
                .ToListAsync();

            var detail = new PhoneDetailModel
            {
                PhoneId = phone.Id,
                Name = phone.Name,
                Description = phone.Description,
                BrandName = brand?.Name ?? "Unknown",

                AvailableColors = variants.Select(v => v.Color).Distinct().ToList(),
                AvailableStorages = variants.Select(v => v.Storage).Distinct().ToList(),

                Variants = variants.Select(v => new PhoneVariantItem
                {
                    Id = v.Id,
                    Color = v.Color,
                    Storage = v.Storage,
                    Price = v.Price
                }).ToList(),

                Images = images.Select(img => new PhoneImageItem
                {
                    ImageUrl = $"https://localhost:7251/{img.ImageUrl}",
                    IsMain = img.IsMain,
                    DisplayOrder = img.DisplayOrder
                }).ToList(),
            };

            return Ok(detail);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddPhone([FromBody] PhoneCreateDto phoneDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var phone = new Phone
            {
                Name = phoneDto.Name,
                Description = phoneDto.Description,
                CreatedAt = DateTime.UtcNow,
                PhoneBrandId = phoneDto.PhoneBrandId
            };

            await _context.Phones.AddAsync(phone);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhoneDetail), new { id = phone.Id }, phone);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            var phone = await _context.Phones.FindAsync(id);

            if (phone == null)
                return NotFound("Phone not found.");

            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 response
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoneById(int id)
        {
            var phone = await _context.Phones
                .Include(p => p.PhoneBrand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phone == null)
            {
                return NotFound("Phone not found.");
            }

            return Ok(new
            {
                phone.Id,
                phone.Name,
                phone.Description,
                phone.PhoneBrandId,
                BrandName = phone.PhoneBrand.Name
            });
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatePhone(int id, [FromBody] PhoneCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var phone = await _context.Phones.FindAsync(id);

            if (phone == null)
                return NotFound("Phone not found.");

            // Update fields
            phone.Name = dto.Name;
            phone.Description = dto.Description;
            phone.PhoneBrandId = dto.PhoneBrandId;

            await _context.SaveChangesAsync();

            return NoContent(); // 204 success response
}

    }
}
