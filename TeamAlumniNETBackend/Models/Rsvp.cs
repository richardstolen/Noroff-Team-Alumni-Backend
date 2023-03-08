using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Rsvp
    {
        
        public Event EventId { get; set; }
        public int UserId { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Accepted { get; set; } = false;
        public int? GuestCount { get; set; }
    }
}
