namespace ResourceServer.Models
{
    public class PhoneRating
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        //* ===== Rating =====
        public decimal Score { get; set; } // e.g., "4.5"
        public int Count { get; set; } // e.g., "1000"
        public string Review { get; set; } // e.g., "Excellent phone!"
        public string ReviewerName { get; set; } // e.g., "John Doe"
        public DateTime ReviewDate { get; set; } // e.g., "2023-10-01"
    }
}