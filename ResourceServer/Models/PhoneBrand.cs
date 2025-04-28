namespace ResourceServer.Models
{
    public class PhoneBrand 
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Apple"
        public DateTime CreatedAt { get; set; } // e.g., "2023-09-01"

        //! ===== Relation =====
        public List<Phone> Phones { get; set; }
    }
}