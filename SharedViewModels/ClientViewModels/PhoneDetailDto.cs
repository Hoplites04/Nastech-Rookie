namespace SharedViewModels.ClientViewModels;

public class PhoneDetailModel
{
    public int PhoneId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BrandName { get; set; }

    // From PhoneVariants
    public List<string> AvailableColors { get; set; } = new();
    public List<string> AvailableStorages { get; set; } = new();

    public List<PhoneVariantItem> Variants { get; set; } = new();
}

public class PhoneVariantItem
{
    public int Id { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Storage { get; set; } = string.Empty;
    public decimal Price { get; set; }
}