// using Microsoft.AspNetCore.Mvc;
// using ResourceServer.Data;
// using ResourceServer.Services



// namespace ResourceServer.Controllers 
// {
//     [ApiController]
//     [Route("api/[controller]")]

//     public class BrandController : ControllerBase
//     {
//         private readonly IBrandService _brandService;

//         public BrandsController(IBrandService brandService)
//         {
//             _brandService = brandService;
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] BrandCreateDto dto)
//         {
//             if (!ModelState.IsValid)
//                 return BadRequest(ModelState);

//             var brandId = await _brandService.CreateBrandAsync(dto.Name);

//             return Ok(new { message = "New brand created", brandId });
//         }

//     }
// }