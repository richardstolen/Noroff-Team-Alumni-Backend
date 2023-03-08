using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
