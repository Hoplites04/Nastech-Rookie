namespace ResourceServer.Models
{
    public class PhoneImage
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        //* ===== Image infomration =====
        public string ImageUrl { get; set; }      // e.g., "https://example.com/image.jpg"
        public bool IsMain { get; set; }           // e.g., "true"
        public int DisplayOrder { get; set; }      // eg ., "1"
    }
}