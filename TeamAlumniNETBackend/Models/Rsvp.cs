namespace TeamAlumniNETBackend.Models
{
    public class Rsvp
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Accepted { get; set; }
        public int GuestCount { get; set; }
    }
}
