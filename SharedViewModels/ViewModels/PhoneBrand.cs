namespace SharedViewModels.ViewModels;

public class PhoneBrand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    //! ===== Relation =====
    public List<Phone> Phones { get; set; } = new List<Phone>();
}