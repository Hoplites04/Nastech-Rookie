namespace ResourceServer.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PhoneBrandId { get; set; }
        public PhoneBrand PhoneBrand { get; set; }

        //! ===== Relation =====
        public PhoneSpecification Specification { get; set; }
        public List<PhoneRating> Ratings { get; set; }
        public List<PhoneVariant> Variants { get; set; }
        public List<PhoneImage> Images { get; set; }
    }
}