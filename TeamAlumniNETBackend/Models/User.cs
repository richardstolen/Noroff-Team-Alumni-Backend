using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TeamAlumniNETBackend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Bio { get; set; }
        public string FunFact { get; set; }
        public List<Post> Posts { get; set; }
        public List <Group> Groups { get; set; }
        public List<Event> Events { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
