using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsPrivate { get; set; }
        public ICollection<Event>? Events { get; set; } = new List<Event>();
        public ICollection<User>? Users { get; set; } = new List<User>();

        public override string? ToString()
        {
            return $"Groupid: {GroupId}, Name: {Name}";
        }
    }
}
