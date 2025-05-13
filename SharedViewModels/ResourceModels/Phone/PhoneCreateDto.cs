namespace SharedViewModels.ResourceModels.Phone;
using System.ComponentModel.DataAnnotations;

public class PhoneCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "PhoneBrandId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhoneBrandId must be a positive integer.")]
    public int PhoneBrandId { get; set; }
}