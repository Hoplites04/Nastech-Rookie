namespace SharedViewModels.ViewModels;

public class PhoneVariant
{
    public int Id { get; set; }
    public int PhoneId { get; set; }
    public Phone Phone { get; set; }

    //* ===== Variant =====
    public string Color { get; set; } // e.g., "Black"
    public string Storage { get; set; } // e.g., "128 GB"
    public decimal Price { get; set; } // e.g., "37.990.000 ₫"., "AMOLED, 120Hz, HDR10+, 1000 nits (peak)"
}
