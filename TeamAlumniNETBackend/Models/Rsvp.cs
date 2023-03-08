using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId { get; set; }
        public Event? Event { get; set; }
        public User? User { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Accepted { get; set; } = false;
        public int? GuestCount { get; set; }
    }
}
