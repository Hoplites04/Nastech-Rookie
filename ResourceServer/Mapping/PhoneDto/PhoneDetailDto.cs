namespace ResourceServer.Mapping.PhoneDto
{
    public class PhoneDetailDto
    {
        public int PhoneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }

        public List<string> AvailableColors { get; set; } = new();
        public List<string> AvailableStorages { get; set; } = new();

        public List<PhoneVariantDto> Variants { get; set; } = new();
    }

    public class PhoneVariantDto
    {
        public int VariantId { get; set; }
        public string Color { get; set; }
        public string Storage { get; set; }
        public decimal Price { get; set; }
    }
}