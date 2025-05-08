using Microsoft.AspNetCore.Mvc;
using ResourceServer.Database;
using ResourceServer.Models;
using ResourceServer.Mapping.BrandDto;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace ResourceServer.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]

    public class BrandController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<BrandController> _logger;

        // private readonly IBrandService _brandService;

        public BrandController(ILogger<BrandController> logger, ApplicationDbContext context)
        {
            // _brandService = brandService;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // var brandId = await _brandService.CreateBrandAsync(dto.Name);
            _logger.LogInformation("Received Create Brand request: {@BrandCreateDto}", dto);

            var brand = new PhoneBrand
            {
                Name = dto.brandName
            };

            _context.PhoneBrands.Add(brand);

            await _context.SaveChangesAsync();

            return Ok(new { message = "New brand created", brand.Id, brand.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Received Delete Brand request for ID: {BrandId}", id);

            var brand = await _context.PhoneBrands.FindAsync(id);
            if (brand == null)
            {
                _logger.LogWarning("Brand ID {BrandId} not found for deletion.", id);
                return NotFound(new { message = $"Brand with ID {id} not found." });
            }

            _context.PhoneBrands.Remove(brand);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Brand ID {BrandId} deleted successfully.", id);
            return Ok(new { message = $"Brand with ID {id} deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.PhoneBrands.Select(b => new
            {
                b.Id,
                b.Name
            }).ToListAsync();

            return Ok(brands);
        }
    }
}

