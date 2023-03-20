using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.DTOs.UserDTOs
{
    public class UserGetDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public string? Bio { get; set; }
        public string? FunFact { get; set; }
        public ICollection<Group>? Groups { get; set; } = new List<Group>();
        public ICollection<Event>? Events { get; set; } = new List<Event>();
        public ICollection<Topic>? Topics { get; set; } = new List<Topic>();
    }
}
