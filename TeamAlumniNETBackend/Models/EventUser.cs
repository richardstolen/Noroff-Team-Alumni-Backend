using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class EventUser
    {
        [Key]
        public int Id { get; set; }
        public Event? Event { get; set; }
        public int? EventId { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public bool Accepted { get; set; } = false;
    }
}
