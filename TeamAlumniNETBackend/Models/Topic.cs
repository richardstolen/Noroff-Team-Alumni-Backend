using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<User>? Users { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Event>? Events { get; set; }
    }
}
