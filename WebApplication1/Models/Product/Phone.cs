namespace WebApplication1.Models.Product
{
    public class PhoneModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int PhoneBrandId { get; set; }
    }
}
