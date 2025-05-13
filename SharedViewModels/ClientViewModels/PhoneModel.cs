namespace SharedViewModels.ClientViewModels;

public class PhoneModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int PhoneBrandId { get; set; }

    public string PhoneBrandName { get; set; } = string.Empty;

    public string? MainImageUrl { get; set; }
}