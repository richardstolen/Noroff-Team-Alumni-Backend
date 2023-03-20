using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamAlumniNETBackend.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Topic>? Topics { get; set; } = new List<Topic>();
        public ICollection<Group>? Groups { get; set; } = new List<Group>();
        public ICollection<EventUser>? Users { get; set; } = new List<EventUser>();
    }
}
