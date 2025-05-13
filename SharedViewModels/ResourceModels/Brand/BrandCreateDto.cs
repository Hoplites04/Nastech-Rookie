using System.ComponentModel.DataAnnotations;
namespace SharedViewModels.ResourceModels.Brand;

public class BrandCreateDto
{
    [Required]
    [StringLength(100)]
    public string brandName { get; set; } = string.Empty;
}
