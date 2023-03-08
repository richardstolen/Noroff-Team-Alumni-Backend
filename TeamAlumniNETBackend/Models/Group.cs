namespace TeamAlumniNETBackend.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Event> Events { get; set; }
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
    }
}
