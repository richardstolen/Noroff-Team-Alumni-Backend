using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamAlumniNETBackend.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public string? Bio { get; set; }
        public string? FunFact { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<Event>? Events { get; set; }
        public ICollection<Topic>? Topics { get; set; }
    }
}
