using SharedViewModels.ViewModels;
using ResourceServer.Database;
using Microsoft.EntityFrameworkCore;

public static class SeedHelper
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var existing = await context.PhoneImages.ToListAsync();
        context.PhoneImages.RemoveRange(existing);
        context.PhoneImages.AddRange(new[]
        {
            new PhoneImage
            {
                PhoneId = 1,
                ImageUrl = "/Images/iPhone16/iPhone16ProMax/iphone_16_pro_max_desert_titan_3552a28ae0.webp",
                IsMain = true,
                DisplayOrder = 0
            },
            new PhoneImage
            {
                PhoneId = 1,
                ImageUrl = "/Images/iPhone16/iPhone16ProMax/iphone_16_pro_max_desert_titan_2_47871ff517.webp",
                IsMain = false,
                DisplayOrder = 1
            },
            new PhoneImage
            {
                PhoneId = 1,
                ImageUrl = "Images/iPhone16/iPhone16ProMax/iphone_16_pro_max_desert_titan_3_19817f66c3.webp",
                IsMain = false,
                DisplayOrder = 2
            },
            new PhoneImage
            {
                PhoneId = 1,
                ImageUrl = "Images/iPhone16/iPhone16ProMax/iphone_16_pro_max_desert_titan_4_15e6458928.webp",
                IsMain = false,
                DisplayOrder = 3
            },
        });
        await context.SaveChangesAsync();
    }
}
