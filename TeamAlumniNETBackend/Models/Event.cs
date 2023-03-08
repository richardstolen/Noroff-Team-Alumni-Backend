using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string? CreatedBy { get; set; }
        public ICollection<Rsvp>? Rsvps { get; set; }
        public ICollection<Topic>? Topics { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
