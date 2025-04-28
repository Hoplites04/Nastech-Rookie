namespace ResourceServer.Mapping.BrandDto
{
    public class BrandCreateDto
    {
        [Required]
        [StringLength(100)]
        public string brandName { get; set; } = string.Empty;
    }
}