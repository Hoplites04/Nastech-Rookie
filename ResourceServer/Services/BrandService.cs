// using ResourceServer.Data;
// using ResourceServer.Models;

// namespace ResourceServer.Services
// {
//     public class BrandService : IBrandService
//     {
//         private readonly ApplicationDbContext _context;

//         public BrandService(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<int> CreateBrandAsync(string name)
//         {
//             var brand = new PhoneBrand
//             {
//                 Name = name
//             };

//             _context.PhoneBrand.Add(brand);
//             await _context.SaveChangesAsync();

//             return brand.Id;
                
//         }
//     }
// }
