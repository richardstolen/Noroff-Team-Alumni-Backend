using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string? CreatedBy { get; set; }
        public List<Rsvp>? Rsvps { get; set; }
        public List<Topic>? Topics { get; set; }
        public List<Group>? Groups { get; set; }
        public List<User>? Users { get; set; }
    }
}
