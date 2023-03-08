namespace TeamAlumniNETBackend.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<Event> Events { get; set; }
    }
}
